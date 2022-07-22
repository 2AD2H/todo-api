using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TodoApp_WebAPI.Models;
using TodoApp_WebAPI.Others;
using TodoApp_WebAPI.Repositories;
using Task = TodoApp_WebAPI.Models.Task;

namespace TodoApp_WebAPI.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private int executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private Timer? _timer = null;

        public TimedHostedService(ILogger<TimedHostedService> logger, ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _logger = logger;
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public System.Threading.Tasks.Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(Globals.TIME_INTERVAL));

            return System.Threading.Tasks.Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);
            EmailService emailService = new EmailService();
            List<Task> tasks = await _taskRepository.GetAllTaskDue();
            _logger.LogInformation("Task count" + tasks.Count);
            foreach (var task in tasks)
            {
                User user = await _userRepository.GetUserById(task.UserId);
                string receiverEmail = user.Email;
                emailService.SendEmail(receiverEmail, task);
                Task updateTask = new Task { Id = task.Id, IsMailed = true};
                await _taskRepository.UpdateTask(updateTask);
            }
        }

        public System.Threading.Tasks.Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return System.Threading.Tasks.Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}