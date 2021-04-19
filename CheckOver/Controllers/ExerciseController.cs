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

        public async Task<IActionResult> ShowAssignedExercises()
        {
            var data = await exerciseRepository.GetUserSolvings();
            return View(data);
        }

        public async Task<IActionResult> ShowCheckedExercise(int SolvingId)
        {
            var data = await exerciseRepository.GetSolvingById(SolvingId);
            return View(data);
        }

        [HttpGet]
        [Route("zadanie/{SolvingId}/rozwiazanie")]
        public async Task<IActionResult> SolveTheExercise(int SolvingId)
        {
            SolvedExerciseVM solvedExerciseVM = new SolvedExerciseVM();
            solvedExerciseVM.Solving = await exerciseRepository.GetSolvingById(SolvingId);
            return View(solvedExerciseVM);
        }

        [HttpPost]
        [Route("zadanie/{SolvingId}/rozwiazanie")]
        public async Task<IActionResult> SolveTheExercise(SolvedExerciseVM solvedExerciseVM, int SolvingId)
        {
            if (ModelState.IsValid)
            {
                await exerciseRepository.ReceiveSolvedExercise(solvedExerciseVM, SolvingId);
                return RedirectToAction(nameof(ShowAssignedExercises));
            }
            return RedirectToAction(nameof(ShowAssignedExercises));
        }

        public async Task<IActionResult> ShowExercisesToCheck()
        {
            var data = await exerciseRepository.ShowExercisesToCheck();
            return View(data);
        }

        [HttpGet]
        [Route("zadanie/{SolvingId}/sprawdzanie")]
        public async Task<IActionResult> CheckTheExercise(int SolvingId)
        {
            CheckTheExerciseVM checkTheExerciseVM = new CheckTheExerciseVM();
            checkTheExerciseVM.Solving = await exerciseRepository.GetSolvingById(SolvingId);
            return View(checkTheExerciseVM);
        }

        [HttpPost]
        [Route("zadanie/{SolvingId}/sprawdzanie")]
        public async Task<IActionResult> CheckTheExercise(CheckTheExerciseVM checkTheExerciseVM, int SolvingId)
        {
            if (ModelState.IsValid)
            {
                await exerciseRepository.ProcessCheckedExercise(checkTheExerciseVM, SolvingId);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ShowCheckedExercises()
        {
            var data = await exerciseRepository.ShowCheckedExercises();
            return View(data);
        }

        public async Task<IActionResult> Index()
        {
            var data = await exerciseRepository.GetUserExercises();
            return View(data);
        }
    }
}