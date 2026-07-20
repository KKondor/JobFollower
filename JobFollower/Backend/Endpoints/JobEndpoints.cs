using JobFollower.Backend.Model.DTO;
using JobFollower.Backend.Service;
using Microsoft.AspNetCore.Http.HttpResults;

namespace JobFollower.Backend.Endpoints
{
    public static class JobEndpoints
    {
        public static void MapJobEndpoints(this IEndpointRouteBuilder group)
        {
            group.MapGet("/",GetAllJobsAsync);
        }
        static async Task<Ok<List<JobApplicationDto>>> GetAllJobsAsync(IJobService jobService)
        {
            return TypedResults.Ok(await jobService.GetAllJobsAsync());
        }
    }   
}
