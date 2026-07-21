using JobFollower.Backend.Model;

namespace JobFollower.Backend.Repository
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);
    }
}
