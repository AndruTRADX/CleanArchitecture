using System;

namespace CleanArchitecture.Application.Exceptions;

public class BadRequestException(string message = "") : ApplicationException(string.IsNullOrWhiteSpace(message) ? "Invalid credentials" : message)
{

}
