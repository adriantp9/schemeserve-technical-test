namespace TechTest.Application.Clients.Exceptions;

public class CqcClientRequestException : Exception
{
    public int StatusCode { get; init; }

    public CqcClientRequestException()
    {
    }

    public CqcClientRequestException(string? message) : base(message)
    {
    }

    public CqcClientRequestException(string? message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    public CqcClientRequestException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
