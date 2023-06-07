using Entities.ViewModels.TeamManagement;

namespace Repository.Interface
{
    public interface ITeamManagementRepository
    {
        public List<TeamManagementViewModel> GetAllTeamsDetails(long userId);

        public bool RemoveUserFromTeam(long userId, long teamId);

        public List<UserDetailOfTeam> GetAllMemberToSetReportingPerson(long userId, long teamId);

        public bool SetReportingPerson(long userIdOfTeamMember, long userIdOfReportingPerson, long teamId);

        public bool RemoveReportingPerson(long teamMemberUserId, long reportingPersonUserId, long teamId);

        public bool AcceptJoinRequest(long userId, long teamId);

        public bool DeclineLeaveRequest(long userId, long teamId);
    }
}
