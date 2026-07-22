using JobFollower.Backend.Model;
using JobFollower.Backend.Model.DTO;
using JobFollower.Backend.Model.Token;
using JobFollower.Backend.Repository;
using Microsoft.AspNetCore.Identity;

namespace JobFollower.Backend.Service
{
    public class UserService : IUserService
    {
        private PasswordHasher<User> _passwordHasher = new();
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;
        public UserService(IUserRepository userRepository, TokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

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

        public async Task<string> CreateRefreshTokenAsync(int userId)
        {
            var rawToken = _tokenService.GenerateRefreshToken();
            var hashedToken = _tokenService.HashToken(rawToken);

            var refreshToken = new RefreshToken
            {
                TokenHash = hashedToken,
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _userRepository.SaveRefreshToken(refreshToken);

            return rawToken;
        }

        public async Task<LoginResponseDto?> RefreshAccessTokenAsync(string rawRefreshToken, Func<string, Task> setNewCookie)
        {
            var hashedToken = _tokenService.HashToken(rawRefreshToken);
            var storedToken = await _userRepository.GetByHashAsync(hashedToken);

            if (storedToken is null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.UtcNow)
                return null;

            storedToken.IsRevoked = true;
            await _userRepository.RevokeTokenAsync(storedToken);

            var newRawToken = await CreateRefreshTokenAsync(storedToken.UserId);
            await setNewCookie(newRawToken);

            var user = await _userRepository.FindByIdAsync(storedToken.UserId);
            if (user is null) return null;

            return new LoginResponseDto(_tokenService.GenerateToken(new UserDto(user)), new UserDto(user));
        }
        public async Task RevokeRefreshTokenAsync(string rawRefreshToken)
        {
            var hashedToken = _tokenService.HashToken(rawRefreshToken);
            var storedToken = await _userRepository.GetByHashAsync(hashedToken);
            if (storedToken is null) return;
            await _userRepository.RevokeTokenAsync(storedToken);
        }
    }
}
