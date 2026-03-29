using System;

namespace CleanArchitecture.Application.Exceptions;

public class NotFoundException(string name, object key) : ApplicationException($"Entidad \"{name}\" ({key}) no fue encontrado")
{
    
}
