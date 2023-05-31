using Entities.Data;
using Entities.Models;
using Entities.ViewModels.UserProfileViewModels;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Repository.Repository
{
    public class UserProfileRepository: IUserProfileRepository
    {
        private readonly ToDoManagerDBContext _db;

        public UserProfileRepository(ToDoManagerDBContext db)
        {
            _db = db;
        }

        public UserProfileViewModel GetUserProfileDetails(long userId)
        {
            var user = _db.Users.Where(user => user.UserId == userId).Select(user => new UserProfileViewModel()
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Avatar = user.Avatar,
                Department = user.Deparment,
                Gender = user.Gender.ToString(),
                LinkedInURL = user.LinkedInURL
            }).FirstOrDefault();

            if(user != null)
            {
                return user;
            }
            else
            {
                return new UserProfileViewModel();
            }
        }

        public bool SaveUserProfileDetails(UserProfileViewModel userDetails)
        {
            if (userDetails != null)
            {
                var user = _db.Users.Where(user => user.UserId == userDetails.UserId).FirstOrDefault();

                if( user != null)
                {
                    user.FirstName = userDetails.FirstName;
                    user.LastName = userDetails.LastName;
                    user.LastName = userDetails.LastName;
                    user.Deparment = userDetails.Department;
                    user.Gender = userDetails.Gender == "Male" ? Users.UserGender.Male : Users.UserGender.Female;
                    user.LinkedInURL = userDetails.LinkedInURL;
                    user.UpdatedAt = DateTime.Now;

                    _db.Update(user);
                    _db.SaveChanges();

                    return true;
                }
            }
            return false;
        }

        public string ChangePassword(long userId, string newPassword, string oldPassword)
        {
            if(!BCrypt.Net.BCrypt.Verify(oldPassword, _db.Users.FirstOrDefault(p => p.UserId == userId)?.Password))
            {
                return "Enter Valid Old Password";
            }

            newPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _db.Users
                .Where(u => u.UserId == userId)
                .ExecuteUpdate(b =>
                    b.SetProperty(u => u.Password, newPassword).SetProperty(u => u.UpdatedAt, DateTime.Now));

            return "Changed";
        }

        public string ChangeImage(long userId, string imageURL)
        {
            _db.Users
                .Where(u => u.UserId == userId)
                .ExecuteUpdate(b => b.SetProperty(u => u.Avatar, imageURL).SetProperty(u => u.UpdatedAt, DateTime.Now));

            return "Changed";
        }

        public List<ListOfTeamsName> GetTeamNames(long userId)
        {
            if(userId != 0)
            {
                var x = _db.TeamMembers.Where(teamMember => teamMember.UserId == userId && teamMember.Role == TeamMembers.Roles.TeamMember).Select(teamMember => new ListOfTeamsName()
                {
                    TeamId = teamMember.Teams.TeamId,
                    TeamName = teamMember.Teams.TeamName,
                    UserId = userId,
                    LeaveStatus = teamMember.Status.ToString(),
                }).ToList();

                return x;
            }
            else
            {
                return new List<ListOfTeamsName>();
            }
        }

        public bool LeaveFromTeam(long teamId, long userId)
        {
            if(teamId != 0 && userId != 0)
            {
                var teamMember = _db.TeamMembers.Where(teamMember => teamMember.TeamId == teamId && teamMember.UserId == userId).FirstOrDefault();

                if(teamMember != null)
                {
                    teamMember.Status = TeamMembers.MemberStatus.RequestedForLeave;

                    _db.Update(teamMember);
                    _db.SaveChanges();

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

        public bool LeaveFromAllTeam(long userId)
        {
            if (userId != 0)
            {
                var teams = _db.TeamMembers.Where(teamMember => teamMember.UserId == userId);

                if (teams != null)
                {
                    foreach(var team in teams)
                    {
                        team.Status = TeamMembers.MemberStatus.RequestedForLeave;
                        _db.Update(team);
                    }
                    
                    _db.SaveChanges();

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
    }
}
