using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ConsoleApplication1;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Voluntinder.Models;
using VoluntinderDb;

namespace Voluntinder.Controllers
{
    [Authorize]
    public class MatchController : Controller
    {
        protected voluntinder_dbEntities DbContext;

        public MatchController()
        {
            DbContext = new voluntinder_dbEntities();
        }
        // GET: Match
        public ActionResult Index()
        {
            var model = new MatchViewModel();
            var userId = User.Identity.GetUserId();
            var user = DbContext.AspNetUsers.FirstOrDefault(x => x.Id == userId);

            if (string.IsNullOrEmpty(user.Name))
            {
                model.PageTitle = "Voluntinder - Find a charity";
                //model.Users = ApplicationDbContext.Users.Where(x => !string.IsNullOrEmpty(user.Name));
            }
            else
            {
                model.PageTitle = "Voluntinder - Find a volunteer";
                var users = DbContext.AspNetUsers.Where(x => string.IsNullOrEmpty(x.Name)).ToList();
                foreach (var volunteer in users)
                {
                    var skills = DbContext.skills_list.Where(x => x.UserId == volunteer.Id);
                    var volunteerModel = new User {Email = volunteer.Email};
                    foreach (var skill in skills)
                    {
                        volunteerModel.Skills.Add(skill.Skill.Name);
                    }
                    model.Users.Add(volunteerModel);
                }
            }

            return View(model);
        }
    }

    public class MatchViewModel
    {
        public MatchViewModel()
        {
            Users = new List<User>();
        }
        public string PageTitle { get; set; }
        public List<User> Users { get; set; }
    }

    public class User
    {
        public User()
        {
            Skills = new List<string>();
        }
        public string Email { get; set; }
        public List<string> Skills { get; set; }

    }
}