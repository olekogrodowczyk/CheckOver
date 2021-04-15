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

        public async Task<int> AddExercise(MakeExerciseVM makeExerciseVM)
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
                DeadLine = DateTime.ParseExact(makeExerciseVM.DeadLineString, "dd-MM-yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture)
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
                .Include(x => x.Creator).Where(x => x.CreatorId == userId)
                .ToListAsync();
            return exercises;
        }

        public async Task AssignExerciseToUsers(int GroupId, int ExerciseId)
        {
            var assignments = await groupRepository.getMembers(GroupId);
            var exercise = await context.Exercises.FirstOrDefaultAsync(x => x.ExerciseId == ExerciseId);
            foreach (var item in assignments)
            {
                var newSolving = new Solving()
                {
                    AssignmentId = item.AssignmentId,
                    Status = "Do wykonania",
                    CreatedAt = DateTime.Now,
                    ProgrammingLanguage = "Not now",
                    Exercise = exercise,
                    ExerciseId = ExerciseId
                };
                await context.Solvings.AddAsync(newSolving);
                await context.SaveChangesAsync();
            }
        }

        public int function()
        {
            return 0;
        }
    }
}