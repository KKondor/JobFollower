using JobFollower.Backend;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<JobDbContext>(opt => opt.UseNpgsql(connectionString));

var app = builder.Build();

app.Run();
