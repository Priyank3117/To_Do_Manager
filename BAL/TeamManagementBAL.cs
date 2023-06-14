using Entities;
using Entities.ViewModels.HomeViewModels;
using Entities.ViewModels.TeamManagement;
using Repository.Interface;

namespace BAL
{
    public class TeamManagementBAL
    {
        private readonly ITeamManagementRepository _TeamManagementRepo;
        private readonly IHomeRepository _HomeRepo;
        private readonly MailHelper _MailHelper;

        public TeamManagementBAL(ITeamManagementRepository teamManagementRepo, IHomeRepository homeRepo, MailHelper mail)
        {
            _TeamManagementRepo = teamManagementRepo;
            _HomeRepo = homeRepo;
            _MailHelper = mail;
        }

        /// <summary>
        /// Get All teams Details
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of team details like - Team Name, Team Members Details and thair Reporting Person, Join Requests and Leave Requests</returns>
        public List<TeamManagementViewModel> GetAllTeamsDetails(long userId)
        {
            return _TeamManagementRepo.GetAllTeamsDetails(userId);
        }

        /// <summary>
        /// Add User In Team
        /// </summary>
        /// <param name="addUsers">Email of user</param>
        /// <returns>True - If successfully Added alse False</returns>
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

                        _MailHelper.SendEmail(sendEmailViewModel);
                    }
                    else
                    {
                        SendEmailViewModel sendEmailViewModel = new()
                        {
                            Body = "You have an invitation to join <b>" + addUsers.TeamName + "</b> Team click below link to see https://localhost:7100",
                            Subject = "Invitation to join a Team",
                            ToEmail = userEmail
                        };

                        _MailHelper.SendEmail(sendEmailViewModel);
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Remove User from Team
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="teamId">Team Id</param>
        /// <returns>True - If succesfully removed alse False</returns>
        public bool RemoveUserFromTeam(long userId, long teamId)
        {
            return _TeamManagementRepo.RemoveUserFromTeam(userId, teamId);
        }

        /// <summary>
        /// Get Al lMember To Set Reporting Person
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="teamId">Team Id</param>
        /// <returns>List of User's details</returns>
        public List<UserDetailOfTeam> GetAllMemberToSetReportingPerson(long userId, long teamId)
        {
            return _TeamManagementRepo.GetAllMemberToSetReportingPerson(userId, teamId);
        }

        /// <summary>
        /// Set Reporting Person
        /// </summary>
        /// <param name="userIdOfTeamMember">User Id of Team Member</param>
        /// <param name="userIdOfReportingPerson">User Id of Reporting Person</param>
        /// <param name="teamId">Team Id</param>
        /// <returns>True - If successfully set alse False</returns>
        public bool SetReportingPerson(long userIdOfTeamMember, long userIdOfReportingPerson, long teamId)
        {
            return _TeamManagementRepo.SetReportingPerson(userIdOfTeamMember, userIdOfReportingPerson, teamId);
        }

        /// <summary>
        /// Remove Reporting Person
        /// </summary>
        /// <param name="teamMemberUserId">User Id of Team Member</param>
        /// <param name="reportingPersonUserId">User Id of Reporting Person</param>
        /// <param name="teamId">Team Id</param>
        /// <returns>True - If successfully removed alse False</returns>
        public bool RemoveReportingPerson(long teamMemberUserId, long reportingPersonUserId, long teamId)
        {
            return _TeamManagementRepo.RemoveReportingPerson(teamMemberUserId, reportingPersonUserId, teamId);
        }

        /// <summary>
        /// Accept Join Request in Team
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="teamId">Team Id</param>
        /// <returns>True - If successfully accept the request</returns>
        public bool AcceptJoinRequest(long userId, long teamId)
        {
            return _TeamManagementRepo.AcceptJoinRequest(userId, teamId);
        }

        /// <summary>
        /// Decline Leave Request
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="teamId">Team Id of User</param>
        /// <returns>True - If successfuly declined join request else Fasle</returns>
        public bool DeclineLeaveRequest(long userId, long teamId)
        {
            return _TeamManagementRepo.DeclineLeaveRequest(userId, teamId);
        }

        /// <summary>
        /// Delete Team
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <returns>True - If successfully team deleted else False</returns>
        public bool DeleteTeam(long teamId)
        {
            return _TeamManagementRepo.DeleteTeam(teamId);
        }
    }
}
