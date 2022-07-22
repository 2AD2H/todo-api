
using System.Threading.Tasks;
using TodoApp_WebAPI.Models;

namespace TodoApp_WebAPI.Repositories
{
    public interface IUserRepository
    {
        public System.Threading.Tasks.Task CreateUserAccount(string auth0Id, string username);
        public Task<User> GetUserByAuth0Id(string auth0Id, string email);
    }
}
