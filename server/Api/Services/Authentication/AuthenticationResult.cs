namespace Api.Services.Authentication;

public record AuthenticationResult(
    string id,
    string first_name,
    string second_name,
    string birthdate,
    string biography,
    string city,
    string token
);
