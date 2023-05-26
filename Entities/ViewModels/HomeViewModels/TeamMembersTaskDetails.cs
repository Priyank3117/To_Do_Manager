namespace Entities.ViewModels.HomeViewModels
{
    public class TeamMembersTaskDetails
    {
        public long UserId { get; set; } = 0;

        public long TaskId { get; set; } = 0;

        public string TaskName { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false;

        public List<TaskDetailViewModel> TodayTasks { get; set; } = new List<TaskDetailViewModel>();
    }
}
