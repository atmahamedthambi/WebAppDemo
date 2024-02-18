using AuthWebApi.Models;

namespace AuthWebApi.Service.IService
{
    public interface IUserService
    {
        Task<ResponseModel> GetAllUsersAsync();
    }
}
