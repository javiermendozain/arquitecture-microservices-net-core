using System.Threading.Tasks;
using Api.Users.Models;
using Api.Users.Helpers;

namespace Api.Users.Interfaces
{
    public interface IUsersService
    {
        Task<Response> AddAsync(UserModel user);
        
        Task<Response> GetAllAsync(int page, int pageSize);

        Task<Response> GetByIdAsync(int id);

        Task<Response> UpdateAsync(int id, UserModel user);

        Task<Response> DeleteAsync(int userId);

    }
}
