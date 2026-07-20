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
        public required User User { get; set; }
        public required String JobName { get; set; }
        public String? JobDescription { get; set; }
        public StatusState Status { get; set; } = StatusState.NotApplied;
        public DateTime AppliedDate { get; set; }

    }
}
