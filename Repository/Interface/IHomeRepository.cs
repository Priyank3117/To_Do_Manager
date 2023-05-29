using Entities.ViewModels.HomeViewModels;

namespace Repository.Interface
{
    public interface IHomeRepository
    {
        public List<AllTeamsViewModel> GetAllTeams(string searchTerm, long userId);

        public TeamViewModel GetTeamDetails(long teamId);

        public long CreateTeam(TeamViewModel team);

        public bool AddUserToTeam(string email, long teamId);

        public bool RequestToJoinTeam(TeamMemberViewModel userRequest);

        public List<TodayTasksViewModel> GetAllTodayTasks(long userId);

        public List<ListOfUsers> GetDataForAddTask(long teamId, long userId);

        public bool AddTask(TaskDetailViewModel task);

        public bool MarkTaskAsCompleteOrUncomplete(TaskDetailViewModel task);
    }
}
