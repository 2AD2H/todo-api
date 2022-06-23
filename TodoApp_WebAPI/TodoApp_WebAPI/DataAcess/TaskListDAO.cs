using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp_WebAPI.Models;

namespace TodoApp_WebAPI.DataAcess
{
    public class TaskListDAO
    {
        private static TaskListDAO instance = null;
        private static readonly object instanceLock = new object();
        private TaskListDAO() { }
        public static TaskListDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TaskListDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<TaskList>> GetAllListNotInsideAnyGroup(int userId)
        {
            List<TaskList> taskLists = new List<TaskList>();
            using (TodoAppContext context = new TodoAppContext())
            {
                return await context.TaskLists.Where(t => t.UserId == userId && t.GroupId == null).ToListAsync();
            }
        }

        public async Task<List<TaskList>> GetAllListInsideAGroup(int userId , int groupId)
        {
            List<TaskList> taskLists = new List<TaskList>();
            using (TodoAppContext context = new TodoAppContext())
            {
                return await context.TaskLists.Where(t => t.UserId == userId && t.GroupId == groupId).ToListAsync();
            }
        }

        public async System.Threading.Tasks.Task CreateList(TaskList taskList)
        {
            using (TodoAppContext context = new TodoAppContext())
            {
                context.TaskLists.Add(taskList);
                await context.SaveChangesAsync();
            }
        }

        // Delete tasks inside of it also
        public async System.Threading.Tasks.Task DeleteList(int taskListId)
        {
            using (TodoAppContext context = new TodoAppContext())
            {
                TaskList taskList = context.TaskLists.Where(t => t.Id == taskListId).FirstOrDefault();
                if (taskList != null)
                {
                    await TaskDAO.Instance.DeleteTaskInsideAList(taskListId);
                    context.TaskLists.Remove(taskList);
                    await context.SaveChangesAsync();
                }
                else
                {
                    return;
                }
            }
        }

        public async System.Threading.Tasks.Task UpdateList(TaskList taskList)
        {
            using (TodoAppContext context = new TodoAppContext())
            {
                context.TaskLists.Update(taskList);
                await context.SaveChangesAsync();
            }
        }
    }
}
