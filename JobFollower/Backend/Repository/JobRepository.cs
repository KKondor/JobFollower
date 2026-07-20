using JobFollower.Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace JobFollower.Backend.Repository
{
    public class JobRepository : IJobRepository
    {
        private readonly JobDbContext _jobdb;

        public JobRepository(JobDbContext jobDb) => _jobdb = jobDb;
        public async Task<List<JobApplication>> GetAllJobsAsync() => await _jobdb.JobApplications.ToListAsync();
    }
}
