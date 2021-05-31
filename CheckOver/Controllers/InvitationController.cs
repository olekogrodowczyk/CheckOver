using CheckOver.Models;
using CheckOver.Models.ViewModels;
using CheckOver.Repository;
using CheckOver.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Controllers
{
    public class InvitationController : Controller
    {
        private readonly IInvitationRepository invitationRepository;

        public InvitationController(IInvitationRepository invitationRepository)
        {
            this.invitationRepository = invitationRepository;
        }

        [HttpGet]
        [Route("tworzenie-zaproszenia/{groupId}")]
        public IActionResult AddNewInvitation(int groupId)
        {
            return View();
        }

        [HttpPost]
        [Route("tworzenie-zaproszenia/{groupId}")]
        public async Task<IActionResult> AddNewInvitation(InvitationVM invitationVM, int groupId)
        {
            if (ModelState.IsValid)
            {
                string result = await invitationRepository.AddNewInvitation(invitationVM, groupId);
                if (result == "Sukces")
                {
                    return RedirectToAction(nameof(AddNewInvitation), new { isSuccess = true, groupId = groupId });
                }
                else
                {
                    ModelState.AddModelError("", result);
                }
            }
            return View();
        }

        [HttpGet]
        [Route("Wysłane")]
        public async Task<IActionResult> GetAllSent()
        {
            var invitations = await invitationRepository.GetSentInvitations();
            return View(invitations);
        }

        [HttpGet]
        [Route("Odebrane")]
        public async Task<IActionResult> GetAllReceived()
        {
            var invitations = await invitationRepository.GetReceivedInvitations();
            return View(invitations);
        }

        public async Task<IActionResult> AcceptInvitation(int id)
        {
            int result = await invitationRepository.AcceptInvitation(id);
            if (result > 0)
            {
                return RedirectToAction(nameof(Index), new { isSuccess = true, AssignmentId = result });
            }
            return RedirectToAction(nameof(Index), new { isSuccess = false });
        }

        public async Task<IActionResult> RejectInvitation(int id)
        {
            int result = await invitationRepository.RejectInvitation(id);
            if (result > 0)
            {
                return RedirectToAction(nameof(Index), new { isSuccess = true, AssignmentId = result });
            }
            return RedirectToAction(nameof(Index), new { isSuccess = false });
        }

        public async Task<IActionResult> Index()
        {
            InvitationsVM invitationsVM = new InvitationsVM();
            invitationsVM.ReceivedInvitations = await invitationRepository.GetReceivedInvitations();
            invitationsVM.SentInvitations = await invitationRepository.GetSentInvitations();
            return View(invitationsVM);
        }
    }
}