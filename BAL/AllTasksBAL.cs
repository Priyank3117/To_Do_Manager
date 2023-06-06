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

        public List<AllTasksOfAllTeams> GetAllTasks(Filter filter)
        {
            return _AllTasksRepo.GetAllTasks(filter);
        }

        public bool AddTaskToTodayTask(TaskDetailViewModel taskDetail)
        {
            return _AllTasksRepo.AddTaskToTodayTask(taskDetail);
        }

        public List<AllTaskForCalenderView> GetTasksForCalenderView(long userId)
        {
            return _AllTasksRepo.GetTasksForCalenderView(userId);
        }
    }
}
