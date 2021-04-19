using CheckOver.Data;
using CheckOver.Models;
using CheckOver.Models.ViewModels;
using CheckOver.Repository;
using CheckOver.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CheckOver.Controllers
{
    public class GroupController : Controller
    {
        private readonly IUserService _userService;
        private readonly IGroupRepository groupRepository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ApplicationDbContext applicationDbContext;

        public GroupController(IUserService userService, IGroupRepository groupRepository, IWebHostEnvironment webHostEnvironment,
            ApplicationDbContext applicationDbContext)
        {
            _userService = userService;
            this.groupRepository = groupRepository;
            this.webHostEnvironment = webHostEnvironment;
            this.applicationDbContext = applicationDbContext;
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
        public async Task<IActionResult> AddNewGroup(MakeGroupVM makeGroupModel)
        {
            if (ModelState.IsValid)
            {
                int id = await groupRepository.AddNewGroup(makeGroupModel);
                if (id > 0)
                {
                    return RedirectToAction(nameof(AddNewGroup), new { isSuccess = true, groupId = id });
                }
            }
            return View();
        }

        [HttpGet]
        [Route("Twoje-grupy")]
        public async Task<ViewResult> GetUsersGroups()
        {
            var data = await groupRepository.GetUsersGroups();
            return View(data);
        }

        [Route("grupa/{id}")]
        public async Task<ViewResult> GetGroup(int id)
        {
            GetGroupVM data = new GetGroupVM();
            data.Group = await groupRepository.GetGroupById(id);
            data.Assignments = await groupRepository.getMembers(id);
            return View(data);
        }

        [HttpGet]
        public ViewResult GetUsers(int GroupId)
        {
            return View();
        }

        [HttpGet]
        [Route("grupa/{id}/ustawienia")]
        public ViewResult GroupSettings(int id)
        {
            return View();
        }

        [Route("grupa/{id}/ustawienia")]
        [HttpPost]
        public async Task<ActionResult> GroupSettings(GroupSettingsVM groupSettings, int id)
        {
            if (ModelState.IsValid)
            {
                await groupRepository.ApplyGroupSettings(groupSettings, id);
                return RedirectToAction("GetUsersGroups", "Group");
            }
            return View();
        }
    }
}