using System.ComponentModel.DataAnnotations;

namespace JobFollower.Backend.Model
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }
    }
}
