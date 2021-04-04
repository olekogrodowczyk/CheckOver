using CheckOver.Models;
using CheckOver.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckOver.Repository
{
    public interface IInvitationRepository
    {
        Task<int> AcceptInvitation(int id);

        Task<string> AddNewInvitation(InvitationVM invitationVM, int id);

        int function();

        Task<List<Invitation>> GetReceivedInvitations();

        Task<List<Invitation>> GetSentInvitations();
    }
}