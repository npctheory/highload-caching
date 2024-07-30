namespace Api.Services.Authentication;

public interface IAuthenticationService
{
    AuthenticationResult Login(string id, string password);
    AuthenticationResult Register(string first_name, string second_name, string birthdate, string biography, string city, string password);
}
