using CheckOver.Data;
using CheckOver.Models;
using CheckOver.Models.ViewModels;
using CheckOver.Repository;
using CheckOver.Service;
using Microsoft.AspNetCore.Authorization;
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
            data.Checkers = await groupRepository.getCheckers(id);
            data.Solvers = await groupRepository.getSolvers(id);
            return View(data);
        }

        [HttpGet]
        public ViewResult GetUsers(int GroupId)
        {
            return View();
        }

        public async Task<IActionResult> DeleteUser(int groupId, string userId)
        {
            bool ifSucceed = await groupRepository.DeleteUserFromGroup(groupId, userId);
            return RedirectToAction("GetGroup", new { id = groupId });
        }

        public async Task<IActionResult> LeaveGroup(int groupId)
        {
            var userId = _userService.GetUserId();
            bool ifSucceed = await groupRepository.DeleteUserFromGroup(groupId, userId);
            return RedirectToAction("GetUsersGroups");
        }

        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            bool ifSucceed = await groupRepository.DeteleGroup(groupId);
            return RedirectToAction("GetUsersGroups");
        }

        [Route("edycja-grupy/{groupId}")]
        public async Task<IActionResult> EditGroup(int groupId)
        {
            GetGroupVM data = new GetGroupVM();
            data.Group = await groupRepository.GetGroupById(groupId);
            data.Checkers = await groupRepository.getCheckers(groupId);
            data.Solvers = await groupRepository.getSolvers(groupId);
            return View(data);
        }

        public async Task<IActionResult> ChangeRole(int groupId, string userId)
        {
            await groupRepository.ChangeRole(groupId, userId);
            return RedirectToAction(nameof(EditGroup), new { groupId = groupId });
        }

        [Route("edycja-grupy/zmiana-zdjecia/{groupId}")]
        [HttpGet]
        public IActionResult ChangeGroupPhoto(int groupId)
        {
            return View();
        }

        [Route("edycja-grupy/zmiana-zdjecia/{groupId}")]
        [HttpPost]
        public async Task<IActionResult> ChangeGroupName(int groupId, ChangeGroupPhotoVM changeGroupPhotoVM)
        {
            await groupRepository.ChangeGroupPhoto(groupId, changeGroupPhotoVM);
            return RedirectToAction(nameof(EditGroup), new { groupId = groupId });
        }

        [Route("edycja-grupy/zmiana-nazwy/{groupId}")]
        [HttpGet]
        public IActionResult ChangeGroupName(int groupId)
        {
            return View();
        }

        [Route("edycja-grupy/zmiana-nazwy/{groupId}")]
        [HttpPost]
        public async Task<IActionResult> ChangeGroupName(int groupId, ChangeGroupNameVM changeGroupNameVM)
        {
            await groupRepository.ChangeGroupName(groupId, changeGroupNameVM);
            return RedirectToAction(nameof(EditGroup), new { groupId = groupId });
        }
    }
}