using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Voluntinder.Models
{
    public class SkillsWithUser
    {
        public string SkillName { get; set; }
        public long? SkillId { get; set; }
        public bool HasSkill { get; set; }
    }
}