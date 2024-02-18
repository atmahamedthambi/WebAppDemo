using AuthWebApi.Data;
using AuthWebApi.Models;
using AuthWebApi.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace AuthWebApi.Service
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _appDbContext;

        public UserService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ResponseModel> GetAllUsersAsync()
        {
            var response = new ResponseModel();
            try
            {
                response.Result = await _appDbContext.Users.ToListAsync();
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
