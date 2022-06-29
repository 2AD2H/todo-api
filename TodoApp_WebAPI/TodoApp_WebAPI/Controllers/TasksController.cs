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
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TasksController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet("userId")]
        public async Task<ActionResult<List<Models.Task>>> GetAllTaskNotInsideAList(int userId)
        {
            return await _taskRepository.GetAllTaskNotInsideAList(userId);
        }

        [HttpGet("{userId}/{listId}")]
        public async Task<ActionResult<List<Models.Task>>> GetAllTaskInsideAList(int userId, int listId)
        {
            return await _taskRepository.GetAllTaskInsideAList(userId, listId);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(Models.Task task)
        {
            try
            {
                task.Id = 0;
                await _taskRepository.CreateTask(task);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask(Models.Task task)
        {
            try
            {
                await _taskRepository.UpdateTask(task);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                await _taskRepository.DeleteTask(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return NoContent();
        }
    }
}
