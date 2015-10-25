using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VoluntinderDb;

namespace Voluntinder.Models
{
    public class MatchViewModel
    {
        public string PageTitle { get; set; }
        public List<PairingCards> Pairing { get; set; }
    }
}