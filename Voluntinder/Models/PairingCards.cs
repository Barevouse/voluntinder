using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VoluntinderDb;

namespace Voluntinder.Models
{
    public class PairingCards
    {
        public string UserId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public List<Skill> Skills { get; set; }
        public string ImageUrl { get; set; }
    }
}