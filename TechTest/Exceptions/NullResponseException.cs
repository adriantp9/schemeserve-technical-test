namespace TechTest.Exceptions;

// TODO: Rename this or remove it.
public class NullResponseException : Exception
{
    public NullResponseException()
    {
    }

    public NullResponseException(string? message) : base(message)
    {
    }

    public NullResponseException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
