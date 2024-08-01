using System.Text.Json.Serialization;

namespace Api.DTO;

public record RegisterRequest(
    [property: JsonPropertyName("first_name")] string FirstName,
    [property: JsonPropertyName("second_name")] string SecondName,
    [property: JsonPropertyName("birthdate")] string Birthdate,
    [property: JsonPropertyName("biography")] string Biography,
    [property: JsonPropertyName("city")] string City,
    [property: JsonPropertyName("password")] string Password
);
