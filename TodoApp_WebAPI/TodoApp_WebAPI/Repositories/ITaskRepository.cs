using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp_WebAPI.Models;

namespace TodoApp_WebAPI.Repositories
{
    public interface ITaskRepository
    {
        public Task<List<Models.Task>> GetAllTaskNotInsideAList(int userId);
        public Task<List<Models.Task>> GetAllTaskInsideAList(int userId, int listId);
        public System.Threading.Tasks.Task<Models.Task> CreateTask(Models.Task task);
        public System.Threading.Tasks.Task UpdateTask(Models.Task task);
        public System.Threading.Tasks.Task DeleteTask(int taskId);
        public System.Threading.Tasks.Task DeleteTaskInsideAList(int listId);
        public Task<List<Models.Task>> GetAllTaskDue();

    }
}
