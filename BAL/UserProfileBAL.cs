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

        public UserProfileViewModel GetUserProfileDetails(long userId)
        {
            return _UserProfileRepo.GetUserProfileDetails(userId);
        }

        public bool SaveUserProfileDetails(UserProfileViewModel userDetails)
        {
            return _UserProfileRepo.SaveUserProfileDetails(userDetails);
        }

        public string ChangePassword(long userId, string newPassword, string oldPassword)
        {
            return _UserProfileRepo.ChangePassword(userId, newPassword, oldPassword);
        }

        public string ChangeImage(long userId, string imageURL)
        {
            return _UserProfileRepo.ChangeImage(userId, imageURL);
        }
    }
}
