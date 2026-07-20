using JobFollower.Backend.Model;

namespace JobFollower.Backend.Repository
{
    public interface IJobRepository
    {
        Task<List<JobApplication>> GetAllJobsAsync(); 
    }
}
