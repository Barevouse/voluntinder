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
        protected ApplicationDbContext ApplicationDbContext { get; set; }
        protected UserManager<ApplicationUser> UserManager { get; set; }

        public MatchController()
        {
            ApplicationDbContext = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }
        // GET: Match
        public ActionResult Index()
        {
            var model = new MatchViewModel();
            var user = UserManager.FindById(User.Identity.GetUserId());

            if (string.IsNullOrEmpty(user.Name))
            {
                model.PageTitle = "Voluntinder - Find a charity";
                //model.Users = ApplicationDbContext.Users.Where(x => !string.IsNullOrEmpty(user.Name));
            }
            else
            {
                model.PageTitle = "Voluntinder - Find a volunteer";
                var users = ApplicationDbContext.Users.Where(x => string.IsNullOrEmpty(user.Name)).ToList();
                foreach (var volunteer in users)
                {
                    model.Users.Add(new User { Email = volunteer.Email});
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
        public string Email { get; set; }
        public List<skills_list> Skills { get; set; }

    }
}