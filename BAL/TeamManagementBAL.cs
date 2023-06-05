using Entities.ViewModels.HomeViewModels;
using Entities.ViewModels.TeamManagement;
using Repository.Interface;

namespace BAL
{
    public class TeamManagementBAL
    {
        private readonly ITeamManagementRepository _TeamManagementRepo;
        private readonly IHomeRepository _HomeRepo;
        private readonly HomeBAL _HomeBAL;

        public TeamManagementBAL(ITeamManagementRepository teamManagementRepo, IHomeRepository homeRepo, HomeBAL homeBAL)
        {
            _TeamManagementRepo = teamManagementRepo;
            _HomeRepo = homeRepo;
            _HomeBAL = homeBAL;
        }

        public List<TeamManagementViewModel> GetAllTeamsDetails(long userId)
        {
            return _TeamManagementRepo.GetAllTeamsDetails(userId);
        }

        public bool AddUser(AddUsersInTeam addUsers)
        {
            if(addUsers.TeamId != 0)
            {
                foreach (var userEmail in addUsers.UserEmails)
                {
                    if (!_HomeRepo.AddUserToTeam(userEmail, addUsers.TeamId))
                    {
                        SendEmailViewModel sendEmailViewModel = new()
                        {
                            Body = "You have an invitation to join <b>" + addUsers.TeamName + "</b> Team click below link to see https://localhost:7100/Account/Registration",
                            Subject = "Invitation to join a Team",
                            ToEmail = userEmail
                        };
                        _HomeBAL.InviteUser(sendEmailViewModel);
                    }
                    else
                    {
                        SendEmailViewModel sendEmailViewModel = new()
                        {
                            Body = "You have an invitation to join <b>" + addUsers.TeamName + "</b> Team click below link to see https://localhost:7100",
                            Subject = "Invitation to join a Team",
                            ToEmail = userEmail
                        };
                        _HomeBAL.InviteUser(sendEmailViewModel);
                    }
                }

                return true;
            }

            return false;
        }

        public bool RemoveUserFromTeam(long userId, long teamId)
        {
            return _TeamManagementRepo.RemoveUserFromTeam(userId, teamId);
        }

        public List<UserDetailOfTeam> GetAllMemberToSetReportingPerson(long userId, long teamId)
        {
            return _TeamManagementRepo.GetAllMemberToSetReportingPerson(userId, teamId);
        }

        public bool SetReportingPerson(long userIdOfTeamMember, long userIdOfReportingPerson, long teamId)
        {
            return _TeamManagementRepo.SetReportingPerson(userIdOfTeamMember, userIdOfReportingPerson, teamId);
        }

        public bool RemoveReportingPerson(long teamMemberUserId, long reportingPersonUserId, long teamId)
        {
            return _TeamManagementRepo.RemoveReportingPerson(teamMemberUserId, reportingPersonUserId, teamId);
        }

        public bool AcceptJoinRequest(long userId, long teamId)
        {
            return _TeamManagementRepo.AcceptJoinRequest(userId, teamId);
        }
    }
}
