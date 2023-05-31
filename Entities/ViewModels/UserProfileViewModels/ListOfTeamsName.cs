namespace Entities.ViewModels.UserProfileViewModels
{
    public class ListOfTeamsName
    {
        public long TeamId { get; set; } = 0;

        public long UserId { get; set; } = 0;

        public string TeamName { get; set; } = string.Empty;

        public string LeaveStatus { get; set; } = string.Empty;
    }
}
