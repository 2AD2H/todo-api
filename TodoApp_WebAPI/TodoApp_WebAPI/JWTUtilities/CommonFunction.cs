using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace TodoApp_WebAPI.JWTUtilities
{
    public class CommonFunction
    {
        public string GetAuth0UserIdFromPayload(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var stream = authHeader.ToString().Split(' ')[1];
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);
                var tokenS = jsonToken as JwtSecurityToken;
                var sub = tokenS.Claims.First(claim => claim.Type == "sub").Value;
                return sub;
            }
            return null;
        }
    }
}
