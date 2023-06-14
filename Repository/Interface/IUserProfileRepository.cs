using Entities.ViewModels.UserProfileViewModels;

namespace Repository.Interface
{
    public interface IUserProfileRepository
    {
        public UserProfileViewModel GetUserProfileDetails(long userId);

        public UserProfileViewModel SaveUserProfileDetails(UserProfileViewModel userDetails);

        public string ChangePassword(long userId, string newPassword, string oldPassword);

        public string ChangeImage(long userId, string imageURL);

        public List<ListOfTeamsName> GetTeamNames(long userId);

        public bool LeaveFromTeam(long teamId, long userId);

        public bool LeaveFromAllTeam(long userId);

        public UserProfileViewModel GetUserAvatar(long UserId);
    }
}
