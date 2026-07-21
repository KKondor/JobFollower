using JobFollower.Backend.Model;
using JobFollower.Backend.Model.DTO;
using JobFollower.Backend.Repository;

namespace JobFollower.Backend.Service
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService(IJobRepository jobRepository) => _jobRepository = jobRepository;

        public async Task<JobApplicationDto> CreateJobAsync(JobApplicationDto job, User user)
        {
            JobApplication convertedJob = new JobApplication
            {
                JobName = job.JobName,
                JobDescription = job.JobDescription,
                Status = job.Status,
                AppliedDate = job.AppliedDate,
                User = user,
                UserId = user.UserId
            };
            var created = await _jobRepository.CreateJobAsync(convertedJob);
            return new JobApplicationDto(created);
        }

        public async Task<bool> DeleteJobAsync(int id)
        {
            return await _jobRepository.DeleteJobAsync(id);
        }

        public async Task<List<JobApplicationDto>> GetAllJobsAsync()
        {
            var jobs = await _jobRepository.GetAllJobsAsync();
            return jobs.Select(x => new JobApplicationDto(x)).ToList();
        }

        public async Task<List<JobApplicationDto>> GetFilteredJobsAsync(int userId, string? name, JobApplication.StatusState? status)
        {
            var jobs = await _jobRepository.GetFilteredJobsAsync(userId, name, status);
            return jobs.Select(x => new JobApplicationDto(x)).ToList();
        }

        public async Task<JobApplicationDto?> GetJobApplicationByIdAsync(int id)
        {
            var job = await _jobRepository.GetJobByIdAsync(id);
            return job == null ? null : new JobApplicationDto(job);
        }

        public async Task<List<JobApplicationDto>> GetJobApplicationsByUserIdAsync(int userId)
        {
            var jobs = await _jobRepository.GetJobsByUserId(userId);
            return jobs.Select(job => new JobApplicationDto(job)).ToList();
        }

        public async Task<JobApplicationDto?> PatchJobAsync(int id, JobPatchDto job)
        {
            var foundJob = await _jobRepository.GetJobByIdAsync(id);
            if (foundJob == null) return null;

            if (job.JobName is not null) foundJob.JobName = job.JobName;
            if (job.JobDescription is not null) foundJob.JobDescription = job.JobDescription;
            if (job.Status is not null) foundJob.Status = job.Status.Value;
            if (job.AppliedDate is not null) foundJob.AppliedDate = job.AppliedDate.Value;

            var result = await _jobRepository.UpdateJobAsync(id, foundJob);
            if (result == null) return null;
            return new JobApplicationDto(result);
        }
    }
}
