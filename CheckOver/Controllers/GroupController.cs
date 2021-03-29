using CheckOver.Models;
using CheckOver.Repository;
using CheckOver.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CheckOver.Controllers
{
    public class GroupController : Controller
    {
        private readonly IUserService _userService;
        private readonly IGroupRepository groupRepository;

        public GroupController(IUserService userService, IGroupRepository groupRepository)
        {
            _userService = userService;
            this.groupRepository = groupRepository;
        }
        public IActionResult Index()
        {
            var userId = groupRepository.id();
            ViewBag.userId = userId;
            return View();
        }

        [HttpGet]
        [Route("tworzenie-grupy")]
        public IActionResult AddNewGroup()
        {
            return View();
        }

        [HttpPost]
        [Route("tworzenie-grupy")]
        public async Task<IActionResult> AddNewGroup(MakeGroupModel makeGroupModel)
        {
            if(ModelState.IsValid)
            {
                int id = await groupRepository.AddNewGroup(makeGroupModel);
                if (id>0)
                {
                    return RedirectToAction(nameof(AddNewGroup), new { isSuccess = true, bookId = id });
                }
            }
            return View();
        }

        public async Task<ViewResult> GetAllGroups()
        {
            var data = await groupRepository.GetAllGroups();
            return View(data);
        }
    }
}
