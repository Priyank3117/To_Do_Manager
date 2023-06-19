namespace Entities.ViewModels.AllTasksViewModel
{
    public class AllTaskForCalenderView
    {
        public string TaskName { get; set; } = string.Empty;

        public string StartDate { get; set; } = string.Empty;

        public string EndDate { get; set; } = string.Empty;

        public string? TaskDescription { get; set; }

        public bool IsCompleted { get; set; } = false;

        public long TaskId { get; set; } = 0;
    }
}
