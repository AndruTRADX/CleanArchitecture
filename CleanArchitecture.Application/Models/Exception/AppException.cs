using System;

namespace CleanArchitecture.Application.Models.Exception;

public class AppException(int statusCode, string message, string? details)
{
    public int Status { get; set; } = statusCode;
    public string Message { get; set; } = message;
    public string? Details { get; set; } = details;
}
