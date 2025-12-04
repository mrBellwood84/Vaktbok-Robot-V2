namespace WebHarvester.Harvest.Exceptions;

public class BrowserElementNoContentException : Exception
{
    public BrowserElementNoContentException() {}
    public BrowserElementNoContentException(string message) : base(message) {}
    public BrowserElementNoContentException(string message, Exception innerException) : base(message, innerException) {}
}