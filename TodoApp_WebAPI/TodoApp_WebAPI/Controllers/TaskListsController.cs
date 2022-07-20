using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp_WebAPI.JWTUtilities;
using TodoApp_WebAPI.Models;
using TodoApp_WebAPI.Repositories;

namespace TodoApp_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskListsController : ControllerBase
    {
        private readonly ITaskListRepository _taskListRepository;
        private readonly IUserRepository _userRepository;
        private readonly HttpContext _httpContext;

        public TaskListsController(ITaskListRepository taskListRepository, IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _taskListRepository = taskListRepository;
            _httpContext = httpContextAccessor.HttpContext;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskList>>> GetTaskListsNotInsideAnyGroupByUserId()
        {
            User user = HttpContext.Items["User"] as User;
            return await _taskListRepository.GetAllListNotInsideAnyGroup(user.Id);
        }


        [HttpGet("{groupId}")]
        public async Task<ActionResult<IEnumerable<TaskList>>> GetTaskListInsideAGroupByUserId(int groupId)
        {
            User user = HttpContext.Items["User"] as User;
            return await _taskListRepository.GetAllListInsideAGroup(user.Id, groupId);
        }

        [HttpPost]
        public async Task<IActionResult> CreateList(TaskList taskList)
        {
            try
            {
                User user = HttpContext.Items["User"] as User;
                taskList.UserId = user.Id;
                await _taskListRepository.CreateList(taskList);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut]
        public async Task<IActionResult> UpdateTaskList( TaskList taskList)
        {
            try
            {
                User user = HttpContext.Items["User"] as User;
                taskList.UserId = user.Id;
                await _taskListRepository.UpdateList(taskList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskList(int id)
        {
            try
            {
                await _taskListRepository.DeleteList(id);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

    }
}
