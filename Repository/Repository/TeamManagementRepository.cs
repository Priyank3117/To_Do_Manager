using Entities.Data;
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

        public List<TeamManagementViewModel> GetAllTeamsDetails(long userId)
        {
            var query = _db.TeamMembers.AsQueryable();
            var users = _db.Users.AsQueryable();

            var teams = query.Where(team => team.UserId == userId && ((team.Status == Entities.Models.TeamMembers.MemberStatus.Approved ) || (team.Status ==  Entities.Models.TeamMembers.MemberStatus.RequestedForLeave))).Select(team => new TeamManagementViewModel()
            {
                TeamId = team.TeamId,
                UserId = userId,
                TeamName = team.Teams.TeamName,
                TeamDescription = team.Teams.TeamDescription,
                Role = team.Role.ToString(),
                MemberStatus = team.Status.ToString(),
                TeamMembers = query.Where(teamMember => teamMember.TeamId == team.TeamId && (teamMember.Status == Entities.Models.TeamMembers.MemberStatus.Approved || teamMember.Status == Entities.Models.TeamMembers.MemberStatus.RequestedForLeave)).Select(user => new UserDetailOfTeam()
                {
                    UserId = user.UserId,
                    Avatar = user.Users.Avatar,
                    UserName = user.Users.FirstName + " " + user.Users.LastName,
                    ReportingPersonUserId = user.ReportinPersonUserId,
                    ReportingPersonUserName = user.ReportinPersonUserId != null ? users.FirstOrDefault(user1 => user1.UserId == user.ReportinPersonUserId)!.FirstName + " " + users.FirstOrDefault(user1 => user1.UserId == user.ReportinPersonUserId)!.LastName : null!,
                    ReportingPersonAvatar = users.FirstOrDefault(user1 => user1.UserId == user.ReportinPersonUserId)!.Avatar
                }).ToList(),
                JoinRequests = team.Role.ToString() == "TeamLeader" ? query.Where(teamMember => teamMember.TeamId == team.TeamId && teamMember.UserId != userId && teamMember.Status == Entities.Models.TeamMembers.MemberStatus.Pending).Select(user => new UserDetailOfTeam()
                {
                    UserId = user.UserId,
                    Avatar = user.Users.Avatar,
                    UserName = user.Users.FirstName + " " + user.Users.LastName,
                }).ToList() : new List<UserDetailOfTeam>(),
                LeaveRequests = team.Role.ToString() == "TeamLeader" ? query.Where(teamMember => teamMember.TeamId == team.TeamId && teamMember.UserId != userId && teamMember.Status == Entities.Models.TeamMembers.MemberStatus.RequestedForLeave).Select(user => new UserDetailOfTeam()
                {
                    UserId = user.UserId,
                    Avatar = user.Users.Avatar,
                    UserName = user.Users.FirstName + " " + user.Users.LastName,
                }).ToList() : new List<UserDetailOfTeam>()
            }).ToList();

            return teams;
        }

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

                var member = _db.TeamMembers.FirstOrDefault(teamMember => teamMember.UserId == userId && teamMember.TeamId == teamId);

                if (member != null)
                {
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

        public List<UserDetailOfTeam> GetAllMemberToSetReportingPerson(long userId, long teamId)
        {
            return _db.TeamMembers.Where(teamMember => teamMember.UserId != userId && teamMember.TeamId == teamId && teamMember.Status == Entities.Models.TeamMembers.MemberStatus.Approved).Select(user => new UserDetailOfTeam()
            {
                UserId = user.UserId,
                Avatar = user.Users.Avatar,
                UserName = user.Users.FirstName + " " + user.Users.LastName
            }).ToList();
        }

        public bool SetReportingPerson(long userIdOfTeamMember, long userIdOfReportingPerson, long teamId)
        {
            var query = _db.TeamMembers.Where(teamMember => teamMember.TeamId == teamId).AsQueryable();

            var teamMember = query.FirstOrDefault(teamMember => teamMember.UserId == userIdOfTeamMember);
            var reportingPerson = query.FirstOrDefault(teamMember => teamMember.UserId == userIdOfReportingPerson);

            if (teamMember != null && reportingPerson != null)
            {
                teamMember.ReportinPersonUserId = userIdOfReportingPerson;

                _db.Update(teamMember);

                reportingPerson.Role = Entities.Models.TeamMembers.Roles.ReportingPerson;

                _db.Update(reportingPerson);
                _db.SaveChanges();

                return true;
            }

            return false;
        }

        public bool RemoveReportingPerson(long teamMemberUserId, long reportingPersonUserId, long teamId)
        {
            if (teamMemberUserId != 0 && reportingPersonUserId != 0 && teamId != 0)
            {
                var query = _db.TeamMembers.Where(teamMember => teamMember.TeamId == teamId).AsQueryable();

                var teamMember = query.FirstOrDefault(teamMember => teamMember.UserId == teamMemberUserId && teamMember.ReportinPersonUserId == reportingPersonUserId);

                if (teamMember != null)
                {
                    teamMember.ReportinPersonUserId = null;

                    _db.Update(teamMember);
                    _db.SaveChanges();

                    if (!query.Any(teamMember => teamMember.ReportinPersonUserId == reportingPersonUserId))
                    {
                        var reportingPerson = query.FirstOrDefault(teamMember => teamMember.UserId == reportingPersonUserId);
                        if(reportingPerson != null)
                        {
                            reportingPerson.Role = Entities.Models.TeamMembers.Roles.TeamMember;

                            _db.Update(reportingPerson);
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
            return false;
        }

        public bool AcceptJoinRequest(long userId, long teamId)
        {
            if (userId != 0 && teamId != 0)
            {
                var teamMember = _db.TeamMembers.FirstOrDefault(teamMember => teamMember.UserId == userId && teamMember.TeamId == teamId);

                if (teamMember != null)
                {
                    teamMember.Status = Entities.Models.TeamMembers.MemberStatus.Approved;

                    _db.Update(teamMember);
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
                    teamMember.Status = Entities.Models.TeamMembers.MemberStatus.Approved;

                    _db.Update(teamMember);
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
    }
}
