using Entities.ViewModels.AllTasksViewModel;
using Entities.ViewModels.HomeViewModels;
using Repository.Interface;

namespace BAL
{
    public class AllTasksBAL
    {
        private readonly IAllTasksRepository _AllTasksRepo;

        public AllTasksBAL(IAllTasksRepository allTasksRepo)
        {
            _AllTasksRepo = allTasksRepo;
        }

        public List<AllTasksOfAllTeams> GetAllTasks(long userId, string searchTerm)
        {
            return _AllTasksRepo.GetAllTasks(userId, searchTerm);
        }

        public bool AddTaskToTodayTask(TaskDetailViewModel taskDetail)
        {
            return _AllTasksRepo.AddTaskToTodayTask(taskDetail);
        }
    }
}
