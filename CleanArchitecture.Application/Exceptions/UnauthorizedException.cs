using System;

namespace CleanArchitecture.Application.Exceptions;

public class UnauthorizedException(string message = "") : ApplicationException(string.IsNullOrWhiteSpace(message) ? "Invalid credentials" : message)
{

}
