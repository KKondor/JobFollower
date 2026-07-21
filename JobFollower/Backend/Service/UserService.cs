using JobFollower.Backend.Model;
using JobFollower.Backend.Model.DTO;
using JobFollower.Backend.Repository;
using Microsoft.AspNetCore.Identity;

namespace JobFollower.Backend.Service
{
    public class UserService : IUserService
    {
        private PasswordHasher<RegisterUserDto> _passwordHasher = new();
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) => _userRepository = userRepository;

        public async Task<UserDto> CreateUserAsync(RegisterUserDto user)
        {
            User convertedUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                HashedPassword = _passwordHasher.HashPassword(user, user.Password)
            };
            var created = await _userRepository.CreateUser(convertedUser);
            return new UserDto(convertedUser);
        }
    }
}
