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
        private readonly ApplicationDbContext _context;
        private readonly IUserService userService;

        public InvitationRepository(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            this.userService = userService;
        }

        public async Task<string> AddNewInvitation(InvitationVM invitationVM, int id)
        {
            var senderId = userService.GetUserId();
            var sender = await _context.Users.FirstOrDefaultAsync(x => x.Id == senderId);
            var receiver = await _context.Users.FirstOrDefaultAsync(x => x.Email == invitationVM.Email);
            var group = await _context.Groups.FirstOrDefaultAsync(x => x.Id == id);
            var assignmentTmp = await _context.Assignments
                .FirstOrDefaultAsync(x => x.User == receiver && x.GroupId == id);
            var invitationTmp = await _context.invitations
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
                CreationDate = DateTime.Now,
                Role = _context.Roles.FirstOrDefault(x => x.Name == "Creator")
            };
            await _context.invitations.AddAsync(newInvitation);
            await _context.SaveChangesAsync();
            return "Sukces";
        }

        public async Task<List<Invitation>> GetSentInvitations()
        {
            var userId = userService.GetUserId();
            var User = _context.Users.FirstOrDefault(x => x.Id == userId);

            var invitations = await _context.invitations.Where(x => x.Sender == User)
                .Select(i => new Invitation()
                {
                    Id = i.Id,
                    GroupId = i.GroupId,
                    Sender = i.Sender,
                    Receiver = i.Receiver,
                    Status = i.Status,
                    Role = _context.Roles.SingleOrDefault(r => r.Name == "Creator"),
                    CreationDate = i.CreationDate,
                    Group = _context.Groups.SingleOrDefault(g => g.Id == i.GroupId)
                }).ToListAsync();

            return invitations;
        }

        public async Task<List<Invitation>> GetReceivedInvitations()
        {
            var userId = userService.GetUserId();
            var User = _context.Users.FirstOrDefault(x => x.Id == userId);

            var invitations = await _context.invitations.Where(x => x.Receiver == User)
                .Select(i => new Invitation()
                {
                    Id = i.Id,
                    GroupId = i.GroupId,
                    Sender = i.Sender,
                    Receiver = i.Receiver,
                    Status = i.Status,
                    Role = _context.Roles.SingleOrDefault(r => r.Name == "Creator"),
                    CreationDate = i.CreationDate,
                    Group = _context.Groups.SingleOrDefault(g => g.Id == i.GroupId)
                }).ToListAsync();

            return invitations;
        }

        public async Task<int> AcceptInvitation(int id)
        {
            var invitation = await _context.invitations
                .Include(x => x.Receiver)
                .Include(x => x.Group)
                .Include(x => x.Role)
                .Include(x => x.Sender).SingleOrDefaultAsync(x => x.Id == id);
            if (invitation.Status == "Oczekujące")
            {
                Assignment newAssignment = new Assignment()
                {
                    Group = invitation.Group,
                    GroupId = invitation.GroupId,
                    User = invitation.Receiver,
                    Role = invitation.Role,
                    RoleId = invitation.Role.Id
                };
                await _context.Assignments.AddAsync(newAssignment);
                invitation.Status = "Zaakceptowane";
                await _context.SaveChangesAsync();
                return newAssignment.Id;
            }
            return 0;
        }

        public int function()
        {
            return 0;
        }
    }
}