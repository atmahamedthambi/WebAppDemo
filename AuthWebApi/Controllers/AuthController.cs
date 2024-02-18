using AuthWebApi.Models;
using AuthWebApi.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace AuthWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<ResponseModel> RegisterAsync(RegistrationModel registrationModel)
        {
            var response = new ResponseModel();
            var result = await _authenticationService.RegisterAsync(registrationModel);
            if (result != null && !result.IsSuccess)
            {
                response.IsSuccess = false;
                response.Result = result;
            }
            response.Result = result.Result;
            return response;
        }

        [HttpPost]
        [Route("add/role")]
        public async Task<ResponseModel> CreateRoleAsync([FromBody] string role)
        {
            var response = await _authenticationService.CreateRoleAsync(role);
            return response;
        }

        [HttpPost]
        [Route("assignRoleToUser")]
        public async Task<ResponseModel> AssignRoleToUserAsync([FromBody] RegistrationModel registrationModel)
        {
            var response = await _authenticationService.AssignRoleToUserAsync(registrationModel);
            return response;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ResponseModel> LoginAsync([FromBody] LoginModel loginModel)
        {
            var response = new ResponseModel();
            var result = await _authenticationService.LoginAsync(loginModel);
            if (result == null)
            {
                response.Message = "Invalid email or password";
                response.IsSuccess = false;
            }
            response.Result = result;
            return response;
        }

       
        [HttpGet]
        [Route("roles")]
        
        public async Task<ResponseModel> GetAllRolesAsync()
        {
            var response = await _authenticationService.GetAllRolesAsync();
            return response;
        }

        [HttpPost]
        [Route("add/claim")]
        public async Task<ResponseModel> AddUserClaimAsync([FromBody]UserClaims claim)
        {
            var response = await _authenticationService.AddUserClaimAsync(claim);
            return response;
            // this endpoint will add a new claim for a user. let's add the claim from the application
        }
    }
}
