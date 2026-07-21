using JobFollower.Backend.Model.DTO;

namespace JobFollower.Backend.Service
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(RegisterUserDto user);
    }
}
