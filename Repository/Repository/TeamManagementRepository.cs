using Entities.Data;
using Entities.Models;
using Entities.ViewModels.HomeViewModels;
using Entities.ViewModels.TeamManagement;
using Repository.Interface;

namespace Repository.Repository
{
    public class TeamManagementRepository : ITeamManagementRepository
    {
        private readonly ToDoManagerDBContext _db;

        public TeamManagementRepository(ToDoManagerDBContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Get All teams Details
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of team details like - Team Name, Team Members Details and thair Reporting Person, Join Requests and Leave Requests</returns>
        public List<TeamManagementViewModel> GetAllTeamsDetails(long userId)
        {
            var query = _db.TeamMembers.AsQueryable();
            var users = _db.Users.AsQueryable();

            var teams = query.Where(team => team.UserId == userId && ((team.Status == TeamMembers.MemberStatus.Approved) || (team.Status == TeamMembers.MemberStatus.RequestedForLeave))).Select(team => new TeamManagementViewModel()
            {
                TeamId = team.TeamId,
                UserId = userId,
                TeamName = team.Teams.TeamName,
                TeamDescription = team.Teams.TeamDescription,
                Role = team.Role.ToString(),
                MemberStatus = team.Status.ToString(),
                TeamLeaderUserId = team.Teams.TeamMembers.FirstOrDefault(teamMember => teamMember.Role == TeamMembers.Roles.TeamLeader && teamMember.TeamId == team.TeamId)!.UserId,
                TeamMembers = query.Where(teamMember => teamMember.TeamId == team.TeamId && (teamMember.Status == TeamMembers.MemberStatus.Approved || teamMember.Status == TeamMembers.MemberStatus.RequestedForLeave)).Select(user => new UserDetailOfTeam()
                {
                    UserId = user.UserId,
                    Avatar = user.Users.Avatar,
                    UserName = user.Users.FirstName + " " + user.Users.LastName,
                    ReportingPersonUserId = user.ReportinPersonUserId,
                    ReportingPersonUserName = user.ReportinPersonUserId != null ? users.FirstOrDefault(user1 => user1.UserId == user.ReportinPersonUserId)!.FirstName + " " + users.FirstOrDefault(user1 => user1.UserId == user.ReportinPersonUserId)!.LastName : null!,
                    ReportingPersonAvatar = users.FirstOrDefault(user1 => user1.UserId == user.ReportinPersonUserId)!.Avatar
                }).ToList(),
                JoinRequests = team.Role.ToString() == "TeamLeader" ? query.Where(teamMember => teamMember.TeamId == team.TeamId && teamMember.UserId != userId && teamMember.Status == TeamMembers.MemberStatus.Pending).Select(user => new UserDetailOfTeam()
                {
                    UserId = user.UserId,
                    Avatar = user.Users.Avatar,
                    UserName = user.Users.FirstName + " " + user.Users.LastName,
                    JoinRequestMessage = user.JoinRequestMessage
                }).ToList() : new List<UserDetailOfTeam>(),
                LeaveRequests = team.Role.ToString() == "TeamLeader" ? query.Where(teamMember => teamMember.TeamId == team.TeamId && teamMember.UserId != userId && teamMember.Status == TeamMembers.MemberStatus.RequestedForLeave).Select(user => new UserDetailOfTeam()
                {
                    UserId = user.UserId,
                    Avatar = user.Users.Avatar,
                    UserName = user.Users.FirstName + " " + user.Users.LastName,
                }).ToList() : new List<UserDetailOfTeam>()
            }).ToList();

            return teams;
        }

        /// <summary>
        /// Remove User from Team
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="teamId">Team Id</param>
        /// <returns>True - If succesfully removed alse False</returns>
        public bool RemoveUserFromTeam(long userId, long teamId)
        {
            if (userId != 0 && teamId != 0)
            {
                var usersTask = _db.Tasks.Where(tasks => tasks.UserId == userId && tasks.TeamId == teamId);

                if (usersTask.Any())
                {
                    foreach (var user in usersTask)
                    {
                        _db.Remove(user);
                    }
                    _db.SaveChanges();
                }

                var teamMemberQuery = _db.TeamMembers.AsQueryable();
                var member = teamMemberQuery.FirstOrDefault(teamMember => teamMember.UserId == userId && teamMember.TeamId == teamId);

                if (member != null)
                {
                    if(member.ReportinPersonUserId != null && teamMemberQuery.Count(teamMember => teamMember.TeamId == teamId && teamMember.ReportinPersonUserId == member.ReportinPersonUserId) == 1)
                    {
                        var reportingPerson = teamMemberQuery.FirstOrDefault(teamMember => teamMember.UserId == member.ReportinPersonUserId && teamMember.TeamId == teamId);

                        if(reportingPerson != null)
                        {
                            reportingPerson.Role = TeamMembers.Roles.TeamMember;

                            _db.SaveChanges();
                        }
                    }

                    if(member.Role == TeamMembers.Roles.ReportingPerson)
                    {
                        var allMembersUnderRP = teamMemberQuery.Where(teamMember => teamMember.TeamId == teamId && teamMember.ReportinPersonUserId == userId);

                        foreach(var memberunderRP in allMembersUnderRP)
                        {
                            memberunderRP.ReportinPersonUserId = null;

                            _db.SaveChanges();
                        }
                    }

                    _db.Remove(member);
                    _db.SaveChanges();
                }

                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Get Al lMember To Set Reporting Person
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="teamId">Team Id</param>
        /// <returns>List of User's details</returns>
        public List<UserDetailOfTeam> GetAllMemberToSetReportingPerson(long userId, long teamId)
        {
            var query = _db.TeamMembers.AsQueryable();

            var allMembers = query.Where(teamMember => teamMember.UserId != userId && teamMember.TeamId == teamId && teamMember.Status == TeamMembers.MemberStatus.Approved && teamMember.Role != TeamMembers.Roles.TeamLeader && teamMember.ReportinPersonUserId == null).Select(user => new UserDetailOfTeam()
            {
                UserId = user.UserId,
                Avatar = user.Users.Avatar,
                UserName = user.Users.FirstName + " " + user.Users.LastName
            }).ToList();

            List<UserDetailOfTeam> allTeamMembers = new();

            foreach (var teamMember in allMembers)
            {
                if (!query.Any(member => member.ReportinPersonUserId == userId && member.TeamId == teamId))
                {
                    allTeamMembers.Add(teamMember);
                }
            }

            return allTeamMembers;
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
            var query = _db.TeamMembers.Where(teamMember => teamMember.TeamId == teamId).AsQueryable();

            var teamMember = query.FirstOrDefault(teamMember => teamMember.UserId == userIdOfTeamMember);
            var reportingPerson = query.FirstOrDefault(teamMember => teamMember.UserId == userIdOfReportingPerson);

            if (teamMember != null && reportingPerson != null)
            {
                teamMember.ReportinPersonUserId = userIdOfReportingPerson;

                reportingPerson.Role = TeamMembers.Roles.ReportingPerson;

                _db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
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
            if (teamMemberUserId != 0 && reportingPersonUserId != 0 && teamId != 0)
            {
                var query = _db.TeamMembers.Where(teamMember => teamMember.TeamId == teamId).AsQueryable();

                var teamMember = query.FirstOrDefault(teamMember => teamMember.UserId == teamMemberUserId && teamMember.ReportinPersonUserId == reportingPersonUserId);

                if (teamMember != null)
                {
                    teamMember.ReportinPersonUserId = null;

                    _db.SaveChanges();

                    if (!query.Any(teamMember => teamMember.ReportinPersonUserId == reportingPersonUserId))
                    {
                        var reportingPerson = query.FirstOrDefault(teamMember => teamMember.UserId == reportingPersonUserId);
                        if (reportingPerson != null)
                        {
                            reportingPerson.Role = TeamMembers.Roles.TeamMember;

                            _db.SaveChanges();
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }            
        }

        /// <summary>
        /// Accept Join Request in Team
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="teamId">Team Id</param>
        /// <returns>True - If successfully accept the request</returns>
        public bool AcceptJoinRequest(long userId, long teamId)
        {
            if (userId != 0 && teamId != 0)
            {
                var teamMember = _db.TeamMembers.FirstOrDefault(teamMember => teamMember.UserId == userId && teamMember.TeamId == teamId);

                if (teamMember != null)
                {
                    teamMember.Status = TeamMembers.MemberStatus.Approved;

                    _db.SaveChanges();

                    return true;
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Decline Leave Request
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="teamId">Team Id of User</param>
        /// <returns>True - If successfuly declined join request else Fasle</returns>
        public bool DeclineLeaveRequest(long userId, long teamId)
        {
            if (userId != 0 && teamId != 0)
            {
                var teamMember = _db.TeamMembers.FirstOrDefault(teamMember => teamMember.UserId == userId && teamMember.TeamId == teamId);

                if (teamMember != null)
                {
                    teamMember.Status = TeamMembers.MemberStatus.Approved;

                    _db.SaveChanges();

                    return true;
                }

                return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Delete Team
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <returns>True - If successfully team deleted else False</returns>
        public bool DeleteTeam(long teamId)
        {
            if (teamId != 0)
            {
                var teamMember = _db.TeamMembers.FirstOrDefault(teamMember => teamMember.TeamId == teamId && teamMember.Role == TeamMembers.Roles.TeamLeader);
                if (teamMember != null)
                    _db.Remove(teamMember);

                var team = _db.Teams.FirstOrDefault(team => team.TeamId == teamId);
                if (team != null)
                    _db.Remove(team);

                _db.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get All Users Email Of Team
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <returns>List of String(Emails)</returns>
        public List<string> GetAllUsersEmailOfTeam(long teamId)
        {
            return _db.TeamMembers.Where(teamMember => teamMember.TeamId == teamId && (teamMember.Status == TeamMembers.MemberStatus.Approved || teamMember.Status == TeamMembers.MemberStatus.RequestedForLeave)).Select(teamMember => teamMember.Users.Email).ToList();
        }
    }
}
