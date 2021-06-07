using CheckOver.Data;
using CheckOver.Models;
using CheckOver.Models.ViewModels;
using CheckOver.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckOver.Repository
{
    public class ExerciseRepository : IExerciseRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IUserService userService;
        private readonly IGroupRepository groupRepository;

        public ExerciseRepository(ApplicationDbContext context, IUserService userService, IGroupRepository groupRepository)
        {
            this.context = context;
            this.userService = userService;
            this.groupRepository = groupRepository;
        }

        public async Task<int> AddExercise(MakeOrUpdateExerciseVM makeExerciseVM)
        {
            var userId = userService.GetUserId();
            var User = context.Users.FirstOrDefault(x => x.Id == userId);
            var newExercise = new Exercise
            {
                Title = makeExerciseVM.Title,
                Description = makeExerciseVM.Description,
                CreatedAt = DateTime.Now,
                MaxPoints = makeExerciseVM.MaxPoints,
                Creator = User,
                CreatorId = userId,
            };
            await context.Exercises.AddAsync(newExercise);
            await context.SaveChangesAsync();
            return newExercise.ExerciseId;
        }

        public async Task<List<Exercise>> GetUserExercises()
        {
            var userId = userService.GetUserId();
            var User = context.Users.FirstOrDefault(x => x.Id == userId);
            var exercises = await context.Exercises
                .Include(x => x.Creator)
                .Where(x => x.CreatorId == userId)
                .ToListAsync();
            return exercises;
        }

        public async Task AssignExerciseToUsers(int GroupId, int ExerciseId, AssignExerciseVM assignExerciseVM)
        {
            var assignments = await groupRepository.getMembers(GroupId);
            var exercise = await context.Exercises.FirstOrDefaultAsync(x => x.ExerciseId == ExerciseId);
            foreach (var item in assignments)
            {
                if (userService.CheckIfUserHasPermission("Wykonanie zadania", GroupId, item.UserId) == true)
                {
                    var newSolving = new Solving()
                    {
                        AssignmentId = item.AssignmentId,
                        Status = "Do wykonania",
                        CreatedAt = DateTime.Now,
                        ProgrammingLanguage = "Not now",
                        Exercise = exercise,
                        ExerciseId = ExerciseId,
                        DeadLine = DateTime.ParseExact(assignExerciseVM.DeadLineString, "dd-MM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture),
                        SentAt = DateTime.MinValue,
                        Configuration = assignExerciseVM.Configuration
                    };
                    await context.Solvings.AddAsync(newSolving);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<List<Solving>> GetUserSolvings()
        {
            var userId = userService.GetUserId();
            var User = context.Users.FirstOrDefault(x => x.Id == userId);
            var userAssignments = await context.Assignments
                .Where(x => x.UserId == userId).ToListAsync();
            List<Solving> solvings = new List<Solving>();
            foreach (var item in userAssignments)
            {
                solvings.AddRange(context.Solvings
                    .Include(x => x.Exercise)
                    .ThenInclude(x => x.Creator)
                    .Include(x => x.Checking)
                    .ThenInclude(x => x.Checker)
                    .Include(x => x.Assignment)
                    .ThenInclude(x => x.Group)
                    .Where(x => x.AssignmentId == item.AssignmentId && x.Status == "Do wykonania"));
            }
            return solvings;
        }

        public async Task<Solving> GetSolvingById(int SolvingId)
        {
            return await context.Solvings
                .Include(x => x.Checking)
                .ThenInclude(x => x.Checker)
                .Include(x => x.Exercise)
                .Include(x => x.Assignment)
                .ThenInclude(x => x.Group)
                .Include(x => x.Assignment)
                .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.SolvingId == SolvingId);
        }

        public async Task ReceiveSolvedExercise(SolvedExerciseVM solvedExerciseVM, int solvingId)
        {
            solvedExerciseVM.Answer = solvedExerciseVM.Answer.Replace("\r\n", "");
            var solving = await context.Solvings
                .Include(x => x.Exercise)
                .FirstOrDefaultAsync(x => x.SolvingId == solvingId);
            solving.Status = "Do sprawdzenia";
            solving.SentAt = DateTime.Now;
            solving.Answer = solvedExerciseVM.Answer;
            if (solving.Configuration)
            {
                AutomaticChecker automaticChecker =
                    new AutomaticChecker(solving.Exercise.Arguments, solving.Exercise.ValidCode, solvedExerciseVM.Answer);
                string result = await automaticChecker.run();
                solving.AutomaticCheckerOutcome = result;
            }
            await context.SaveChangesAsync();
        }

        public async Task<List<Solving>> ShowExercisesToCheck()
        {
            var userId = userService.GetUserId();
            var User = context.Users.FirstOrDefault(x => x.Id == userId);
            var solvings = await context.Assignments
                .Include(x => x.Group)
                .ThenInclude(x => x.Assignments)
                .Include(x => x.Role)
                .Where(x => x.UserId == userId)
                .Select(x => x.Group)
                .SelectMany(x => x.Assignments)
                .Where(x => x.Role.Name == "Uczeń" && x.UserId != userId)
                .Include(x => x.Solvings)
                .SelectMany(x => x.Solvings)
                .Where(x => x.Status == "Do sprawdzenia")
                .Include(x => x.Checking)
                .Include(x => x.Assignment)
                .ThenInclude(x => x.Group)
                .Include(x => x.Assignment)
                .ThenInclude(x => x.User)
                .Include(x => x.Exercise)
                .ToListAsync();

            var solvingsToReturn = new List<Solving>();
            foreach (var item in solvings)
            {
                if (userService.CheckIfUserHasPermission("Sprawdzanie zadania", item.Assignment.GroupId) == true)
                {
                    solvingsToReturn.Add(item);
                }
            }
            return solvingsToReturn;
        }

        public async Task<List<Group>> getGroupsWithPrivilege()
        {
            var groups = await groupRepository.GetUsersGroups();
            var groupsToReturn = new List<Group>();
            foreach (var item in groups)
            {
                if (userService.CheckIfUserHasPermission("Dodawanie zadania", item.GroupId))
                {
                    groupsToReturn.Add(item);
                }
            }
            return groupsToReturn;
        }

        public async Task ProcessCheckedExercise(CheckTheExerciseVM checkTheExerciseVM, int SolvingId)
        {
            var userId = userService.GetUserId();
            var User = context.Users.FirstOrDefault(x => x.Id == userId);
            var solving = await GetSolvingById(SolvingId);
            solving.Status = "Sprawdzone";
            var newChecking = new Checking()
            {
                Solving = solving,
                SolvingId = SolvingId,
                Points = checkTheExerciseVM.Points,
                Remarks = checkTheExerciseVM.Remarks,
                Checker = User,
                CheckerId = userId,
                CreatedAt = DateTime.Now
            };
            await context.Checkings.AddAsync(newChecking);
            await context.SaveChangesAsync();
        }

        public async Task<List<Solving>> ShowCheckedExercises()
        {
            var userId = userService.GetUserId();
            var User = context.Users.FirstOrDefault(x => x.Id == userId);
            var solvings = await context.Solvings
                .Include(x => x.Checking)
                .ThenInclude(x => x.Checker)
                .Include(x => x.Assignment)
                .ThenInclude(x => x.Group)
                .Include(x => x.Exercise)
                .Where(x => x.Status == "Sprawdzone" && x.Assignment.UserId == userId)
                .ToListAsync();
            return solvings;
        }

        public async Task<Exercise> GetExerciseById(int exerciseId)
        {
            var exercise = await context.Exercises.FirstOrDefaultAsync(x => x.ExerciseId == exerciseId);
            string result = exercise.Arguments;
            return exercise;
        }

        public async Task UpdateExercise(int exerciseId, MakeOrUpdateExerciseVM makeOrUpdateExerciseVM)
        {
            var exercise = await GetExerciseById(exerciseId);
            exercise.Title = makeOrUpdateExerciseVM.Title;
            exercise.Description = makeOrUpdateExerciseVM.Description;
            exercise.MaxPoints = makeOrUpdateExerciseVM.MaxPoints;
            await context.SaveChangesAsync();
        }

        public async Task DeleteExercise(int ExerciseId)
        {
            var exercise = await GetExerciseById(ExerciseId);
            if (exercise != null)
            {
                context.Exercises.Remove(exercise);
                await context.SaveChangesAsync();
            }
        }

        public async Task ConfigureExercise(ConfigureExerciseVM configureExerciseVM, int ExerciseId)
        {
            var exercise = await GetExerciseById(ExerciseId);
            exercise.ValidCode = configureExerciseVM.ValidCode;
            exercise.Arguments = configureExerciseVM.Arguments;
            await context.SaveChangesAsync();
        }

        public async Task<List<Solving>> ShowCheckedExercisesByUsers()
        {
            var userId = userService.GetUserId();
            var User = context.Users.FirstOrDefault(x => x.Id == userId);
            var solvings = await context
                .Solvings
                .Include(x => x.Checking)
                .Include(x => x.Exercise)
                .Include(x => x.Assignment)
                .ThenInclude(x => x.Group)
                .Where(x => x.Checking.Checker == User).ToListAsync();
            return solvings;
        }

        public int function()
        {
            return 0;
        }
    }
}