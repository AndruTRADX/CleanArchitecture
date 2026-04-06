using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CleanArchitecture.Application.Contracts.Identity;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Application.Models.Identity.Request;
using CleanArchitecture.Application.Models.Identity.Response;
using CleanArchitecture.Application.Models.Identity.Settings;
using CleanArchitecture.Identity.Models;
using CleanArchitecture.Identity.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Identity.Services;

public class AuthService(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    IOptions<JwtSettings> jwtSettings,
    CAIdentityDbContext context,
    TokenValidationParameters validationParameters) : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;
    private readonly CAIdentityDbContext _context = context;
    private readonly TokenValidationParameters _tokenValidationParameters = validationParameters;

    private static class ClaimTypesConstants
    {
        public const string Id = "Id";
        public const string Jti = JwtRegisteredClaimNames.Jti;
        public const string Email = JwtRegisteredClaimNames.Email;
        public const string Sub = JwtRegisteredClaimNames.Sub;
    }

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email)
            ?? throw new NotFoundException(nameof(IdentityUser), request.Email);

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName ?? string.Empty,
            request.Password,
            isPersistent: false,
            lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            throw new BadRequestException("Invalid credentials");
        }

        var (token, refreshToken) = await GenerateTokenAsync(user);

        return new AuthResponse
        {
            Id = user.Id,
            Token = token,
            Email = user.Email ?? string.Empty,
            Username = user.UserName ?? string.Empty,
            RefreshToken = refreshToken
        };
    }

    public async Task<AuthResponse> RefreshToken(TokenRequest request)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var tokenValidationParamsClone = _tokenValidationParameters.Clone();
        tokenValidationParamsClone.ValidateLifetime = false;

        try
        {
            var principal = jwtTokenHandler.ValidateToken(
                request.Token,
                tokenValidationParamsClone,
                out var validatedToken);

            if (validatedToken is not JwtSecurityToken jwtSecurityToken ||
                !string.Equals(jwtSecurityToken.Header.Alg, SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return CreateErrorResponse("Token has encryption errors");
            }

            var expClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
            if (expClaim is null || !long.TryParse(expClaim.Value, out var utcExpiryDate))
            {
                return CreateErrorResponse("Token doesn't have a valid expiration date");
            }

            var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);

            if (expiryDate > DateTime.UtcNow)
            {
                return CreateErrorResponse("Token has not expired yet");
            }

            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);

            if (storedToken is null)
            {
                return CreateErrorResponse("Token doesn't exist");
            }

            if (storedToken.IsBeingUsed)
            {
                return CreateErrorResponse("Token has already been used");
            }

            if (storedToken.HasBeenRevoked)
            {
                return CreateErrorResponse("Revoked token");
            }

            var jtiClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
            if (jtiClaim is null || string.IsNullOrWhiteSpace(jtiClaim.Value))
            {
                return CreateErrorResponse("Invalid JTI identifier");
            }

            var jti = jtiClaim.Value;
            if (storedToken.JwtId != jti)
            {
                return CreateErrorResponse("Token doesn't match the initial value");
            }

            if (storedToken.ExpireDate < DateTime.UtcNow)
            {
                return CreateErrorResponse("Refresh token has expired");
            }

            storedToken.IsBeingUsed = true;
            _context.RefreshTokens.Update(storedToken);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(storedToken.UserId);

            if (user is null)
            {
                return CreateErrorResponse("User associated with the refresh token doesn't exist");
            }

            var (newToken, newRefreshToken) = await GenerateTokenAsync(user);

            return new AuthResponse
            {
                Id = user.Id,
                Token = newToken,
                Email = user.Email ?? string.Empty,
                Username = user.UserName ?? string.Empty,
                RefreshToken = newRefreshToken,
                Success = true
            };
        }
        catch (Exception ex) when (ex.Message.Contains("Lifetime validation failed. The token is expired"))
        {
            return CreateErrorResponse("Session expired, please log in");
        }
        catch
        {
            return CreateErrorResponse("Session corrupted, please log in");
        }
    }

    public async Task<RegistrationResponse> Register(RegistrationRequest request)
    {
        var existingUser = await _userManager.FindByNameAsync(request.Username);

        if (existingUser is not null)
        {
            throw new BadRequestException("Username already taken");
        }

        var existingEmail = await _userManager.FindByEmailAsync(request.Email);
        if (existingEmail is not null)
        {
            throw new BadRequestException("Email already taken");
        }

        var user = new IdentityUser
        {
            Email = request.Email,
            UserName = request.Username,
            EmailConfirmed = true
        };

        var createResult = await _userManager.CreateAsync(user, request.Password);

        if (!createResult.Succeeded)
        {
            throw new BadRequestException(string.Join(", ", createResult.Errors.Select(e => e.Description)));
        }

        var applicationUser = new ApplicationUser
        {
            IdentityId = new Guid(user.Id),
            Name = request.Name,
            LastName = request.LastName,
            Country = request.Country,
            Email = request.Email,
            Phone = request.Phone,
        };

        _context.ApplicationUsers.Add(applicationUser);
        await _context.SaveChangesAsync();

        var (token, refreshToken) = await GenerateTokenAsync(user);

        return new RegistrationResponse
        {
            Email = user.Email,
            Token = token,
            UserId = user.Id,
            Username = user.UserName,
            RefreshToken = refreshToken
        };
    }

    // ========== Private helper methods ==========
    private static AuthResponse CreateErrorResponse(string errorMessage) =>
        new() { Success = false, Errors = [errorMessage] };

    private async Task<(string Token, string RefreshToken)> GenerateTokenAsync(IdentityUser user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var symmetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Key));

        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();

        var claims = new List<Claim>
        {
            new(ClaimTypesConstants.Id, user.Id),
            new(ClaimTypesConstants.Email, user.Email ?? string.Empty),
            new(ClaimTypesConstants.Sub, user.Email ?? string.Empty),
            new(ClaimTypesConstants.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(userClaims);
        claims.AddRange(roleClaims);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_jwtSettings.ExpireTime),
            SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);

        var refreshToken = new RefreshToken
        {
            JwtId = token.Id,
            IsBeingUsed = false,
            HasBeenRevoked = false,
            UserId = user.Id,
            CreatedDate = DateTime.UtcNow,
            ExpireDate = DateTime.UtcNow.AddMonths(6),
            Token = $"{GenerateCryptoRandomString(35)}{Guid.NewGuid()}"
        };

        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();

        return (jwtToken, refreshToken.Token);
    }

    private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return dateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
    }

    private static string GenerateCryptoRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var randomBytes = RandomNumberGenerator.GetBytes(length);
        var result = new char[length];

        for (int i = 0; i < length; i++)
        {
            result[i] = chars[randomBytes[i] % chars.Length];
        }

        return new string(result);
    }
}
