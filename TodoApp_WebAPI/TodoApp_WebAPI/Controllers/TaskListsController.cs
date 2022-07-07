using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public TaskListsController(ITaskListRepository taskListRepository)
        {
            _taskListRepository = taskListRepository;
        }

        [HttpGet("userId")]
        public async Task<ActionResult<IEnumerable<TaskList>>> GetTaskListsNotInsideAnyGroupByUserId(int userId)
        {
            return await _taskListRepository.GetAllListNotInsideAnyGroup(userId);
        }


        [HttpGet("{userId}/{groupId}")]
        public async Task<ActionResult<IEnumerable<TaskList>>> GetTaskListInsideAGroupByUserId(int userId, int groupId)
        {
            return await _taskListRepository.GetAllListInsideAGroup(userId, groupId);
        }

        [HttpPost]
        public async Task<IActionResult> CreateList(TaskList taskList)
        {
            try
            {
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
