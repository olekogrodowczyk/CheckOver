using CheckOver.Models;
using CheckOver.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckOver.Repository
{
    public interface IExerciseRepository
    {
        Task<int> AddExercise(MakeExerciseVM makeExerciseVM);

        Task AssignExerciseToUsers(int GroupId, int ExerciseId);

        int function();

        Task<Solving> GetSolvingById(int SolvingId);

        Task<List<Exercise>> GetUserExercises();

        Task<List<Solving>> GetUserSolvings();

        Task ProcessCheckedExercise(CheckTheExerciseVM checkTheExerciseVM, int SolvingId);

        Task ReceiveSolvedExercise(SolvedExerciseVM solvedExerciseVM, int solvingId);

        Task<List<Solving>> ShowCheckedExercises();

        Task<List<Solving>> ShowExercisesToCheck();
    }
}