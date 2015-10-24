using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Voluntinder.Controllers
{
    public class SkillsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public void SaveSkills(string userId, List<string> skills)
        {
            foreach (var skill in skills)
            {
                
            }
        }
    }
}