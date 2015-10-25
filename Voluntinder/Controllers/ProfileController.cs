using System.Linq;
using System.Web.Mvc;
using Microsoft.Runtime.CompilerServices;
using Tweetinvi.Core.Extensions;
using Voluntinder.Models;
using VoluntinderDb;

namespace Voluntinder.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        protected voluntinder_dbEntities DbContext;

        public ProfileController()
        {
            DbContext = new voluntinder_dbEntities();
        }

        public ActionResult Index(string profileId)
        {
            var user = DbContext.AspNetUsers.FirstOrDefault(x => x.Id == profileId);

            var model = new ProfileViewModel
            {
                Name = user.Name,
                Description = user.Description,
                ImageUrl = user.ImageUrl,
                UserId = user.Id,
                UserName = user.UserName
            };

            var skills = DbContext.skills_list.Where(x => x.UserId == profileId);

            skills.ForEach(x => model.Skills.Add(x.Skill));

            return View(model);
        }

    }
}