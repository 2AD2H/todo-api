using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TodoApp_WebAPI.Requirements
{
    public class JWTRequirement : IAuthorizationRequirement
    {
        
    }

    public class JWTRequirementHandler : AuthorizationHandler<JWTRequirement>
    {
        private readonly HttpClient _client;
        private readonly HttpContext _httpContext;

        public JWTRequirementHandler(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _client = httpClientFactory.CreateClient();
            _httpContext = httpContextAccessor.HttpContext;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, JWTRequirement requirement)
        {
            if (_httpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var stream = authHeader.ToString().Split(' ')[1];
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);
                var tokenS = jsonToken as JwtSecurityToken;
                var sub = tokenS.Claims.First(claim => claim.Type == "sub").Value;
                if (sub != null) 
                {
                    context.Succeed(requirement);
                }
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
