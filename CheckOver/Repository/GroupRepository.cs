using CheckOver.Data;
using CheckOver.Models;
using CheckOver.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CheckOver.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public GroupRepository(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
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
            var newGroup = new Group()
            {
                Admin = User,
                Name = makeGroupModel.Name,
                CreateDate = DateTime.Now
            };
            await _context.Groups.AddAsync(newGroup);
            await _context.SaveChangesAsync();
            return newGroup.Id;
        }

        public async Task<List<Group>> GetAllGroups()
        {
            return await _context.Groups
                .Select(group => new Group()
                {
                    Admin = group.Admin,
                    CreateDate = group.CreateDate,
                    Name = group.Name,
                    Id = group.Id
                }).ToListAsync();
        }
        
    }
}
