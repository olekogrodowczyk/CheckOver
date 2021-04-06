using CheckOver.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckOver.Repository
{
    public interface IGroupRepository
    {
        Task<int> AddNewGroup(MakeGroupVM makeGroupModel);

        Task<int> ApplyGroupSettings(GroupSettingsVM groupSettings, int id);

        Task<List<Group>> GetAllGroups();

        Task<Group> GetGroupById(int id);

        Task<string> getGroupPhoto(int id);

        Task<List<Assignment>> getMembers(int groupId);

        Task<List<Group>> GetUsersGroups();

        string id();
    }
}