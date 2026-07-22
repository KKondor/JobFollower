using JobFollower.Backend.Model.DTO;

namespace JobFollower.Backend.Service
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(RegisterUserDto user);
        Task<UserDto?> ValidateUserAsync(string email, string password);
        Task<string> CreateRefreshTokenAsync(int userId);
        Task<LoginResponseDto?> RefreshAccessTokenAsync(string rawRefreshToken, Func<string, Task> setNewCookie);
        Task RevokeRefreshTokenAsync(string rawRefreshToken);
    }
}
