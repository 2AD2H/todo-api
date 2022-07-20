using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApp_WebAPI.Models;
using TodoApp_WebAPI.Repositories;
using TodoApp_WebAPI.JWTUtilities;


namespace TodoApp_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly HttpContext _httpContext;
        public GroupsController(IGroupRepository groupRepository,IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _groupRepository = groupRepository;
            _httpContext = httpContextAccessor.HttpContext;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroupsByUserId()
        {
            User user = HttpContext.Items["User"] as User;
            return await _groupRepository.GetAllGroupByUserId(user.Id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(Group group)
        {
            try
            {
                User user = HttpContext.Items["User"] as User;
                group.Id = 0;
                group.UserId = user.Id;
                await _groupRepository.CreateGroup(group);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> RenameGroup( Group group)
        {
            try
            {
                User user = HttpContext.Items["User"] as User;
                group.UserId = user?.Id;
                await _groupRepository.RenameGroup(group);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            try
            {
                await _groupRepository.DeleteGroup(id);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

    }
}
