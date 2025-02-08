namespace Roomly.Users.Infrastructure.Exceptions;

public class LoginException : Exception
{
    public LoginException(string message) : base(message){}
    public LoginException() : base("Failed to login"){}
}