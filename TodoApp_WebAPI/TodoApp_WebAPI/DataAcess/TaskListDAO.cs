﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public async System.Threading.Tasks.Task<TaskList> CreateList(TaskList taskList)
        {
            using (TodoAppContext context = new TodoAppContext())
            {
                taskList.Id = 0;
                taskList.TaskCount = 0;
                context.TaskLists.Add(taskList);
                await context.SaveChangesAsync();
                return taskList;
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
                var originalTaskList = context.TaskLists.Find(taskList.Id);
                foreach (PropertyInfo pi in taskList.GetType().GetProperties())
                {
                    if (pi.PropertyType == typeof(int))
                    {
                        if ((int)pi.GetValue(taskList) == 0)
                        {
                            pi.SetValue(taskList, pi.GetValue(originalTaskList));
                        }
                    }
                    if (pi.GetValue(taskList) == null)
                    {
                        pi.SetValue(taskList, pi.GetValue(originalTaskList));
                    }
                }
                if(taskList.GroupId == -1)
                {
                    taskList.GroupId = null;
                }
                context.Entry<Models.TaskList>(originalTaskList).State = EntityState.Detached;
                context.TaskLists.Update(taskList);
                await context.SaveChangesAsync();
            }
        }

        public async System.Threading.Tasks.Task IncreaseCountById(int taskListId)
        {
            using (TodoAppContext context = new TodoAppContext())
            {
                TaskList taskList = context.TaskLists.Find(taskListId);
                taskList.TaskCount += 1;
                context.Entry<TaskList>(taskList).State = EntityState.Detached;
                context.Update(taskList);
                await context.SaveChangesAsync();
            }
        }

        public async System.Threading.Tasks.Task DecreseCountById(int taskListId)
        {
            using (TodoAppContext context = new TodoAppContext())
            {
                TaskList taskList = context.TaskLists.Find(taskListId);
                taskList.TaskCount -= 1;
                context.Entry<TaskList>(taskList).State = EntityState.Detached;
                context.Update(taskList);
                await context.SaveChangesAsync();
            }
        }
    }
}
