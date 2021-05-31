using CheckOver.Models;
using CheckOver.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckOver.Repository
{
    public interface IGroupRepository
    {
        Task<int> AddNewGroup(MakeGroupVM makeGroupModel);
        Task ChangeGroupName(int groupId, ChangeGroupNameVM changeGroupNameVM);
        Task ChangeGroupPhoto(int groupId, ChangeGroupPhotoVM changeGroupPhotoVM);

        Task ChangeRole(int groupId, string userId);

        Task<bool> DeleteUserFromGroup(int groupId, string userId);

        Task<bool> DeteleGroup(int groupId);

        Task<List<Group>> GetAllGroups();

        Task<List<Assignment>> getCheckers(int groupId);

        Task<Group> GetGroupById(int id);

        Task<string> getGroupPhoto(int id);

        Task<List<Assignment>> getMembers(int groupId);

        Task<List<Assignment>> getSolvers(int groupId);

        Task<List<Group>> GetUsersGroups();

        string id();
    }
}