namespace Entities.ViewModels.AllTasksViewModel
{
    public class AllTaskForCalenderView
    {
        public string TaskName { get; set; } = string.Empty;

        public string StartDate { get; set; } = string.Empty;

        public string EndDate { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false;
    }
}
