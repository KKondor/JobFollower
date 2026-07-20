using System.ComponentModel.DataAnnotations;

namespace JobFollower.Backend.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
    }
}
