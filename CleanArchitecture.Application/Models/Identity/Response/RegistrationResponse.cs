namespace CleanArchitecture.Application.Models.Identity.Response;

public class RegistrationResponse
{
    public string UserId { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}
