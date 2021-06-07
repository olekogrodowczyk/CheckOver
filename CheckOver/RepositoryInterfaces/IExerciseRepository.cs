using CheckOver.Models;
using CheckOver.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CheckOver.Repository
{
    public interface IExerciseRepository
    {
        Task<int> AddExercise(MakeOrUpdateExerciseVM makeExerciseVM);

        Task AssignExerciseToUsers(int GroupId, int ExerciseId, AssignExerciseVM assignExerciseVM);

        Task ConfigureExercise(ConfigureExerciseVM configureExerciseVM, int ExerciseId);

        Task DeleteExercise(int ExerciseId);

        int function();

        Task<Exercise> GetExerciseById(int ExerciseId);

        Task<List<Group>> getGroupsWithPrivilege();

        Task<Solving> GetSolvingById(int SolvingId);

        Task<List<Exercise>> GetUserExercises();

        Task<List<Solving>> GetUserSolvings();

        Task ProcessCheckedExercise(CheckTheExerciseVM checkTheExerciseVM, int SolvingId);

        Task ReceiveSolvedExercise(SolvedExerciseVM solvedExerciseVM, int solvingId);

        Task<List<Solving>> ShowCheckedExercises();

        Task<List<Solving>> ShowCheckedExercisesByUsers();

        Task<List<Solving>> ShowExercisesToCheck();

        Task UpdateExercise(int exerciseId, MakeOrUpdateExerciseVM makeOrUpdateExerciseVM);
    }
}