﻿using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp_WebAPI.DataAcess;
using TodoApp_WebAPI.Repositories;

namespace TodoApp_WebAPI.RepositoriesImplementation
{
    public class TaskRepoImplementation : ITaskRepository
    {
        public async System.Threading.Tasks.Task<Models.Task> CreateTask(Models.Task task)
        {
            return await TaskDAO.Instance.CreateTask(task);
        }

        public async System.Threading.Tasks.Task DeleteTask(int taskId)
        {
            await TaskDAO.Instance.DeleteTask(taskId);
        }

        public async System.Threading.Tasks.Task DeleteTaskInsideAList(int listId)
        {
            await TaskDAO.Instance.DeleteTaskInsideAList(listId);
        }

        public Task<List<Models.Task>> GetAllTaskDue()
        {
            return TaskDAO.Instance.GetAllTaskDue();
        }

        public async Task<List<Models.Task>> GetAllTaskInMyDay(int userId)
        {
            return await TaskDAO.Instance.GetAllTaskInMyDay(userId);
        }

        public async Task<List<Models.Task>> GetAllTaskInsideAList(int userId, int listId)
        {
            return await TaskDAO.Instance.GetAllTaskInsideAList(userId, listId);
        }

        public async Task<List<Models.Task>> GetAllTaskIsImportant(int userId)
        {
            return await TaskDAO.Instance.GetAllTaskImportant(userId);
        }

        public async Task<List<Models.Task>> GetAllTaskNotInsideAList(int userId)
        {
           return await TaskDAO.Instance.GetAllTaskNotInsideAList(userId);
        }

        public async System.Threading.Tasks.Task UpdateTask(Models.Task task)
        {
            await TaskDAO.Instance.UpdateTask(task);
        }
    }
}
