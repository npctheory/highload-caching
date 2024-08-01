using System.ComponentModel.DataAnnotations;

namespace Api.DTO
{
    public class UserResponse
    {
        [Key]
        [Required]
        [StringLength(255)]
        public required string Id { get; set; }

        [Required]
        [StringLength(255)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(255)]
        public required string SecondName { get; set; }

        [Required]
        public required DateTime Birthdate { get; set; }

        public string? Biography { get; set; }

        [StringLength(255)]
        public string? City { get; set; }
    }
}
