using System.ComponentModel.DataAnnotations;

namespace JobFollower.Backend.Model.DTO
{
    public class UserDto
    {
        [Key]
        public int UserId { get; set; }
        [MaxLength(30)]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        public UserDto() { }
        public UserDto(User user) => (UserId,Name,Email) = (user.UserId,user.Name, user.Email);
    }
}
