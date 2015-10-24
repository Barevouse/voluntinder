using System.Collections.Generic;
using System.Data.Entity;
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
            var matches = new List<PairingCards>();

            if (user.IsCharity == false)
            {
                model.PageTitle = "Find a charity";
                var charities = DbContext.AspNetUsers.Where(x => x.IsCharity == true).ToList();
                var pairings = DbContext.Pairings.Where(y => y.UserId == user.Id);
                foreach (var pair in charities)
                {
                    foreach (var pairing in pairings)
                    {
                        if (pairing.PairedUser != pair.Id)
                        {
                            matches.Add(new PairingCards
                            {
                                Name = pair.Name,
                                Description = null,

                            });
                        }
                    }
                }

                model = BuildModel(charities);

            }
            else
            {
                model.PageTitle = "Find a volunteer";
                var users = DbContext.AspNetUsers.Where(x => string.IsNullOrEmpty(x.Name)).ToList();

                model = BuildModel(users);
            }

            return View(model);
        }

        public MatchViewModel BuildModel(IEnumerable<AspNetUser> users)
        {
            var model = new MatchViewModel();

            //    foreach (var volunteer in users)
            //    {
            //        var skills = DbContext.skills_list.Where(x => x.UserId == volunteer.Id);
            //        var volunteerModel = new User { Email = volunteer.Email };
            //        foreach (var skill in skills)
            //        {
            //            volunteerModel.Skills.Add(skill.Skill.Name);
            //        }
            //        model.Users.Add(volunteerModel);
            //    }

            return model;
            //}
        }

    }
}