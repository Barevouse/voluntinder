using System.Linq;
using System.Web.Mvc;
using Microsoft.Runtime.CompilerServices;
using Tweetinvi;
using Tweetinvi.Core.Credentials;
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

            var twitterCredentials = new TwitterCredentials("0N5Q3XLB9CyXBaskZGlurmjr4", "2wsz47zbOEQaj8hz2UCkozxFU8BwldpTPqb42I2grRYJWiYseF", "401598968-hM5pvUyjZPcH9J5B32l9u3SjuLoKuybwS2SNxdhA", "FN5HEHob09t75fJJB422KP4MmJMzg2DvKlMgCkhr9HjKV");
            var profile = Auth.ExecuteOperationWithCredentials(twitterCredentials, () =>
            {
                return Tweetinvi.User.GetUserFromScreenName(user.UserName);
            });

            var tweets = Auth.ExecuteOperationWithCredentials(twitterCredentials, () =>
            {
                return profile.GetUserTimeline(10);
            });

            model.Tweets = tweets;
            return View(model);
        }

    }
}