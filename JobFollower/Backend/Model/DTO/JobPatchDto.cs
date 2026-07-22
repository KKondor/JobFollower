using System.ComponentModel.DataAnnotations;
using static JobFollower.Backend.Model.JobApplication;

namespace JobFollower.Backend.Model.DTO
{
    public class JobPatchDto
    {
        [MaxLength(100)]
        public string? JobName { get; set; }
        [MaxLength(10000)]
        public string? JobDescription { get; set; }
        [Range(0,5)]
        public StatusState? Status { get; set; } = StatusState.NotApplied;
        public DateTime? AppliedDate { get; set; }
    }
}
