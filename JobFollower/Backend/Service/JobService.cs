using JobFollower.Backend.Model.DTO;
using JobFollower.Backend.Repository;

namespace JobFollower.Backend.Service
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository) => _jobRepository = jobRepository;
        public async Task<List<JobApplicationDto>> GetAllJobsAsync()
        {
            var jobs = await _jobRepository.GetAllJobsAsync();
            return jobs.Select(x => new JobApplicationDto(x)).ToList();
        }
    }
}
