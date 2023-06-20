namespace Entities.ViewModels.TeamManagement
{
    public class UserDetailOfTeam
    {
        public long UserId { get; set; } = 0;

        public string UserName { get; set; } = string.Empty;

        public string Avatar { get; set; } = string.Empty;

        public long? ReportingPersonUserId { get; set; } = 0;

        public string ReportingPersonUserName { get; set; } = string.Empty;

        public string ReportingPersonAvatar { get; set; } = string.Empty;

        public string MemberStatus { get; set; } = string.Empty;

        public string? JoinRequestMessage { get; set; }
    }
}
