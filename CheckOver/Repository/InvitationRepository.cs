using CheckOver.Data;
using CheckOver.Models;
using CheckOver.Models.ViewModels;
using CheckOver.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Repository
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IUserService userService;

        public InvitationRepository(ApplicationDbContext context, IUserService userService)
        {
            this.context = context;
            this.userService = userService;
        }

        public async Task<string> AddNewInvitation(InvitationVM invitationVM, int id)
        {
            var senderId = userService.GetUserId();
            var sender = await context.Users.FirstOrDefaultAsync(x => x.Id == senderId);
            var receiver = await context.Users.FirstOrDefaultAsync(x => x.Email == invitationVM.Email);
            var group = await context.Groups.FirstOrDefaultAsync(x => x.GroupId == id);
            var assignmentTmp = await context.Assignments
                .FirstOrDefaultAsync(x => x.User == receiver && x.GroupId == id);
            var invitationTmp = await context.Invitations
                .FirstOrDefaultAsync(x => x.GroupId == id && x.Receiver == receiver);

            if (sender == receiver) { return "Nie możesz wysłać zaproszenia do samego siebie."; }
            if (receiver == null) { return "Nie znaleziono użytkownika z podanem adresem E-mail."; }
            if (assignmentTmp != null) { return "Zapraszany użytkownik już należy do tej grupy."; }
            if (invitationTmp != null) { return "Istnieje już takie samo zaproszenie"; }

            var newInvitation = new Invitation()
            {
                Sender = sender,
                Receiver = receiver,
                Status = "Oczekujące",
                Group = group,
                CreatedAt = DateTime.Now,
                Role = context.Roles.FirstOrDefault(x => x.Name == "Creator")
            };
            await context.Invitations.AddAsync(newInvitation);
            await context.SaveChangesAsync();
            return "Sukces";
        }

        public async Task<List<Invitation>> GetSentInvitations()
        {
            var userId = userService.GetUserId();
            var User = context.Users.FirstOrDefault(x => x.Id == userId);

            return await context.Invitations.Include(x => x.Sender)
                .Include(x => x.Receiver)
                .Include(x => x.Role)
                .Include(x => x.Group)
                .Where(x => x.Sender == User).ToListAsync();
        }

        public async Task<List<Invitation>> GetReceivedInvitations()
        {
            var userId = userService.GetUserId();
            var User = context.Users.FirstOrDefault(x => x.Id == userId);

            return await context.Invitations.Include(x => x.Sender)
                .Include(x => x.Receiver)
                .Include(x => x.Role)
                .Include(x => x.Group)
                .Where(x => x.Receiver == User).ToListAsync();
        }

        public async Task<int> AcceptInvitation(int id)
        {
            var invitation = await context.Invitations
                .Include(x => x.Receiver)
                .Include(x => x.Group)
                .Include(x => x.Role)
                .Include(x => x.Sender).SingleOrDefaultAsync(x => x.InvitationId == id);
            if (invitation.Status == "Oczekujące")
            {
                Assignment newAssignment = new Assignment()
                {
                    Group = invitation.Group,
                    GroupId = invitation.GroupId,
                    User = invitation.Receiver,
                    Role = invitation.Role,
                    RoleId = invitation.Role.RoleId,
                    CreatedAt = DateTime.Now
                };
                await context.Assignments.AddAsync(newAssignment);
                invitation.Status = "Zaakceptowane";
                await context.SaveChangesAsync();
                return newAssignment.AssignmentId;
            }
            return 0;
        }
    }
}