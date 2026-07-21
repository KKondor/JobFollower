using System.ComponentModel.DataAnnotations;

namespace JobFollower.Backend.Model.DTO
{
    public class RegisterUserDto
    {
        [Key]
        public int UserId { get; set; }
        [MaxLength(30)]
        public required String Name { get; set; }
        [EmailAddress]
        public required String Email { get; set; }
        public required String Password { get; set; }

        public RegisterUserDto() { }
    }
}
