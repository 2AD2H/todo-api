
using System.Threading.Tasks;

namespace TodoApp_WebAPI.Repositories
{
    public interface IUserRepository
    {
        public Task CreateUserAccount(string auth0Id, string username);
        public Task<int> GetUserIdByAuth0Id(string auth0Id);
    }
}
