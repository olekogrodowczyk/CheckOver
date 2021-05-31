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
using CheckOver.Models.ViewModels;

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

        public async Task<List<Assignment>> getSolvers(int groupId)
        {
            var assignments = await getMembers(groupId);
            var list = new List<Assignment>();
            foreach (var item in assignments)
            {
                if (userService.CheckIfUserHasPermission("Wykonanie zadania", item.GroupId, item.UserId))
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public async Task<List<Assignment>> getCheckers(int groupId)
        {
            var assignments = await getMembers(groupId);
            var list = new List<Assignment>();
            foreach (var item in assignments)
            {
                if (userService.CheckIfUserHasPermission("Sprawdzanie zadania", item.GroupId, item.UserId))
                {
                    list.Add(item);
                }
            }
            return list;
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
                Creator = User,
                Name = makeGroupModel.Name,
                CreatedAt = DateTime.Now,
                CoverImageUrl = makeGroupModel.CoverImageUrl
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

        public async Task<bool> DeleteUserFromGroup(int groupId, string userId)
        {
            var assignment = await context.Assignments.FirstOrDefaultAsync(x => x.UserId == userId && x.GroupId == groupId);
            context.Assignments.Remove(assignment);
            await context.SaveChangesAsync();
            return assignment == null ? false : true;
        }

        public async Task<bool> DeteleGroup(int groupId)
        {
            var assignments = await context.Assignments.Where(x => x.GroupId == groupId).ToListAsync();
            var group = await context.Groups.FirstOrDefaultAsync(x => x.GroupId == groupId);
            context.Assignments.RemoveRange(assignments);
            context.Groups.Remove(group);
            await context.SaveChangesAsync();
            return assignments == null && group == null ? false : true;
        }

        public async Task ChangeRole(int groupId, string userId)
        {
            var assignment = await context.Assignments
                .Include(x => x.Role)
                .ThenInclude(x => x.RolePermissions)
                .ThenInclude(x => x.Permission)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.GroupId == groupId);
            var Role = new Role();
            if (assignment.Role.Name == "Sprawdzający")
            {
                Role = await context.Roles.FirstOrDefaultAsync(x => x.Name == "Uczeń");
            }
            else
            {
                Role = await context.Roles.FirstOrDefaultAsync(x => x.Name == "Sprawdzający");
            }
            assignment.Role = Role;
            await context.SaveChangesAsync();
        }

        public async Task ChangeGroupPhoto(int groupId, ChangeGroupPhotoVM changeGroupPhotoVM)
        {
            var group = await context.Groups.FirstOrDefaultAsync(x => x.GroupId == groupId);
            if (changeGroupPhotoVM.CoverPhoto != null)
            {
                string folder = "group/cover/";
                folder += Guid.NewGuid().ToString() + "_" + changeGroupPhotoVM.CoverPhoto.FileName;
                changeGroupPhotoVM.CoverImageUrl = folder;
                string serverFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
                await changeGroupPhotoVM.CoverPhoto.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            }
            group.CoverImageUrl = changeGroupPhotoVM.CoverImageUrl;
            await context.SaveChangesAsync();
        }

        public async Task ChangeGroupName(int groupId, ChangeGroupNameVM changeGroupNameVM)
        {
            var group = await context.Groups.FirstOrDefaultAsync(x => x.GroupId == groupId);
            group.Name = changeGroupNameVM.Name;
            await context.SaveChangesAsync();
        }
    }
}