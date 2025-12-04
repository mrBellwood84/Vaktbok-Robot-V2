namespace WebHarvester.Harvest.Exceptions;

public class BrowserPageTimeoutException : Exception
{
    public BrowserPageTimeoutException() {}
    public BrowserPageTimeoutException(string message) {}
    public BrowserPageTimeoutException(string message, Exception innerException) {}
}