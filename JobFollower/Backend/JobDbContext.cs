using JobFollower.Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace JobFollower.Backend
{
    public class JobDbContext : DbContext
    {
        public JobDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> users => Set<User>();
        public DbSet<JobApplication> jobApplications => Set<JobApplication>();
    }
}
