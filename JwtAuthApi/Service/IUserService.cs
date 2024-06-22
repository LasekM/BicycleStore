/*using JwtAuthApi.Models;

namespace JwtAuthApi.Services
{
    public interface IUserService
    {
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserById(int id);
        Task Register(User user);
        User Authenticate(string username, string password);

    }
}
*/


using System.Threading.Tasks;
using JwtAuthApi.Models;

namespace JwtAuthApi.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<User> GetUserById(int id);
        Task<User> GetUserByUsername(string username);
        Task Register(User user);
    }
}
