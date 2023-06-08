using Entities.ViewModels.UserProfileViewModels;
using Repository.Interface;

namespace BAL
{
    public class UserProfileBAL
    {
        private readonly IUserProfileRepository _UserProfileRepo;

        public UserProfileBAL(IUserProfileRepository userProfileRepo)
        {
            _UserProfileRepo = userProfileRepo;
        }

        /// <summary>
        /// Get User Profile Details like firstname, lastname, gender and avatar
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Details of user</returns>
        public UserProfileViewModel GetUserProfileDetails(long userId)
        {
            return _UserProfileRepo.GetUserProfileDetails(userId);
        }

        /// <summary>
        /// Save User Profile's Data
        /// </summary>
        /// <param name="userDetails">User's Details like FirstName, LastName, Gender, Department and LinkedInURL</param>
        /// <returns>User Profile Page if any validation error then with error</returns>
        public bool SaveUserProfileDetails(UserProfileViewModel userDetails)
        {
            return _UserProfileRepo.SaveUserProfileDetails(userDetails);
        }

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="oldPassword">Old Password of User</param>
        /// <param name="newPassword">New Password of User</param>
        /// <returns>String with operation status</returns>
        public string ChangePassword(long userId, string newPassword, string oldPassword)
        {
            return _UserProfileRepo.ChangePassword(userId, newPassword, oldPassword);
        }

        /// <summary>
        /// Change User's Avatar
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="imageURL">Image file that selected by user</param>
        /// <returns>"Changed" if successfully changed</returns>
        public string ChangeImage(long userId, string imageURL)
        {
            return _UserProfileRepo.ChangeImage(userId, imageURL);
        }

        /// <summary>
        /// Get All Teams Name for Leave Team
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List of all team names</returns>
        public List<ListOfTeamsName> GetTeamNames(long userId)
        {
            return _UserProfileRepo.GetTeamNames(userId);
        }

        /// <summary>
        /// Leave From Team
        /// </summary>
        /// <param name="teamId">Team Id</param>
        /// <param name="userId">User Id</param>
        /// <returns>True - If successfully leaved from team else False</returns>
        public bool LeaveFromTeam(long teamId, long userId)
        {
            return _UserProfileRepo.LeaveFromTeam(teamId, userId);
        }

        /// <summary>
        /// Leave from All Team
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>True - If successfully leaved from all teams else False</returns>
        public bool LeaveFromAllTeam(long userId)
        {
            return _UserProfileRepo.LeaveFromAllTeam(userId);
        }
    }
}
