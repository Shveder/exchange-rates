namespace exchange_rates.Exceptions;

public class IncorrectDataException : Exception
{
    public IncorrectDataException(string? message) : base(message)
    {
    }
}