using JobFollower.Backend.Model;
using JobFollower.Backend.Model.DTO;

namespace JobFollower.Backend.Service
{
    public interface IJobService
    {
        Task<List<JobApplicationDto>> GetAllJobsAsync();
        Task<JobApplicationDto?> GetJobApplicationByIdAsync(int id);
        Task<List<JobApplicationDto>> GetJobApplicationsByUserIdAsync(int userId);
        Task<List<JobApplicationDto>> GetFilteredJobsAsync(int userId, string? name, JobApplication.StatusState? status);
        Task<JobApplicationDto> CreateJobAsync(JobApplicationDto job, User user);
        Task<JobApplicationDto?> PatchJobAsync(int id, JobPatchDto job);
        Task<bool> DeleteJobAsync(int id);
    }
}
