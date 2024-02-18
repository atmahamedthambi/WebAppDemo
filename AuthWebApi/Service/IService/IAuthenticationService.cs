using AuthWebApi.Models;

namespace AuthWebApi.Service.IService
{
    public interface IAuthenticationService
    {
        Task<ResponseModel> RegisterAsync(RegistrationModel registration);
        Task<ResponseModel> CreateRoleAsync(string role);
        Task<ResponseModel> AssignRoleToUserAsync(RegistrationModel registrationModel);
        Task<UserModel> LoginAsync(LoginModel login);
        Task<ResponseModel> GetAllRolesAsync();
        Task<ResponseModel> AddUserClaimAsync(UserClaims claim);
    }
}
