using CheckOver.Data;
using CheckOver.Models;
using CheckOver.Service;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace CheckOver.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IUserService userService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IBlobService blobService;

        public GroupRepository(ApplicationDbContext context, IUserService userService,
            IWebHostEnvironment webHostEnvironment, IBlobService blobService)
        {
            this.context = context;
            this.userService = userService;
            this.webHostEnvironment = webHostEnvironment;
            this.blobService = blobService;
        }

        public string id()
        {
            var userId = userService.GetUserId();
            return userId;
        }

        public async Task<List<Assignment>> getMembers(int groupId)
        {
            var group = await context.Groups.Include(x => x.Assignments)
                .ThenInclude(x => x.User).Include(x => x.Assignments).ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.GroupId == groupId);
            var assignments = group.Assignments.ToList();
            return assignments;
        }

        public int GetUsers(int GroupId)
        {
            var group = context.Groups.FirstOrDefaultAsync(x => x.GroupId == GroupId);
            return 0;
        }

        public async Task<string> getGroupPhoto(int id)
        {
            var group = await context.Groups.FirstOrDefaultAsync(x => x.GroupId == id);
            return group.CoverImageUrl;
        }

        public async Task<string> AddNewPhotoToServer(IFormFile CoverPhoto, string CoverImageUrl)
        {
            var uploads = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
            bool exists = Directory.Exists(uploads);
            if (!exists)
            {
                Directory.CreateDirectory(uploads);
            }
            string fileName = Guid.NewGuid().ToString() + "_" + CoverPhoto.FileName;
            CoverImageUrl = fileName;
            var fileStream = CoverPhoto.OpenReadStream();
            string contentType = CoverPhoto.ContentType;
            string serverFolder = Path.Combine(webHostEnvironment.WebRootPath, fileName);
            await blobService.UploadFileBlobAsync("checkoverblob", fileStream, contentType, fileName);
            return CoverImageUrl;
        }

        public async Task<int> AddNewGroup(MakeGroupVM makeGroupModel)
        {
            var userId = userService.GetUserId();
            var User = context.Users.FirstOrDefault(x => x.Id == userId);
            string url = "";
            if (makeGroupModel.CoverPhoto != null)
            {
                url = await AddNewPhotoToServer(makeGroupModel.CoverPhoto, makeGroupModel.CoverImageUrl);
            }
            var newGroup = new Group()
            {
                Creator = User,
                Name = makeGroupModel.Name,
                CreatedAt = DateTime.Now,
                CoverImageUrl = url
            };
            var newRole = context.Roles.FirstOrDefault(x => x.Name == "Założyciel");
            var newAssignment = new Assignment()
            {
                User = User,
                Role = newRole,
                Group = newGroup,
                CreatedAt = DateTime.Now
            };
            await context.Groups.AddAsync(newGroup);
            await context.Assignments.AddAsync(newAssignment);
            await context.SaveChangesAsync();
            return newGroup.GroupId;
        }

        public async Task<List<Group>> GetAllGroups()
        {
            return await context.Groups.Take(2).ToListAsync();
        }

        public async Task<List<Group>> GetUsersGroups()
        {
            var userId = userService.GetUserId();
            var User = context.Users.FirstOrDefault(x => x.Id == userId);
            var groups = await context.Assignments.Include(x => x.Group.Creator).Where(x => x.User == User).
                Select(x => x.Group).ToListAsync();
            return groups;
        }

        public async Task<Group> GetGroupById(int id)
        {
            var groups = await context.Groups
                .Include(x => x.Invitations)
                .Include(x => x.Creator)
                .Include(x => x.Assignments)
                .FirstOrDefaultAsync(x => x.GroupId == id);
            return groups;
        }

        public async Task<int> ApplyGroupSettings(GroupSettingsVM groupSettings, int id)
        {
            var result = context.Groups.SingleOrDefault(g => g.GroupId == id);
            string url = "";
            if (result != null)
            {
                if (groupSettings.CoverPhoto != null)
                {
                    url = await AddNewPhotoToServer(groupSettings.CoverPhoto, groupSettings.CoverImageUrl);
                }
                if (groupSettings.Name != null) { result.Name = groupSettings.Name; }
                if (groupSettings.CoverPhoto != null) { result.CoverImageUrl = url; }
                await context.SaveChangesAsync();
                return id;
            }
            return 0;
        }
    }
}