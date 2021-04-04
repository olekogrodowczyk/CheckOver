using CheckOver.Data;
using CheckOver.Models;
using CheckOver.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment webHostEnvironment;

        public GroupRepository(ApplicationDbContext context, IUserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userService = userService;
            this.webHostEnvironment = webHostEnvironment;
        }

        public int function()
        {
            return 0;
        }

        public string id()
        {
            var userId = _userService.GetUserId();
            return userId;
        }

        public async Task<int> AddNewGroup(MakeGroupModel makeGroupModel)
        {
            var userId = _userService.GetUserId();
            var User = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (makeGroupModel.CoverPhoto != null)
            {
                string folder = "group/cover/";
                folder += Guid.NewGuid().ToString() + "_" + makeGroupModel.CoverPhoto.FileName;
                makeGroupModel.CoverImageUrl = folder;
                string serverFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
                await makeGroupModel.CoverPhoto.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            }
            var newGroup = new Group()
            {
                Admin = User,
                Name = makeGroupModel.Name,
                CreateDate = DateTime.Now,
                CoverImageUrl = makeGroupModel.CoverImageUrl
            };
            var newRole = _context.Roles.FirstOrDefault(x => x.Name == "Creator");
            var newAssignment = new Assignment()
            {
                User = User,
                Role = newRole,
                Group = newGroup
            };
            await _context.Groups.AddAsync(newGroup);
            await _context.Assignments.AddAsync(newAssignment);
            await _context.SaveChangesAsync();
            return newGroup.Id;
        }

        public async Task<List<Group>> GetAllGroups()
        {
            return await _context.Groups.Take(2).ToListAsync();
        }

        public async Task<List<Group>> GetUsersGroups()
        {
            var userId = _userService.GetUserId();
            var User = _context.Users.FirstOrDefault(x => x.Id == userId);
            var groups = await _context.Assignments.Include(x => x.Group.Admin).Where(x => x.User == User).
                Select(x => x.Group).ToListAsync();
            return groups;
        }

        public async Task<Group> GetGroupById(int id)
        {
            return await _context.Groups.Where(x => x.Id == id)
                .Select(group => new Group()
                {
                    Admin = group.Admin,
                    CreateDate = group.CreateDate,
                    Id = group.Id,
                    Name = group.Name,
                    CoverImageUrl = group.CoverImageUrl,
                    Invitations = group.Invitations.Select(i => new Invitation()
                    {
                        Id = i.Id,
                        Group = i.Group,
                        Sender = i.Sender,
                        Receiver = i.Receiver,
                        Status = i.Status,
                        Role = i.Role
                    }).ToList(),
                    Assignments = group.Assignments.Select(a => new Assignment()
                    {
                        Id = a.Id,
                        Group = a.Group,
                        Role = a.Role,
                        GroupId = a.GroupId
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<int> ApplyGroupSettings(GroupSettings groupSettings, int id)
        {
            var result = _context.Groups.SingleOrDefault(g => g.Id == id);
            if (result != null)
            {
                if (groupSettings.CoverPhoto != null)
                {
                    string folder = "group/cover";
                    folder += Guid.NewGuid().ToString() + "_" + groupSettings.CoverPhoto.FileName;
                    groupSettings.CoverImageUrl = folder;
                    string serverFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
                    await groupSettings.CoverPhoto.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                }
                if (groupSettings.Name != null)
                {
                    result.Name = groupSettings.Name;
                }
                if (groupSettings.CoverImageUrl != null)
                {
                    result.CoverImageUrl = groupSettings.CoverImageUrl;
                }
                await _context.SaveChangesAsync();
                return id;
            }
            return 0;
        }
    }
}