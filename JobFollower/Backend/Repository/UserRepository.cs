using JobFollower.Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace JobFollower.Backend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly JobDbContext _dbContext;
        public UserRepository(JobDbContext dbContext) => _dbContext = dbContext;

        public async Task<User> CreateUser(User user)
        {
            var result = await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> FindByEmail(string email)
        {
            var querry = _dbContext.Users.AsQueryable();
            querry = querry.Where(x => x.Email == email);
            return await querry.FirstOrDefaultAsync();
        }
    }
}
