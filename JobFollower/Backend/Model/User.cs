using System.ComponentModel.DataAnnotations;

namespace JobFollower.Backend.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [MaxLength(30)]
        public required string Name { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        public required string HashedPassword { get; set; }
    }
}
