namespace Entities.ViewModels.HomeViewModels
{
    public class AllTodayTasksForListView
    {
        public long TaskId { get; set; } = 0;

        public long TeamId { get; set; } = 0;

        public long UserId { get; set; } = 0;

        public string TeamName { get; set; } = string.Empty;

        public string Avatar { get; set; } = "/images/EmptyProfile.png";

        public string UserName { get; set; } = string.Empty;

        public string TaskName { get; set; } = string.Empty;

        public string Deadline { get; set; } = string.Empty;

        public bool TaskStatus { get; set; } = false;

        public bool IsMyTask { get; set; } = false;
    }
}
