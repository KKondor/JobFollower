using JobFollower.Backend.Model;
using JobFollower.Backend.Model.Token;
using Microsoft.EntityFrameworkCore;

namespace JobFollower.Backend
{
    public class JobDbContext : DbContext
    {
        public JobDbContext(DbContextOptions<JobDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<JobApplication> JobApplications => Set<JobApplication>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    }
}
