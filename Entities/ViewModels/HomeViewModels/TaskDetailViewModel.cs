namespace Entities.ViewModels.HomeViewModels
{
    public class TaskDetailViewModel
    {
        public long TaskId { get; set; } = 0;

        public string TaskName { get; set; } = string.Empty;

        public bool IsCompleted { get; set; } = false;
    }
}
