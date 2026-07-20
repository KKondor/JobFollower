using JobFollower.Backend;
using JobFollower.Backend.Endpoints;
using JobFollower.Backend.Repository;
using JobFollower.Backend.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IJobService, JobService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<JobDbContext>(opt => opt.UseNpgsql(connectionString));

var app = builder.Build();
app.MapGroup("/jobs").WithTags("JobModel").MapJobEndpoints();
app.Run();
