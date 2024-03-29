﻿using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TodoApp_WebAPI.Models;

namespace TodoApp_WebAPI.DataAcess
{
    public class UserDAO
    {
        private static UserDAO instance = null;
        private static readonly object instanceLock = new object();
        private UserDAO() { }
        public static UserDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new UserDAO();
                    }
                    return instance;
                }
            }
        }

        public async System.Threading.Tasks.Task CreateUserAccount(string auth0Id, string username)
        {
            using (TodoAppContext context = new TodoAppContext())
            {
                User user = new User { Id = 0, Email = username };
                context.Users.Add(user);
                await context.SaveChangesAsync();
                int insetedUserId = user.Id;
                Account account = new Account { Id = auth0Id, UserId = insetedUserId };
                context.Accounts.Add(account);
                await context.SaveChangesAsync();
            }
        }

        public async Task<User> GetUserByAuth0Id(string auth0Id, string email)
        {
            using (TodoAppContext context = new TodoAppContext())
            {
                var acc = await context.Accounts.Where(acc => acc.Id == auth0Id).FirstOrDefaultAsync();
                if(acc == null)
                {
                    await CreateUserAccount(auth0Id, email);
                    acc = await context.Accounts.Where(acc => acc.Id == auth0Id).FirstOrDefaultAsync();
                }
                return context.Users.Find(acc.UserId);
            }
        }
        public async Task<User> GetUserById(int id)
        {
            using (TodoAppContext context = new TodoAppContext())
            {
                return await context.Users.FindAsync(id);
            }
        }
    }
}
