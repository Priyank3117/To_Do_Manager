using Entities.ViewModels.AllTasksViewModel;
using Entities.ViewModels.HomeViewModels;

namespace Repository.Interface
{
    public interface IAllTasksRepository
    {
        public List<AllTasksOfAllTeams> GetAllTasks(long userId, string searchTerm);

        public bool AddTaskToTodayTask(TaskDetailViewModel taskDetail);

        public List<AllTaskForCalenderView> GetTasksForCalenderView(long userId);
    }
}
