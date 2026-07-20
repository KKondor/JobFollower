using JobFollower.Backend.Model.DTO;

namespace JobFollower.Backend.Service
{
    public interface IJobService
    {
        Task<List<JobApplicationDto>> GetAllJobsAsync();
    }
}
