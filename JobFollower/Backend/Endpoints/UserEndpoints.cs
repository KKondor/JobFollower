using JobFollower.Backend.Helpers;
using JobFollower.Backend.Model.DTO;
using JobFollower.Backend.Service;
using Microsoft.AspNetCore.Http.HttpResults;

namespace JobFollower.Backend.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapToUserEndpoints(this IEndpointRouteBuilder group)
        {
            group.MapPost("/register",RegisterUser);
            group.MapPost("/login",Login);
            group.MapPost("/refresh", Refresh);
            group.MapPost("/logout", Logout);
        }

        static async Task<Results<Created<UserDto>,ValidationProblem>> RegisterUser(RegisterUserDto user,IUserService userService)
        {
            var errors = ValidationHelper.Validate(user);
            if (errors.Count > 0)
            {
                var problemDict = ValidationHelper.ToValidationDictionary(errors);
                return TypedResults.ValidationProblem(problemDict);
            }

            var savedUser = await userService.CreateUserAsync(user);
            return TypedResults.Created($"/{savedUser.UserId}", savedUser);
        }
        static async Task<Results<Ok<LoginResponseDto>, UnauthorizedHttpResult>> Login(LoginDto login, IUserService userService, TokenService tokenService, HttpContext httpContext)
        {
            var user = await userService.ValidateUserAsync(login.Email, login.Password);
            if (user is null) return TypedResults.Unauthorized();

            var accessToken = tokenService.GenerateToken(user);
            var refreshToken = await userService.CreateRefreshTokenAsync(user.UserId);

            httpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return TypedResults.Ok(new LoginResponseDto(accessToken,user));
        }
        static async Task<Results<Ok<string>, UnauthorizedHttpResult>> Refresh(IUserService userService, HttpContext httpContext)
        {
            var rawRefreshToken = httpContext.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(rawRefreshToken))
                return TypedResults.Unauthorized();

            var newAccessToken = await userService.RefreshAccessTokenAsync(
                rawRefreshToken,
                newToken =>
                {
                    httpContext.Response.Cookies.Append("refreshToken", newToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddDays(7)
                    });
                    return Task.CompletedTask;
                });

            if (newAccessToken is null)
            {
                httpContext.Response.Cookies.Delete("refreshToken");
                return TypedResults.Unauthorized();
            }

            return TypedResults.Ok(newAccessToken);
        }

        static async Task<Results<Ok<string>, UnauthorizedHttpResult>> Logout(IUserService userService, HttpContext httpContext)
        {
            var rawRefreshToken = httpContext.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(rawRefreshToken))
                return TypedResults.Unauthorized();

            await userService.RevokeRefreshTokenAsync(rawRefreshToken);
            httpContext.Response.Cookies.Delete("refreshToken");
            return TypedResults.Ok("Logged out successfully.");
        }
    }
}
