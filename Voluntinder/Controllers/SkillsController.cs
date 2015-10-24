using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConsoleApplication1;

namespace Voluntinder.Controllers
{
    public class SkillsController : Controller
    {
        public voluntinder_dbEntities Dbcontext;
        public ActionResult Index()
        {
            return View();
        }

        public void SaveSkills(string userId, List<string> skills)
        {
            foreach (var skill in skills)
            {
                Dbcontext.skills_list.Add(new skills_list {UserId = userId, Skills = skill});
            }
            Dbcontext.SaveChanges();
        }

        public List<String> GetSkills(string userId)
        {
            var skills = Dbcontext.skills_list.Where(x => x.UserId == userId).Select(y=>y.Skills).ToList();
            return skills;
        }
    }
}