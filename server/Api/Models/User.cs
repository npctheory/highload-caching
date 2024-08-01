using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class User
{
    [StringLength(255)]
    public string? Id { get; set; }

    [StringLength(255)]
    public string? PasswordHash { get; set; }

    [StringLength(255)]
    public string? FirstName { get; set; }

    [StringLength(255)]
    public string? SecondName { get; set; }

    public DateTime? Birthdate { get; set; }

    public string? Biography { get; set; }

    public string? City { get; set; }
}