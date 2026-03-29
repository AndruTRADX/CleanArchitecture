using System;
using CleanArchitecture.Application.Models.Identity.Request;
using CleanArchitecture.Application.Models.Identity.Response;

namespace CleanArchitecture.Application.Contracts.Identity;

public interface IAuthService
{
    Task<AuthResponse> Login(AuthRequest request);
    Task<RegistrationResponse> Register(RegistrationRequest request);
    
}
