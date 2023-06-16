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

        /// <summary>
        /// Get User Profile Details like firstname, lastname, gender and avatar
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Details of user</returns>
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

        /// <summary>
        /// Save User Profile's Data
        /// </summary>
        /// <param name="userDetails">User's Details like FirstName, LastName, Gender, Department and LinkedInURL</param>
        /// <returns>User Profile Page if any validation error then with error</returns>
        public UserProfileViewModel SaveUserProfileDetails(UserProfileViewModel userDetails)
        {
            if (userDetails != null)
            {
                var user = _db.Users.Where(user => user.UserId == userDetails.UserId).FirstOrDefault();

                if( user != null)
                {
                    user.FirstName = userDetails.FirstName;
                    user.LastName = userDetails.LastName;
                    user.LastName = userDetails.LastName;
                    user.Deparment = userDetails.Department == null ? userDetails.Department : userDetails.Department.Trim();
                    user.Gender = userDetails.Gender == "Male" ? Users.UserGender.Male : Users.UserGender.Female;
                    user.LinkedInURL = userDetails.LinkedInURL;
                    user.UpdatedAt = DateTime.Now;

                    _db.Update(user);
                    _db.SaveChanges();

                    return new UserProfileViewModel
                    {
                        Avatar = user.Avatar,
                        FirstName = userDetails.FirstName,
                        LastName = userDetails.LastName
                    };
                }
            }
            return new UserProfileViewModel();
        }

        public UserProfileViewModel GetUserAvatar(long UserId)
        {
            var user = _db.Users.FirstOrDefault(user => user.UserId == UserId);
            UserProfileViewModel userInfo = new();

            if (user != null)
            {
                userInfo.Avatar = user.Avatar;
                userInfo.FirstName = user.FirstName;
                userInfo.LastName = user.LastName;
            }         

            return userInfo;
        }

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="oldPassword">Old Password of User</param>
        /// <param name="newPassword">New Password of User</param>
        /// <returns>String with operation status</returns>
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

        /// <summary>
        /// Change User's Avatar
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="imageURL">Image file that selected by user</param>
        /// <returns>"Changed" if successfully changed</returns>
        public string ChangeImage(long userId, string imageURL)
        {
            _db.Users
                .Where(u => u.UserId == userId)
                .ExecuteUpdate(b => b.SetProperty(u => u.Avatar, imageURL).SetProperty(u => u.UpdatedAt, DateTime.Now));

            return "Changed";
        }

        /// <summary>
        /// Get All Teams Name for Leave Team
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of all team names</returns>
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

        /// <summary>
        /// Leave From Team
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <param name="userId">User Id</param>
        /// <returns>True - If successfully leaved from team else False</returns>
        public bool LeaveFromTeam(long teamId, long userId)
        {
            if(teamId != 0 && userId != 0)
            {
                var teamMember = _db.TeamMembers.Where(teamMember => teamMember.TeamId == teamId && teamMember.UserId == userId).FirstOrDefault();

                if(teamMember != null)
                {
                    teamMember.Status = TeamMembers.MemberStatus.RequestedForLeave;

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

        /// <summary>
        /// Leave from All Team
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>True - If successfully leaved from all teams else False</returns>
        public bool LeaveFromAllTeam(long userId)
        {
            if (userId != 0)
            {
                var teams = _db.TeamMembers.Where(teamMember => teamMember.UserId == userId && teamMember.Role != TeamMembers.Roles.TeamLeader);

                if (teams != null)
                {
                    foreach(var team in teams)
                    {
                        team.Status = TeamMembers.MemberStatus.RequestedForLeave;
                        _db.SaveChanges();
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
    }
}
