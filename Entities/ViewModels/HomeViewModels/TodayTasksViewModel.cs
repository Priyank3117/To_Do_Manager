namespace Entities.ViewModels.HomeViewModels
{
    public class TodayTasksViewModel
    {
        public long TeamId { get; set; } = 0;

        public long UserId  { get; set; } = 0;

        public string UserName { get; set; } = string.Empty;

        public string TeamName { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public List<TaskDetailViewModel> TodayTasks { get; set; }  = new List<TaskDetailViewModel>();

        public List<TeamMembersTaskDetails> TeamMembersTaasks { get; set; }  = new List<TeamMembersTaskDetails>();
    }
}
