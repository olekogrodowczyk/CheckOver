using CheckOver.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckOver.Repository
{
    public interface IGroupRepository
    {
        Task<int> AddNewGroup(MakeGroupModel makeGroupModel);
        Task<int> ApplyGroupSettings(GroupSettings groupSettings, int id);
        int function();
        Task<List<Group>> GetAllGroups();
        Task<Group> GetGroupById(int id);
        Task<List<Group>> GetUsersGroups();
        string id();
    }
}