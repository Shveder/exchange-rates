namespace Rates.Infrastructure.Exceptions;

public class IncorrectDataException : Exception
{
    public IncorrectDataException(string? message) : base(message)
    {
    }
}