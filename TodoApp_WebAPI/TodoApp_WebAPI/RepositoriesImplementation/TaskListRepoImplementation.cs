using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp_WebAPI.DataAcess;
using TodoApp_WebAPI.Models;
using TodoApp_WebAPI.Repositories;

namespace TodoApp_WebAPI.RepositoriesImplementation
{
    public class TaskListRepoImplementation : ITaskListRepository
    {
        public async System.Threading.Tasks.Task<TaskList> CreateList(TaskList taskList)
        {
            return await TaskListDAO.Instance.CreateList(taskList);
        }

        public async System.Threading.Tasks.Task DeleteList(int taskListId)
        {
            await TaskListDAO.Instance.DeleteList(taskListId);
        }

        public async Task<List<TaskList>> GetAllListInsideAGroup(int userId, int groupId)
        {
           return await TaskListDAO.Instance.GetAllListInsideAGroup(userId, groupId);
        }

        public async Task<List<TaskList>> GetAllListNotInsideAnyGroup(int userId)
        {
            return await TaskListDAO.Instance.GetAllListNotInsideAnyGroup(userId);
        }

        public async System.Threading.Tasks.Task UpdateList(TaskList taskList)
        {
            await TaskListDAO.Instance.UpdateList(taskList);
        }
    }
}
