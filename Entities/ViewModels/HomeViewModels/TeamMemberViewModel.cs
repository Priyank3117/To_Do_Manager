namespace Entities.ViewModels.HomeViewModels
{
    public class TeamMemberViewModel
    {
        public long TeamMemberId { get; set; }

        public long TeamId { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
