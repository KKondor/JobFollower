using JobFollower.Backend.Model;
using JobFollower.Backend.Model.DTO;
using JobFollower.Backend.Repository;
using Microsoft.AspNetCore.Identity;

namespace JobFollower.Backend.Service
{
    public class UserService : IUserService
    {
        private PasswordHasher<User> _passwordHasher = new();
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) => _userRepository = userRepository;

        public async Task<UserDto> CreateUserAsync(RegisterUserDto user)
        {
            User convertedUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                HashedPassword = ""
            };
            convertedUser.HashedPassword = _passwordHasher.HashPassword(convertedUser, user.Password);
            var created = await _userRepository.CreateUser(convertedUser);
            return new UserDto(created);
        }

        public async Task<UserDto?> ValidateUserAsync(string email, string password)
        {
            var foundUser = await _userRepository.FindByEmail(email);
            if (foundUser == null) return null;
            PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(foundUser, foundUser.HashedPassword, password);
            if (result == PasswordVerificationResult.Success) return new UserDto(foundUser);
            return null;
        }
    }
}
