using Entities.ViewModels.AllTasksViewModel;
using Entities.ViewModels.HomeViewModels;

namespace Repository.Interface
{
    public interface IAllTasksRepository
    {
        public List<AllTasksOfAllTeams> GetAllTasks(Filter filter);

        public List<ListOfUsers> GetForAddTaskToToDo(long teamId, long userId);

        public bool AddTaskToTodayTask(TaskDetailViewModel taskDetail);

        public bool AddTaskToTodayTaskForTeamMember(TaskDetailViewModel taskDetail);

        public List<AllTaskForCalenderView> GetTasksForCalenderView(long userId);
    }
}
