﻿using System;
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
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly HttpContext _httpContext;

        public TasksController(ITaskRepository taskRepository,IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _taskRepository = taskRepository;
            _httpContext = httpContextAccessor.HttpContext;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Models.Task>>> GetAllTaskNotInsideAList()
        {
            User user = HttpContext.Items["User"] as User;
            return await _taskRepository.GetAllTaskNotInsideAList(user.Id);
        }

        [HttpGet("{listId}")]
        public async Task<ActionResult<List<Models.Task>>> GetAllTaskInsideAList(int listId)
        {
            User user = HttpContext.Items["User"] as User;
            return await _taskRepository.GetAllTaskInsideAList(user.Id, listId);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(Models.Task task)
        {
            try
            {
                User user = HttpContext.Items["User"] as User;
                task.UserId = user.Id;
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
