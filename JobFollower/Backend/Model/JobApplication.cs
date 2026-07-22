using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobFollower.Backend.Model
{
    public class JobApplication
    {
        public enum StatusState
        {
            NotApplied,
            Applied,
            Rejected,
            Ghosted,
            Interviewed,
            Accepted
        }
        [Key]
        public int JobId { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        [MaxLength(100)]
        public required string JobName { get; set; }
        [MaxLength(10000)]
        public string? JobDescription { get; set; }
        [Range(0,5)]
        public StatusState Status { get; set; } = StatusState.NotApplied;
        public DateTime AppliedDate { get; set; }

    }
}
