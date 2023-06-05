namespace Entities.ViewModels.TeamManagement
{
    public class AddUsersInTeam
    {
        public long TeamId { get; set; } = 0;

        public string TeamName { get; set; } = string.Empty;

        public List<string> UserEmails { get; set; } = new List<string>();
    }
}
