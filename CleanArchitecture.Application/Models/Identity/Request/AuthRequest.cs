namespace CleanArchitecture.Application.Models.Identity.Request;

public class AuthRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
