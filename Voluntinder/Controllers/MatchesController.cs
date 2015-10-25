using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Device.Location;
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
            var userId = User.Identity.GetUserId();
            var user = Dbcontext.AspNetUsers.Find(userId);
            var userLocation = new GeoCoordinate(user.Latitude.Value, user.Longitude.Value);
            var model = new MyMatchesModel();
            var allMyPairing = Dbcontext.Pairings.Where(x => x.UserId == userId).ToList();
            var myPairing = Dbcontext.Pairings.Where(x => x.PairedUser == userId && x.Paired).ToList();
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
                            ProfileLink = "/profile?profileId=" + pairing.PairedUser,
                            Tweet = "https://twitter.com/intent/tweet?hashtags=Voluntinder&original_referer=https%3A%2F%2Fvoluntinder.azurewebsites.net%2Fweb%2Ftweet-button&ref_src=twsrc%5Etfw&related=%2Ctwitter&text=Looking%20forward%20to%20working%20together&tw_p=tweetbutton&via=" + pairing.AspNetUser.UserName,
                            DistanceFrom = CalculateDistance(userLocation, pairing.AspNetUser)
                        });
                    }
                }

            model.Matches = matches;

            return View(model);
        }

        private double CalculateDistance(GeoCoordinate userLocation, AspNetUser aspNetUser)
        {
            var pairedLocation = new GeoCoordinate(aspNetUser.Latitude.Value, aspNetUser.Longitude.Value);
            return userLocation.GetDistanceTo(pairedLocation) * 0.0016; // metres
        }
    }
}