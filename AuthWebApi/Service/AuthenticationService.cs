using AuthWebApi.Data;
using AuthWebApi.Models;
using AuthWebApi.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthWebApi.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppDbContext _appDbContext;
        private readonly JwtFields options;

        public AuthenticationService(UserManager<IdentityUser> userManager, AppDbContext appDbContext,
            IOptions<JwtFields> options)
        {
            this._userManager = userManager;
            this._appDbContext = appDbContext;
            this.options = options.Value;
        }


        public async Task<ResponseModel> CreateRoleAsync(string role)
        {
            var response = new ResponseModel();
            try
            {
                await _appDbContext.Roles.AddAsync(new IdentityRole() { Name = role, NormalizedName = role.ToUpper() });
                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<ResponseModel> RegisterAsync(RegistrationModel registration)
        {
            var response = new ResponseModel();
            var userModel = new UserModel();
            var user = new IdentityUser()
            {
                UserName = registration.Username,
                Email = registration.Email,
                PhoneNumber = registration.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, registration.Password);
            if (result != null && result.Succeeded)
            {
                await AssignRoleToUserAsync(registration);
                userModel.Token = await GetTokenAsync(user);
                var res = _appDbContext.Users.First(x => x.Email == user.Email);
                userModel.Email = user.Email;
                userModel.PhoneNumber = user.PhoneNumber;
                userModel.Id = user.Id;
                userModel.Username = user.UserName;
                userModel.Token = await GetTokenAsync(user);
                response.Result = userModel;
                response.IsSuccess = true;
            }
            else
            {
                response.Message = result.Errors.First().Description;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<ResponseModel> AssignRoleToUserAsync(RegistrationModel registrationModel)
        {
            var response = new ResponseModel();
            var user = _appDbContext.Users.First(x => x.Email == registrationModel.Email);
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, registrationModel.Role);
                if (result != null && result.Succeeded)
                {
                    response.Message = "Role assigned to user successfully";
                }
                else
                {
                    response.Message = result.Errors.First().Description;
                }
            }
            return response;
        }

        public async Task<UserModel> LoginAsync(LoginModel login)
        {
            var userModel = new UserModel();
            var user = _appDbContext.Users.First(x => x.Email == login.Email);
            var isUserExists = await _userManager.CheckPasswordAsync(user, login.Password);
            if (user == null || !isUserExists)
            {
                return null;
            }
            userModel.Email = user.Email;
            userModel.PhoneNumber = user.PhoneNumber;
            userModel.Id = user.Id;
            userModel.Username = user.UserName;
            userModel.Token = await GetTokenAsync(user);
            return userModel;
        }

        private async Task<string> GetTokenAsync(IdentityUser identityUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(options.Secret);
            var roles = await _userManager.GetRolesAsync(identityUser);
            var assignedClaims = await _userManager.GetClaimsAsync(identityUser);

            string role = string.Empty;
            if (roles != null && roles.Any())
            {
                role = roles.First();
            }
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, identityUser.Email),
                new Claim(JwtRegisteredClaimNames.Sub, identityUser.Id),
                new Claim(JwtRegisteredClaimNames.Name, identityUser.UserName),
                new Claim(ClaimTypes.Role, role)
            };
            if(assignedClaims != null && assignedClaims.Any()) 
            {
                foreach (var item in assignedClaims)
                {
                    claims.Add(new Claim(item.Type, item.Value));
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = options.Audience,
                Issuer = options.Issuer,
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.Now.AddMinutes(60) // token will be valid for 60 minutes
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            await Task.CompletedTask;
            return tokenString;
        }

        public async Task<ResponseModel> GetAllRolesAsync()
        {
            var response = new ResponseModel();
            try
            {
                var roles = await _appDbContext.Roles.ToListAsync();
                response.Result = roles;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<ResponseModel> AddUserClaimAsync(UserClaims claim)
        {
            // User claims can be added with UserManager object
            var response = new ResponseModel();
            try
            {
                var user = await _appDbContext.Users.FirstAsync(x => x.Email == claim.Email);
                var claimResult = await _userManager.AddClaimAsync(user, new Claim(claim.ClaimType, claim.ClaimValue));
                response.Result = claimResult;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }
    }
}
