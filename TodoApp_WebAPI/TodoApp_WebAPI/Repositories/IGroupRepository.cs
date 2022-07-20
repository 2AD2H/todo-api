using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp_WebAPI.Models;

namespace TodoApp_WebAPI.Repositories
{
    public interface IGroupRepository
    {
        public Task<List<Group>> GetAllGroupByUserId(int userId);
        public System.Threading.Tasks.Task<Group> CreateGroup(Group group);
        public System.Threading.Tasks.Task DeleteGroup(int groupId);
        public System.Threading.Tasks.Task RenameGroup(Group group);
    }
}
