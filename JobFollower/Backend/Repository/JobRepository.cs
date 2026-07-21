using JobFollower.Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace JobFollower.Backend.Repository
{
    public class JobRepository : IJobRepository
    {
        private readonly JobDbContext _jobdb;
        public JobRepository(JobDbContext jobDb) => _jobdb = jobDb;
        public async Task<List<JobApplication>> GetAllJobsAsync() => await _jobdb.JobApplications.ToListAsync();

        public async Task<JobApplication?> GetJobByIdAsync(int userId, int id)
        {
            return await _jobdb.JobApplications.Where(x => x.UserId == userId && x.JobId == id).FirstOrDefaultAsync();
        }
        public async Task<List<JobApplication>> GetJobsByUserId(int userId)
        {
            var querry = _jobdb.JobApplications.AsQueryable();
            var filteredJobs = querry.Where(x => x.UserId == userId);
            return await filteredJobs.ToListAsync();
        }
        public async Task<List<JobApplication>> GetFilteredJobsAsync(int userId, string? name, JobApplication.StatusState? status)
        {
            var querry = _jobdb.JobApplications.AsQueryable();
            querry = querry.Where(job =>  job.UserId == userId);
            if (!string.IsNullOrWhiteSpace(name))
            {
                querry = querry.Where(job => job.JobName.Contains(name));
            }
            if (status.HasValue)
            {
                querry = querry.Where(job => job.Status == status.Value);
            }
            return await querry.ToListAsync();
        }

        public async Task<JobApplication> CreateJobAsync(JobApplication job)
        {
            var result = await _jobdb.JobApplications.AddAsync(job);
            await _jobdb.SaveChangesAsync();
            return job;
        }

        public async Task<JobApplication?> UpdateJobAsync(int userId,int id, JobApplication job)
        {
            var foundJob = await _jobdb.JobApplications.Where(x => x.UserId == userId && x.JobId == id).FirstOrDefaultAsync();
            if (foundJob == null) return null;

            foundJob.JobName = job.JobName;
            foundJob.JobDescription = job.JobDescription;
            foundJob.Status = job.Status;

            await _jobdb.SaveChangesAsync();
            return foundJob;
        }

        public async Task<bool> DeleteJobAsync(int userId,int id)
        {
            var foundJob = await _jobdb.JobApplications.Where(x => x.UserId == userId && x.JobId == id).FirstOrDefaultAsync();
            if (foundJob == null) return false;
            _jobdb.JobApplications.Remove(foundJob);
            await _jobdb.SaveChangesAsync();
            return true;
        }
    }
}
