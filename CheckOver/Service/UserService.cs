using CheckOver.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CheckOver.Service
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly ApplicationDbContext context;

        public UserService(IHttpContextAccessor httpContext, ApplicationDbContext context)
        {
            _httpContext = httpContext;
            this.context = context;
        }

        public string GetUserId()
        {
            return _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string GetUserEmail()
        {
            return _httpContext.HttpContext.User?.FindFirstValue(ClaimTypes.Email);
        }

        public bool CheckIfUserHasPermission(string permissionString, int groupId, string userId = null)
        {
            if (userId == null) { userId = GetUserId(); }
            int? permissionId = context.Permissions.FirstOrDefault(x => x.Title == permissionString).PermissionId;
            if (permissionId != null)
            {
                var rolePermissions = context.Assignments
                    .Include(x => x.Role)
                    .ThenInclude(x => x.RolePermissions)
                    .FirstOrDefault(x => x.UserId == userId && x.GroupId == groupId)
                    .Role
                    .RolePermissions;
                var permission = rolePermissions.FirstOrDefault(x => x.PermissionId == permissionId);
                return permission == null ? false : true;
            }
            return false;
        }

        public bool ifCreator(int id)
        {
            var userId = GetUserId();
            var group = context.Groups
                .FirstOrDefault(x => x.GroupId == id);
            return userId == group.CreatorId ? true : false;
        }
    }
}