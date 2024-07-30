using Api.Common.Interfaces.Authentication;

namespace Api.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{   
    private readonly IJwtTokenGenerator _jwtTokenGerator;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator)
    {
        _jwtTokenGerator = jwtTokenGenerator;
    }


    public AuthenticationResult Login(string id, string password)
    {
        var authResult = new AuthenticationResult(
            id: "123",
            first_name: "John",
            second_name: "Doe",
            birthdate: "2000-01-01",
            biography: "Software Developer",
            city: "New York",
            token: "abcdefg"
        );

        return authResult;
    }

    public AuthenticationResult Register(string first_name, string second_name, string birthdate, string biography, string city, string password)
    {
        string user_id = "randomword001";

        var token = _jwtTokenGerator.GenerateToken(user_id, first_name, second_name);

        var authResult = new AuthenticationResult(
            id: "123",
            first_name: "John",
            second_name: "Doe",
            birthdate: "2000-01-01",
            biography: "Software Developer",
            city: "New York",
            token: token
        );

        return authResult;
    }
}
