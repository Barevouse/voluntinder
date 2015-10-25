using System;

namespace Voluntinder.Models
{
    public class MatchesShortProfile
    {
        public string Name { get; set; }
        public DateTime MatchedOn { get; set; }
        public string ProfileImage { get; set; }
        public string ProfileLink { get; set; }
        public string userId { get; set; }
        public string Tweet { get; set; }
        public double DistanceFrom { get; set; }
    }
}