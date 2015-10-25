using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Voluntinder.Models;
using VoluntinderDb;
using RedirectToRouteResult = System.Web.Http.Results.RedirectToRouteResult;

namespace Voluntinder.Controllers
{
    public class MatchesController : Controller
    {
        public voluntinder_dbEntities Dbcontext;

        public MatchesController()
        {
             Dbcontext = new voluntinder_dbEntities();
        }

        // GET: Matches
        public ActionResult Index()
        {
            var user = User.Identity.GetUserId();
            var model = new MyMatchesModel();
            var allMyPairing = Dbcontext.Pairings.Where(x => x.UserId == user).ToList();
            var MyPairing = Dbcontext.Pairings.Where(x => x.PairedUser == user).ToList();
            var matches = new List<MatchesShortProfile>();

            foreach (var pairing in allMyPairing)
            {
                foreach (var othersPairs in MyPairing)
                {
                    if (pairing.UserId == othersPairs.PairedUser && othersPairs.UserId == pairing.PairedUser && pairing.Paired && othersPairs.Paired)
                    {
                        matches.Add(new MatchesShortProfile
                        {
                            Name = othersPairs.AspNetUser.Name,
                            MatchedOn = othersPairs.MatchedOn > pairing.MatchedOn ? othersPairs.MatchedOn.Value : pairing.MatchedOn.Value,
                            ProfileImage = othersPairs.AspNetUser.ImageUrl,
                            ProfileLink = "/profile?profileId=" + othersPairs.PairedUser
                        });
                    }
                }
            }

            model.Matches = matches;

            return View(model);
        }
    }
}