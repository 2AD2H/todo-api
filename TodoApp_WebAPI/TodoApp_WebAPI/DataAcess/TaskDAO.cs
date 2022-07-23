using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TodoApp_WebAPI.Models;
using TodoApp_WebAPI.Others;

namespace TodoApp_WebAPI.DataAcess
{
    public class TaskDAO
    {
        private static TaskDAO instance = null;
        private static readonly object instanceLock = new object();
        private TaskDAO() { }
        public static TaskDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TaskDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<Models.Task>> GetAllTaskNotInsideAList(int userId)
        {
            List<Models.Task> tasks = new List<Models.Task>();
            using (TodoAppContext context = new TodoAppContext())
            {
               return await context.Tasks.Where(t => t.UserId == userId && t.ListId == null).ToListAsync();
            }
        }

        public async Task<List<Models.Task>> GetAllTaskInsideAList(int userId, int listId)
        {
            List<Models.Task> tasks = new List<Models.Task>();
            using (TodoAppContext context = new TodoAppContext())
            {
                return await context.Tasks.Where(t => t.UserId == userId && t.ListId == listId).ToListAsync();
            }        
        }

        public async Task<List<Models.Task>> GetAllTaskInMyDay(int userId)
        {
            List<Models.Task> tasks = new List<Models.Task>();
            using (TodoAppContext context = new TodoAppContext())
            {
                return await context.Tasks.Where(t => t.UserId == userId && t.IsInMyDay == true).ToListAsync();
            }
        }

        public async Task<List<Models.Task>> GetAllTaskImportant(int userId)
        {
            List<Models.Task> tasks = new List<Models.Task>();
            using (TodoAppContext context = new TodoAppContext())
            {
                return await context.Tasks.Where(t => t.UserId == userId && t.IsImportant == true).ToListAsync();
            }
        }

        public async System.Threading.Tasks.Task<Models.Task> CreateTask(Models.Task task)
        {
            using (TodoAppContext context = new TodoAppContext())
            {
                if(task.ListId != null)
                {
                    await TaskListDAO.Instance.IncreaseCountById((int)task.ListId);
                }
                if(task.Note == null)
                {
                    task.Note = "none";
                }
                task.CreatedDate = System.DateTime.Now;
                context.Tasks.Add(task);
                await context.SaveChangesAsync();
                return task;
            }
        }

        public async System.Threading.Tasks.Task UpdateTask(Models.Task task)
        {
            using (TodoAppContext context = new TodoAppContext())
            {
                Models.Task originalTask = context.Tasks.Find(task.Id);
                // case 1: thêm task ở ngoài list vào 1 list nào đó 
                if (originalTask.ListId == null && task.ListId != null && task.ListId != -1)
                {
                    await TaskListDAO.Instance.IncreaseCountById((int)task.ListId);
                }
                // case 2: Chuyển task từ list này vào list khác
                else if(originalTask.ListId != null && task.ListId != null 
                    && originalTask.ListId != task.ListId && task.ListId != -1)
                {
                    await TaskListDAO.Instance.IncreaseCountById((int)task.ListId);
                    await TaskListDAO.Instance.DecreseCountById((int)originalTask.ListId);
                }
                // case 3: Chuyển task từ list ra ngoài
                else if(originalTask.ListId != null && task.ListId == -1)
                {
                    await TaskListDAO.Instance.DecreseCountById((int)originalTask.ListId);
                }
                foreach(PropertyInfo pi in task.GetType().GetProperties())
                {
                    if(pi.PropertyType == typeof(int))
                    {
                        if ((int)pi.GetValue(task) == 0)
                        {
                            pi.SetValue(task, pi.GetValue(originalTask));
                        }
                    }
                    if(pi.GetValue(task) == null)
                    {
                        pi.SetValue(task, pi.GetValue(originalTask));
                    }
                }
                if(task.ListId == -1)
                {
                    task.ListId = null;
                }
                context.Entry<Models.Task>(originalTask).State = EntityState.Detached;
                context.Tasks.Update(task);
                await context.SaveChangesAsync();
            }
        }


        public async System.Threading.Tasks.Task DeleteTask(int taskId)
        {
            using (TodoAppContext context = new TodoAppContext())
            {
                Models.Task task = context.Tasks.Where(t => t.Id == taskId).FirstOrDefault();
                if (task != null)
                {
                    if(task.ListId != null)
                    {
                        await TaskListDAO.Instance.DecreseCountById((int)task.ListId);
                    }
                    context.Tasks.Remove(task);
                    await context.SaveChangesAsync();
                }
                else
                {
                    return;
                }
            }
        }

        public async System.Threading.Tasks.Task DeleteTaskInsideAList(int listId)
        {
            using (TodoAppContext context = new TodoAppContext())
            {
                List<Models.Task> task = context.Tasks.Where(t => t.ListId == listId).ToList();
                if (task != null)
                {
                    context.Tasks.RemoveRange(task);
                    await context.SaveChangesAsync();
                }
                else
                {
                    return;
                }
            }
        }
        // return all task having its due date just coming 
        public async Task<List<Models.Task>> GetAllTaskDue()
        {
            List<Models.Task> tasks = new List<Models.Task>();
            using (TodoAppContext context = new TodoAppContext())
            {
                return await context.Tasks
                    .Where(t => DateTime.Now >= t.DueDate 
                    && (bool)t.IsCompleted == false && (bool)t.IsMailed == false)
                    .ToListAsync();
            }
        }
    }
}
