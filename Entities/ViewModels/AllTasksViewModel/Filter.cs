namespace Entities.ViewModels.AllTasksViewModel
{
    public class Filter
    {
        public long UserId { get; set; }

        public string? TeamName { get; set; }

        public string? TaskName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? TaskStatus { get; set; }
    }
}
