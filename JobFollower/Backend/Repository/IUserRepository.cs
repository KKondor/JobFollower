using JobFollower.Backend.Model;
using JobFollower.Backend.Model.Token;

namespace JobFollower.Backend.Repository
{
    public interface IUserRepository
    {
        Task<User?> FindByEmail(string email);
        Task<User> CreateUser(User user);
        Task<User?> FindByIdAsync(int id);
        Task SaveRefreshToken(RefreshToken token);
        Task<RefreshToken?> GetByHashAsync(string tokenHash);
        Task RevokeTokenAsync(RefreshToken token);
    }
}
