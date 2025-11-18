namespace Infrastructure.Scraper.Exceptions;

public class BrowserElementNotFoundException : Exception
{
    public BrowserElementNotFoundException() : base() {}
    public BrowserElementNotFoundException(string message) : base(message) {}
    public BrowserElementNotFoundException(string message, Exception innerException) : base(message, innerException) {}
    

}