using JobFollower.Backend.Model;
using JobFollower.Backend.Model.Token;
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

        public async Task<User?> FindByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<RefreshToken?> GetByHashAsync(string tokenHash)
        {
            return await _dbContext.RefreshTokens.Where(x => x.TokenHash == tokenHash).FirstOrDefaultAsync();
        }

        public async Task RevokeTokenAsync(RefreshToken token)
        {
            token.IsRevoked = true;
            _dbContext.RefreshTokens.Update(token);
            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveRefreshToken(RefreshToken token)
        {
            await _dbContext.RefreshTokens.AddAsync(token);
            await _dbContext.SaveChangesAsync();
        }
    }
}
