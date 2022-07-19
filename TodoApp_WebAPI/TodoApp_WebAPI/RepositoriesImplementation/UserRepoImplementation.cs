using System.Threading.Tasks;
using TodoApp_WebAPI.DataAcess;
using TodoApp_WebAPI.Models;
using TodoApp_WebAPI.Repositories;

namespace TodoApp_WebAPI.RepositoriesImplementation
{
    public class UserRepoImplementation : IUserRepository
    {
        public async System.Threading.Tasks.Task CreateUserAccount(string auth0Id, string username)
        {
            await UserDAO.Instance.CreateUserAccount(auth0Id, username);
        }

        public async Task<User> GetUserByAuth0Id(string auth0Id)
        {
            return await UserDAO.Instance.GetUserByAuth0Id(auth0Id);
        }
    }
}
