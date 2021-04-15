using CheckOver.Models.ViewModels;
using CheckOver.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Controllers
{
    public class ExerciseController : Controller
    {
        private readonly IExerciseRepository exerciseRepository;
        private readonly IGroupRepository groupRepository;

        public ExerciseController(IExerciseRepository exerciseRepository, IGroupRepository groupRepository)
        {
            this.exerciseRepository = exerciseRepository;
            this.groupRepository = groupRepository;
        }

        [HttpGet]
        public IActionResult AddExercise()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddExercise(MakeExerciseVM makeExerciseVM)
        {
            int id = await exerciseRepository.AddExercise(makeExerciseVM);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AssignExercise(int ExerciseId)
        {
            var groups = await groupRepository.GetUsersGroups();
            ViewBag.ExerciseId = ExerciseId;
            return View(groups);
        }

        public async Task<IActionResult> AssignExerciseToUsers(int GroupId, int ExerciseId)
        {
            await exerciseRepository.AssignExerciseToUsers(GroupId, ExerciseId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var data = await exerciseRepository.GetUserExercises();
            return View(data);
        }
    }
}