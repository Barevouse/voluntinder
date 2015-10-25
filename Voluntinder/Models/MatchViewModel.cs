using System.Collections.Generic;

namespace Voluntinder.Models
{
    public class MatchViewModel
    {
        public string PageTitle { get; set; }
        public string UserLocation { get; set; }
        public List<ProfileViewModel> Profile { get; set; }
        public string Distance { get; set; }
    }
}