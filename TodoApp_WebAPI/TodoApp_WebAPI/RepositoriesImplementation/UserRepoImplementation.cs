using System.Threading.Tasks;
using TodoApp_WebAPI.DataAcess;
using TodoApp_WebAPI.Repositories;

namespace TodoApp_WebAPI.RepositoriesImplementation
{
    public class UserRepoImplementation : IUserRepository
    {
        public async Task CreateUserAccount(string auth0Id, string username)
        {
            await UserDAO.Instance.CreateUserAccount(auth0Id, username);
        }

        public async Task<int> GetUserIdByAuth0Id(string auth0Id)
        {
            return await UserDAO.Instance.GetUserIdByAuth0Id(auth0Id);
        }
    }
}
