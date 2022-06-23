using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp_WebAPI.DataAcess;
using TodoApp_WebAPI.Models;
using TodoApp_WebAPI.Repositories;

namespace TodoApp_WebAPI.RepositoriesImplementation
{
    public class GroupRepoImplementation : IGroupRepository
    {

        public async System.Threading.Tasks.Task DeleteGroup(int groupId)
        {
            await GroupDAO.Instance.DeleteGroup(groupId);
        }

        async System.Threading.Tasks.Task IGroupRepository.RenameGroup(Group group)
        {
            await GroupDAO.Instance.RenameGroup(group);
        }


        async Task<List<Group>> IGroupRepository.GetAllGroupByUserId(int userId)
        {
            return await GroupDAO.Instance.GetAllGroupByUserId(userId);
        }

        async System.Threading.Tasks.Task IGroupRepository.CreateGroup(Group group)
        {
            await GroupDAO.Instance.CreateGroup(group);
        }
    }
}
