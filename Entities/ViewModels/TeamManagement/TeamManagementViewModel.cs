namespace Entities.ViewModels.TeamManagement
{
    public class TeamManagementViewModel
    {
        public long TeamId { get; set; } = 0;

        public long UserId { get; set; } = 0;

        public long TeamLeaderUserId { get; set; } = 0;

        public string TeamName { get; set; } = string.Empty;

        public string TeamDescription { get; set; } = string.Empty;

        public string MemberStatus { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public List<UserDetailOfTeam> TeamMembers { get; set; } = new List<UserDetailOfTeam>();

        public List<UserDetailOfTeam> JoinRequests { get; set; } = new List<UserDetailOfTeam>();

        public List<UserDetailOfTeam> LeaveRequests { get; set; } = new List<UserDetailOfTeam>();
    }
}
