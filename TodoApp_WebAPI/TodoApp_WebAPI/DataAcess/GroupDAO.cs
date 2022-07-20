using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TodoApp_WebAPI.Models;

namespace TodoApp_WebAPI.DataAcess
{
    public class GroupDAO
    {
        private static GroupDAO instance = null;
        private static readonly object instanceLock = new object();
        private GroupDAO() { }
        public static GroupDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new GroupDAO();
                    }
                    return instance;
                }
            }
        }

        public async Task<List<Group>> GetAllGroupByUserId(int userId)
        {
            
            using (TodoAppContext context = new TodoAppContext())
            {
                return await context.Groups.Where(g => g.UserId == userId).ToListAsync();
            }
        }

        public async System.Threading.Tasks.Task<Group> CreateGroup(Group group)
        {
            using (TodoAppContext context = new TodoAppContext())
            {
                context.Groups.Add(group);
                await context.SaveChangesAsync();
                return group;
            }
        }

        public async System.Threading.Tasks.Task DeleteGroup(int groupId)
        {
            using (TodoAppContext context = new TodoAppContext())
            {
                Group group = context.Groups.Where(g => g.Id == groupId).FirstOrDefault();
                if (group != null)
                {
                    context.Groups.Remove(group);
                    await context.SaveChangesAsync();
                }
                else
                {
                    return;
                }
            }
        }

        public async System.Threading.Tasks.Task RenameGroup(Group group)
        {
            using (TodoAppContext context = new TodoAppContext())
            {
                var originalGroup = context.Groups.Find(group.Id);
                foreach (PropertyInfo pi in group.GetType().GetProperties())
                {
                    if (pi.PropertyType == typeof(int))
                    {
                        if ((int)pi.GetValue(group) == 0)
                        {
                            pi.SetValue(group, pi.GetValue(originalGroup));
                        }
                    }
                    if (pi.GetValue(group) == null)
                    {
                        pi.SetValue(group, pi.GetValue(originalGroup));
                    }
                }
                context.Entry<Models.Group>(originalGroup).State = EntityState.Detached;
                context.Groups.Update(group);
                await context.SaveChangesAsync();
            }
        }
    }
}
