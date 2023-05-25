using Entities.ViewModels.HomeViewModels;

namespace Repository.Interface
{
    public interface IHomeRepository
    {
        public long CreateTeam(TeamViewModel team);

        public bool AddUserToTeam(string email, long teamId);
    }
}
