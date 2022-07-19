using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp_WebAPI.Models;
using TodoApp_WebAPI.Repositories;
using Task = System.Threading.Tasks.Task;

namespace TodoApp_WebAPI.JWTUtilities
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserRepository userRepository)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                await attachUserToContext(context, userRepository);
            }
            await _next(context);
        }

        private async Task attachUserToContext(HttpContext context, IUserRepository userRepository)
        {
            var id = context.User.Identity.Name;
            context.Items["User"] = await userRepository.GetUserByAuth0Id(id);
        }
    }
}
