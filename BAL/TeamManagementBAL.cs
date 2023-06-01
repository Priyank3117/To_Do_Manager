using Entities.ViewModels.TeamManagement;
using Repository.Interface;

namespace BAL
{
    public class TeamManagementBAL
    {
        private readonly ITeamManagementRepository _TeamManagementRepo;

        public TeamManagementBAL(ITeamManagementRepository teamManagementRepo)
        {
            _TeamManagementRepo = teamManagementRepo;
        }

        public List<TeamManagementViewModel> GetAllTeamsDetails(long userId)
        {
            return _TeamManagementRepo.GetAllTeamsDetails(userId);
        }
    }
}
