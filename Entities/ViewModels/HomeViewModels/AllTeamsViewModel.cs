using Entities.ViewModels.DocumentViewModels;

namespace Entities.ViewModels.HomeViewModels
{
    public class AllTeamsViewModel
    {
        public long TeamId { get; set; } = 0;

        public string TeamName { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public List<DocumentViewModel> DocumentViewModels { get; set; }
    }
}
