namespace Entities.ViewModels.HomeViewModels
{
    public class TaskDetailViewModel
    {
        public long TeamId { get; set; } = 0;

        public long FromUserId { get; set; } = 0;

        public long UserId { get; set; } = 0;

        public long TaskId { get; set; } = 0;

        public string TaskName { get; set; } = string.Empty;

        public string TeamName { get; set; } = string.Empty;

        public string StartDateForDisplay { get; set; } = string.Empty;

        public string EndDateForDisplay { get; set; } = string.Empty;

        public string? TaskDescription { get; set; }

        public bool IsCompleted { get; set; } = false;

        public bool IsTodayTask { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
