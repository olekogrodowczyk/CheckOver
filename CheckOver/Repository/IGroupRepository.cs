using CheckOver.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckOver.Repository
{
    public interface IGroupRepository
    {
        Task<int> AddNewGroup(MakeGroupModel makeGroupModel);
        int function();
        Task<List<Group>> GetAllGroups();
        string id();
    }
}