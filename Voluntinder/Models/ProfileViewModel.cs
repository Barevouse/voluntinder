using System.Collections.Generic;
using Tweetinvi.Core.Interfaces;
using VoluntinderDb;

namespace Voluntinder.Models
{
    public class ProfileViewModel
    {
        public ProfileViewModel()
        {
            Skills = new List<Skill>();
        }
        public string UserId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public List<Skill> Skills { get; set; }
        public string ImageUrl { get; set; }
        public string UserName { get; set; }
        public IEnumerable<ITweet> Tweets { get; set; }
        public string Location { get; set; }
        public string Distance { get; set; }
    }
}