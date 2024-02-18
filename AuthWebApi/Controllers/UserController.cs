using AuthWebApi.Models;
using AuthWebApi.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        [Route("allUsers")]
        public async Task<ResponseModel> GetAllUsersAsync()
        {
            var response = await _userService.GetAllUsersAsync();
            return response;
        }
    }
}
