using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Models.ViewModels
{
    public class InvitationsVM
    {
        public List<Invitation> SentInvitations { get; set; }
        public List<Invitation> ReceivedInvitations { get; set; }
    }
}