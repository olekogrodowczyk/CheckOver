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

        Task<List<Exercise>> GetUserExercises();
    }
}