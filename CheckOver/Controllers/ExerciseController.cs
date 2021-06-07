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
    public class ExerciseController : Controller
    {
        private readonly IExerciseRepository exerciseRepository;
        private readonly IGroupRepository groupRepository;
        private readonly IUserService userService;

        public ExerciseController(IExerciseRepository exerciseRepository, IGroupRepository groupRepository, IUserService userService)
        {
            this.exerciseRepository = exerciseRepository;
            this.groupRepository = groupRepository;
            this.userService = userService;
        }

        [HttpGet]
        [Route("zadanie/dodaj-zadanie")]
        public IActionResult AddExercise()
        {
            return View();
        }

        [HttpPost]
        [Route("zadanie/dodaj-zadanie")]
        public async Task<IActionResult> AddExercise(MakeOrUpdateExerciseVM makeExerciseVM)
        {
            int id = await exerciseRepository.AddExercise(makeExerciseVM);
            return RedirectToAction("Index");
        }

        [Route("zadanie/edytuj/{ExerciseId}")]
        [HttpGet]
        public async Task<IActionResult> UpdateExercise(int ExerciseId)
        {
            MakeOrUpdateExerciseVM makeOrUpdateExerciseVM = new MakeOrUpdateExerciseVM();
            var exercise = await exerciseRepository.GetExerciseById(ExerciseId);
            makeOrUpdateExerciseVM.Description = exercise.Description;
            makeOrUpdateExerciseVM.Title = exercise.Title;
            makeOrUpdateExerciseVM.MaxPoints = exercise.MaxPoints;
            return View(makeOrUpdateExerciseVM);
        }

        [Route("zadanie/edytuj/{ExerciseId}")]
        [HttpPost]
        public async Task<IActionResult> UpdateExercise(int ExerciseId, MakeOrUpdateExerciseVM makeOrUpdateExerciseVM)
        {
            await exerciseRepository.UpdateExercise(ExerciseId, makeOrUpdateExerciseVM);
            return RedirectToAction("Index");
        }

        [Route("zadanie/przypisz/{ExerciseId}")]
        public async Task<IActionResult> AssignExercise(int ExerciseId)
        {
            var data = await exerciseRepository.getGroupsWithPrivilege();
            ViewBag.ExerciseId = ExerciseId;
            return View(data);
        }

        [HttpGet]
        [Route("zadanie/przypisz/{ExerciseId}/{GroupId}")]
        public async Task<IActionResult> AssignExerciseToUsers(int GroupId, int ExerciseId)
        {
            AssignExerciseVM assignExerciseVM = new AssignExerciseVM();
            assignExerciseVM.Exercise = await exerciseRepository.GetExerciseById(ExerciseId);
            return View(assignExerciseVM);
        }

        [HttpPost]
        [Route("zadanie/przypisz/{ExerciseId}/{GroupId}")]
        public async Task<IActionResult> AssignExerciseToUsers(int GroupId, int ExerciseId, AssignExerciseVM assignExerciseVM)
        {
            if (ModelState.IsValid)
            {
                await exerciseRepository.AssignExerciseToUsers(GroupId, ExerciseId, assignExerciseVM);
            }
            return RedirectToAction("Index");
        }

        [Route("zadanie/przypisane-zadania")]
        public async Task<IActionResult> ShowAssignedExercises()
        {
            var data = await exerciseRepository.GetUserSolvings();
            return View(data);
        }

        [Route("zadanie/sprawdzone-zadanie/{SolvingId}")]
        public async Task<IActionResult> ShowCheckedExercise(int SolvingId)
        {
            var data = await exerciseRepository.GetSolvingById(SolvingId);
            return View(data);
        }

        [HttpGet]
        [Route("zadanie/rozwiazanie/{SolvingId}")]
        public async Task<IActionResult> SolveTheExercise(int SolvingId)
        {
            SolvedExerciseVM solvedExerciseVM = new SolvedExerciseVM();
            solvedExerciseVM.Solving = await exerciseRepository.GetSolvingById(SolvingId);
            return View(solvedExerciseVM);
        }

        [HttpPost]
        [Route("zadanie/rozwiazanie/{SolvingId}")]
        public async Task<IActionResult> SolveTheExercise(SolvedExerciseVM solvedExerciseVM, int SolvingId)
        {
            if (ModelState.IsValid)
            {
                await exerciseRepository.ReceiveSolvedExercise(solvedExerciseVM, SolvingId);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("zadanie/zadania-do-sprawdzenia")]
        public async Task<IActionResult> ShowExercisesToCheck()
        {
            var data = await exerciseRepository.ShowExercisesToCheck();
            return View(data);
        }

        [HttpGet]
        [Route("zadanie/sprawdzanie/{SolvingId}")]
        public async Task<IActionResult> CheckTheExercise(int SolvingId)
        {
            CheckTheExerciseVM checkTheExerciseVM = new CheckTheExerciseVM();
            checkTheExerciseVM.Solving = await exerciseRepository.GetSolvingById(SolvingId);
            return View(checkTheExerciseVM);
        }

        [HttpPost]
        [Route("zadanie/sprawdzanie/{SolvingId}")]
        public async Task<IActionResult> CheckTheExercise(CheckTheExerciseVM checkTheExerciseVM, int SolvingId)
        {
            if (ModelState.IsValid)
            {
                await exerciseRepository.ProcessCheckedExercise(checkTheExerciseVM, SolvingId);
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("zadanie/wybierz-zadanie")]
        public async Task<IActionResult> ChooseExercise(int GroupId)
        {
            ViewBag.GroupId = GroupId;
            var data = await exerciseRepository.GetUserExercises();
            return View(data);
        }

        public async Task<IActionResult> Index()
        {
            SolvingsVM solvingsVM = new SolvingsVM();
            solvingsVM.Checked = await exerciseRepository.ShowCheckedExercises();
            solvingsVM.ToCheck = await exerciseRepository.ShowExercisesToCheck();
            solvingsVM.ToSolve = await exerciseRepository.GetUserSolvings();
            solvingsVM.CheckedByUser = await exerciseRepository.ShowCheckedExercisesByUsers();
            return View(solvingsVM);
        }

        public async Task<IActionResult> DeleteExercise(int ExerciseId)
        {
            await exerciseRepository.DeleteExercise(ExerciseId);
            return RedirectToAction("Index");
        }

        [Route("zadanie/utworzone-zadania")]
        public async Task<IActionResult> ShowCreatedExercises()
        {
            var data = await exerciseRepository.GetUserExercises();
            return View(data);
        }

        [HttpGet]
        [Route("zadanie/konfiguracja/{ExerciseId}")]
        public async Task<IActionResult> ConfigureExercise(int ExerciseId)
        {
            var data = await exerciseRepository.GetExerciseById(ExerciseId);
            ConfigureExerciseVM configureExerciseVM = new ConfigureExerciseVM();
            configureExerciseVM.Title = data.Title;
            configureExerciseVM.Description = data.Description;
            configureExerciseVM.MaxPoints = data.MaxPoints;
            configureExerciseVM.Arguments = data.Arguments;
            configureExerciseVM.ValidCode = data.ValidCode;
            return View(configureExerciseVM);
        }

        [HttpPost]
        [Route("zadanie/konfiguracja/{ExerciseId}")]
        public async Task<IActionResult> ConfigureExercise(ConfigureExerciseVM configureExerciseVM, int ExerciseId)
        {
            await exerciseRepository.ConfigureExercise(configureExerciseVM, ExerciseId);
            return RedirectToAction(nameof(Index));
        }
    }
}