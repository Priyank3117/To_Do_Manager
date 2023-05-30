using Entities.ViewModels.HomeViewModels;

namespace Entities.ViewModels.AllTasksViewModel
{
    public class AllTasksOfAllTeams
    {
        public string Role { get; set; } = string.Empty;

        public long TeamId { get; set; } = 0;

        public long UserId { get; set; } = 0;

        public string TeamName { get; set; } = string.Empty;

        public List<TaskDetailViewModel> MyTasks { get; set; } = new List<TaskDetailViewModel>();

        public List<TeamMembersTaskDetails> TeamMembersTaasks { get; set; } = new List<TeamMembersTaskDetails>();
    }
}
