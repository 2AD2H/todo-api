using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TodoApp_WebAPI.JWTUtilities;
using TodoApp_WebAPI.Models;
using TodoApp_WebAPI.Repositories;

namespace TodoApp_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly HttpContext _httpContext;
        public AccountController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> CreateUserAccount()
        {
            try
            {
                string auth0Id = CommonFunction.Instance.GetAuth0UserIdFromPayload(_httpContext);
                string username = CommonFunction.Instance.GetAuth0UserNameFromPayload(_httpContext);
                await _userRepository.CreateUserAccount(auth0Id, username);
                return Ok();
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

/*        [HttpGet("GetCurrentUserId")]
        public async Task<ActionResult<int>> GetUserIdByAuth0Id()
        {
            string auth0Id = CommonFunction.Instance.GetAuth0UserIdFromPayload(_httpContext);
            return await _userRepository.GetUserIdByAuth0Id(auth0Id);
        }*/
    }
}
