using JobFollower.Backend.Model;

namespace JobFollower.Backend.Repository
{
    public interface IJobRepository
    {
        Task<List<JobApplication>> GetAllJobsAsync();
        Task<JobApplication?> GetJobByIdAsync(int id);
        Task<List<JobApplication>> GetJobsByUserId(int userId);
        Task<List<JobApplication>> GetFilteredJobsAsync(int userId, string? name, JobApplication.StatusState? status);
        Task<JobApplication> CreateJobAsync(JobApplication job);
        Task<JobApplication?> UpdateJobAsync(int id, JobApplication job);
        Task<bool> DeleteJobAsync(int id);
    }
}
