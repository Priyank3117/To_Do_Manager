using Entities.ViewModels.TeamManagement;

namespace Repository.Interface
{
    public interface ITeamManagementRepository
    {
        public List<TeamManagementViewModel> GetAllTeamsDetails(long userId);
    }
}
