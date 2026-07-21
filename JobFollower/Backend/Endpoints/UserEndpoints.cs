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
        static async Task<Results<Ok<string>, UnauthorizedHttpResult>> Login(LoginDto login, IUserService userService, TokenService tokenService)
        {
            var user = await userService.ValidateUserAsync(login.Email, login.Password);
            if (user is null) return TypedResults.Unauthorized();

            var token = tokenService.GenerateToken(user);
            return TypedResults.Ok(token);
        }
    }
}
