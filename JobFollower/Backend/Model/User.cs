using System.ComponentModel.DataAnnotations;

namespace JobFollower.Backend.Model
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public required String Name { get; set; }
        public required String Email { get; set; }
        public required String HashedPassword { get; set; }
    }
}
