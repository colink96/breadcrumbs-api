using BreadcrumbsApi.Dtos;
using Microsoft.AspNetCore.Identity;

namespace BreadcrumbsApi.Services
{
    public interface IUserService
    {
        Task<User> RegisterAsync(User user);
        LoginResult Login(LoginRequest loginRequest);
    }
}
