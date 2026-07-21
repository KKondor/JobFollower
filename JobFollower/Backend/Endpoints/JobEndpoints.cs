using JobFollower.Backend.Model;
using JobFollower.Backend.Model.DTO;
using JobFollower.Backend.Service;
using JobFollower.Backend.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;

namespace JobFollower.Backend.Endpoints
{
    public static class JobEndpoints
    {
        public static void MapJobEndpoints(this IEndpointRouteBuilder group)
        {
            group.MapGet("/",GetFilteredJobsAsync);
            group.MapGet("/{id}", GetJobById);
            group.MapDelete("/{id}", DeleteJob);
            //group.MapPost("/", CreateJob);
            group.MapPatch("/{id}", PatchJob);
        }
        static async Task<Ok<List<JobApplicationDto>>> GetAllJobsAsync(IJobService jobService)
        {
            return TypedResults.Ok(await jobService.GetAllJobsAsync());
        }

        static async Task<Ok<List<JobApplicationDto>>> GetFilteredJobsAsync(int userId, string? name, JobApplication.StatusState? status, IJobService jobService)
        {
            return TypedResults.Ok(await jobService.GetFilteredJobsAsync(userId, name, status));
        }
        static async Task<Results<Ok<JobApplicationDto>, NotFound>> GetJobById(int id, IJobService jobService) 
        {
            return await jobService.GetJobApplicationByIdAsync(id)
                is JobApplicationDto jobApplication
                    ? TypedResults.Ok(jobApplication)
                    : TypedResults.NotFound();
        }
        //TODO This has an invalid User input that the minimal api can't handle have to fix with auth.
        static async Task<Results<Created<JobApplicationDto>, ValidationProblem>> CreateJob(JobApplicationDto job, User user, IJobService service)
        {
            var errors = ValidationHelper.Validate(job);
            if (errors.Count > 0)
            {
                var problemDict = ValidationHelper.ToValidationDictionary(errors);
                return TypedResults.ValidationProblem(problemDict);
            }

            var savedTodo = await service.CreateJobAsync(job,user);

            return TypedResults.Created($"/{savedTodo.JobId}",savedTodo);
        }
        static async Task<Results<Created<JobApplicationDto>,NotFound,ValidationProblem>> PatchJob(int id, JobPatchDto jobPatch, IJobService service)
        {
            var errors = ValidationHelper.Validate(jobPatch);
            if (errors.Count > 0)
            {
                var problemDict = ValidationHelper.ToValidationDictionary(errors);
                return TypedResults.ValidationProblem(problemDict);
            }

            var patchedJob = await service.PatchJobAsync(id, jobPatch);

            if (patchedJob == null) return TypedResults.NotFound();
            return TypedResults.Created($"/{patchedJob.JobId}", patchedJob);
        }
        static async Task<Results<NoContent,NotFound>> DeleteJob(int id, IJobService service)
        {
            bool result = await service.DeleteJobAsync(id);
            if (result) return TypedResults.NoContent();
            return TypedResults.NotFound();
        }
    }   
}
