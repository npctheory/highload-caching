using System.Text.Json.Serialization;

namespace Api.DTO;

public record LoginRequest(
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("password")] string Password
);
