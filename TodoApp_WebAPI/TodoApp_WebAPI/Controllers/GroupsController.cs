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


namespace TodoApp_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupRepository _groupRepository;
        private readonly HttpContext _httpContext;
        public GroupsController(IGroupRepository groupRepository, IHttpContextAccessor httpContextAccessor)
        {
            _groupRepository = groupRepository;
            _httpContext = httpContextAccessor.HttpContext;
        }

        [HttpGet("userId")]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroupsByUserId(int userId)
        {
            if (_httpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var stream = authHeader.ToString().Split(' ')[1];
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);
                var tokenS = jsonToken as JwtSecurityToken;
                var iss = tokenS.Claims.First(claim => claim.Type == "iss").Value;
            }
            return await _groupRepository.GetAllGroupByUserId(userId);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(Group group)
        {
            try
            {
                group.Id = 0;
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
