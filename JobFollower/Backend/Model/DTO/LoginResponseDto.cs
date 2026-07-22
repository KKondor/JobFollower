namespace JobFollower.Backend.Model.DTO
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }
        public UserDto User { get; set; }
        public LoginResponseDto(string accessToken, UserDto user)
        {
            AccessToken = accessToken;
            User = user;
        }
    }
}
