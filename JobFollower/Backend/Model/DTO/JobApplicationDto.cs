using System.ComponentModel.DataAnnotations;
using static JobFollower.Backend.Model.JobApplication;

namespace JobFollower.Backend.Model.DTO
{
    public class JobApplicationDto
    {
        [Key]
        public int JobId { get; set; }
        [MaxLength(100)]
        public string JobName { get; set; }
        [MaxLength(1000)]
        public string? JobDescription { get; set; }
        [Range(0,5)]
        public StatusState Status { get; set; } = StatusState.NotApplied;
        public DateTime AppliedDate { get; set; }

        public JobApplicationDto() { }
        public JobApplicationDto(JobApplication jobApplication) =>
            (JobId, JobName, JobDescription, Status, AppliedDate) =
            (jobApplication.JobId, jobApplication.JobName, jobApplication.JobDescription, jobApplication.Status, jobApplication.AppliedDate);
    }
}
