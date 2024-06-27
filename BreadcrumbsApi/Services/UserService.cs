using BreadcrumbsApi.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BreadcrumbsApi.Services
{
    public class UserService : IUserService
    {
        private readonly BreadcrumbsContext _breadcrumbsContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger<UserService> _logger;

        public UserService(BreadcrumbsContext breadcrumbsContext, IPasswordHasher<User> passwordHasher, IOptions<JwtOptions> jwtOptions, ILogger<UserService> logger)
        {
            _breadcrumbsContext = breadcrumbsContext;
            _passwordHasher = passwordHasher;
            _jwtOptions = jwtOptions.Value;
            _logger = logger;
        }

        public async Task<User> RegisterAsync(User user)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, user.Password);
            user.Password = string.Empty;

            try
            {
                using (var transaction = _breadcrumbsContext.Database.BeginTransaction())
                {
                    await _breadcrumbsContext.Users.AddAsync(user);
                    await _breadcrumbsContext.SaveChangesAsync();

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return await Task.FromResult(user);
        }

        public LoginResult Login(LoginRequest loginRequest)
        {
            try
            {
                var user = _breadcrumbsContext.Users.Where<User>(u => u.Email == loginRequest.Email).First();
                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginRequest.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    return new LoginResult
                    {
                        ApiKey = GenerateToken(user),
                        LoginSuccess = true,
                    };
                }
                else
                {
                    return new LoginResult
                    {
                        LoginSuccess = false,
                        Message = "Incorrect Password."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new LoginResult
                {
                    LoginSuccess = false,
                    Message = ex.Message
                };
            }
        }

        private string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("Name", user.Username),
            };

            var token = new JwtSecurityToken(
                _jwtOptions.Issuer,
                null,
                claims,
                expires: DateTime.Now.AddDays(Convert.ToDouble(_jwtOptions.Expiration)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
    