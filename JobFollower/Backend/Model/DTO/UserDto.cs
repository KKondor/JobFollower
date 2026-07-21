using System.ComponentModel.DataAnnotations;

namespace JobFollower.Backend.Model.DTO
{
    public class UserDto
    {
        [Key]
        public int UserId { get; set; }
        [MaxLength(30)]
        public String Name { get; set; }
        [EmailAddress]
        public String Email { get; set; }

        public UserDto() { }
        public UserDto(User user) => (UserId,Name,Email) = (user.UserId,user.Name, user.Email);
    }
}
