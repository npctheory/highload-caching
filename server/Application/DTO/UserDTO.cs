using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTO;

public record UserDTO(
    string Id,
    string FirstName,
    string SecondName,
    DateTime Birthdate,
    string Biography,
    string City
);