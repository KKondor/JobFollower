using JobFollower.Backend.Model;
using JobFollower.Backend.Model.DTO;

namespace JobFollower.Backend.Service
{
    public interface IJobService
    {
        Task<List<JobApplicationDto>> GetAllJobsAsync();
        Task<JobApplicationDto?> GetJobApplicationByIdAsync(int userId,int id);
        Task<List<JobApplicationDto>> GetJobApplicationsByUserIdAsync(int userId);
        Task<List<JobApplicationDto>> GetFilteredJobsAsync(int userId, string? name, JobApplication.StatusState? status);
        Task<JobApplicationDto> CreateJobAsync(JobApplicationDto job, int userId);
        Task<JobApplicationDto?> PatchJobAsync(int userId,int id, JobPatchDto job);
        Task<bool> DeleteJobAsync(int userId,int id);
    }
}
