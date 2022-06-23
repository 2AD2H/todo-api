using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp_WebAPI.Models;

namespace TodoApp_WebAPI.Repositories
{
    public interface ITaskListRepository
    {
        public Task<List<TaskList>> GetAllListNotInsideAnyGroup(int userId);
        public Task<List<TaskList>> GetAllListInsideAGroup(int userId, int groupId);
        public System.Threading.Tasks.Task CreateList(TaskList taskList);
        public System.Threading.Tasks.Task DeleteList(int taskListId);
        public System.Threading.Tasks.Task UpdateList(TaskList taskList);

    }
}
