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
            var myPairing = Dbcontext.Pairings.Where(x => x.PairedUser == user && x.Paired).ToList();
            var matches = new List<MatchesShortProfile>();

            foreach (var pairing in allMyPairing)
            {                    
                if(myPairing.Any(x=>x.UserId == pairing.PairedUser) && pairing.Paired)
                    {
                        matches.Add(new MatchesShortProfile
                        {
                            Name = pairing.AspNetUser.Name,
                            MatchedOn = pairing.MatchedOn.Value,
                            ProfileImage = pairing.AspNetUser.ImageUrl,
                            ProfileLink = "/profile?profileId=" + pairing.PairedUser
                        });
                    }
                }

            model.Matches = matches;

            return View(model);
        }
    }
}