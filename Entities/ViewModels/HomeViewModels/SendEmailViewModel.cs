namespace Entities.ViewModels.HomeViewModels
{
    public class SendEmailViewModel
    {
        public string ToEmail { get; set; } = string.Empty;

        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;
    }
}
