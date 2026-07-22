using JobFollower.Backend.Model;
using JobFollower.Backend.Model.DTO;
using JobFollower.Backend.Service;
using JobFollower.Backend.Helpers;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace JobFollower.Backend.Endpoints
{
    public static class JobEndpoints
    {
        public static void MapJobEndpoints(this IEndpointRouteBuilder group)
        {
            group.MapGet("/",GetAllJobsForUserAsync);
            group.MapGet("/{id}", GetJobById);
            group.MapDelete("/{id}", DeleteJob);
            group.MapPost("/", CreateJob);
            group.MapPatch("/{id}", PatchJob);
        }
        static async Task<Ok<List<JobApplicationDto>>> GetAllJobsForUserAsync(IJobService jobService, ClaimsPrincipal claimsPrincipal)
        {
            var userIdClaim = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var userId = int.Parse(userIdClaim!);
            return TypedResults.Ok(await jobService.GetJobApplicationsByUserIdAsync(userId));
        }

        static async Task<Ok<List<JobApplicationDto>>> GetFilteredJobsAsync(ClaimsPrincipal claimsPrincipal, string? name, JobApplication.StatusState? status, IJobService jobService)
        {
            var userIdClaim = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var userId = int.Parse(userIdClaim!);
            return TypedResults.Ok(await jobService.GetFilteredJobsAsync(userId, name, status));
        }
        static async Task<Results<Ok<JobApplicationDto>, NotFound>> GetJobById(ClaimsPrincipal claimsPrincipal,int id, IJobService jobService) 
        {
            var userIdClaim = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var userId = int.Parse(userIdClaim!);
            return await jobService.GetJobApplicationByIdAsync(userId,id)
                is JobApplicationDto jobApplication
                    ? TypedResults.Ok(jobApplication)
                    : TypedResults.NotFound();
        }
        static async Task<Results<Created<JobApplicationDto>, ValidationProblem>> CreateJob(CreateJobDto job,ClaimsPrincipal claimsPrincipal, IJobService service)
        {
            var errors = ValidationHelper.Validate(job);
            if (errors.Count > 0)
            {
                var problemDict = ValidationHelper.ToValidationDictionary(errors);
                return TypedResults.ValidationProblem(problemDict);
            }

            var userIdClaim = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var userId = int.Parse(userIdClaim!);

            var savedJob = await service.CreateJobAsync(job, userId);

            return TypedResults.Created($"/{savedJob.JobId}",savedJob);
        }
        static async Task<Results<Created<JobApplicationDto>,NotFound,ValidationProblem>> PatchJob(ClaimsPrincipal claimsPrincipal,int id, JobPatchDto jobPatch, IJobService service)
        {
            var userIdClaim = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var userId = int.Parse(userIdClaim!);
            var errors = ValidationHelper.Validate(jobPatch);
            if (errors.Count > 0)
            {
                var problemDict = ValidationHelper.ToValidationDictionary(errors);
                return TypedResults.ValidationProblem(problemDict);
            }

            var patchedJob = await service.PatchJobAsync(userId,id, jobPatch);

            if (patchedJob == null) return TypedResults.NotFound();
            return TypedResults.Created($"/{patchedJob.JobId}", patchedJob);
        }
        static async Task<Results<NoContent,NotFound>> DeleteJob(ClaimsPrincipal claimsPrincipal,int id, IJobService service)
        {
            var userIdClaim = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var userId = int.Parse(userIdClaim!);
            bool result = await service.DeleteJobAsync(userId,id);
            if (result) return TypedResults.NoContent();
            return TypedResults.NotFound();
        }
    }   
}
