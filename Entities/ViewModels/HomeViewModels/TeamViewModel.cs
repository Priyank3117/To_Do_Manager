﻿using Entities.ViewModels.AccountViewModels;
using System.ComponentModel.DataAnnotations;

namespace Entities.ViewModels.HomeViewModels
{
    public class TeamViewModel
    {
        public long TeamId { get; set; }

        public long TeamLeaderUserId { get; set; }

        [Required]
        public string TeamName { get; set; } = string.Empty;

        public string? MessageForMembers { get; set; }

        [Required]
        public string TeamDescription { get; set; } = string.Empty;

        [Required]
        public List<string> UserEmails { get; set; } = new List<string>();
    }
}
