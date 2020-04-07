using System.Threading.Tasks;
using Api.Enroll.Helpers;

namespace Api.Enroll.Interfaces
{
    public interface IUsersService
    {
        Task<Response> GetUserById(int Id);
    }
}
