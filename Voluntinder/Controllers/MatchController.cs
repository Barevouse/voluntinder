using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Device.Location;
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
            var userLocation = new GeoCoordinate(user.Latitude.Value, user.Longitude.Value);
            var matches = new List<ProfileViewModel>();

            if (!user.IsCharity.Value)
            {
                var yourSkills = DbContext.skills_list.Where(x => x.UserId == user.Id);
                var skills = DbContext.Skills;
                var userSkills = new List<Skill>();

                foreach (var skill in skills)
                {
                    if (yourSkills.Any(y => y.SkillId == skill.Id))
                    {
                        userSkills.Add(skill);
                    }
                }

                model.PageTitle = "Find a charity";
                model.UserLocation = user.Location;
                var charities = DbContext.AspNetUsers.Where(x => x.IsCharity == true);
                var pairings = DbContext.Pairings.Where(y => y.UserId == user.Id);
                foreach (var pair in charities)
                {
                    var theirSkills = DbContext.skills_list.Where(x => x.UserId == pair.Id);
                    if (!pairings.Any(x => x.PairedUser == pair.Id) &&
                        (theirSkills.Select(y => y.SkillId).Intersect(yourSkills.Select(c => c.SkillId)).Any() ||
                         !theirSkills.Any()))
                    {
                        matches.Add(new ProfileViewModel
                        {
                            Name = pair.Name,
                            Description = pair.Description,
                            ImageUrl = pair.ImageUrl,
                            Skills = userSkills,
                            UserId = pair.Id,
                            UserName = pair.UserName,
                            Location = pair.Location,
                            Distance = CalculateDistance(userLocation, pair)

                        });
                    }
                }
            }
            else
            {
                model.PageTitle = "Find a volunteer";
                model.UserLocation = user.Location;
                var users = DbContext.AspNetUsers.Where(x => x.IsCharity == false);
                var yourSkills = DbContext.skills_list.Where(x => x.UserId == user.Id);
                var pairings = DbContext.Pairings.Where(y => y.UserId == user.Id);
                foreach (var pair in users)
                {
                    var skill_list = DbContext.skills_list.Where(x => x.UserId == user.Id);
                    var skills = DbContext.Skills;
                    var userSkills = new List<Skill>();

                    foreach (var skill in skills)
                    {
                        if (skill_list.Any(y => y.SkillId == skill.Id))
                        {
                            userSkills.Add(skill);
                        }
                    }
                    var theirSkills = DbContext.skills_list.Where(x => x.UserId == pair.Id);
                    if (!pairings.Any(x => x.PairedUser == pair.Id) &&
                        (theirSkills.Select(y => y.SkillId).Intersect(yourSkills.Select(c => c.SkillId)).Any() ||
                         !theirSkills.Any()))
                    {
                        matches.Add(new ProfileViewModel
                        {
                            Name = pair.Name,
                            Description = pair.Description,
                            ImageUrl = pair.ImageUrl,
                            Skills = userSkills,
                            UserId = pair.Id,
                            UserName = pair.UserName,
                            Location = pair.Location,
                            Distance = CalculateDistance(userLocation, pair)
                        });
                    }
                }
            }

            model.Profile = matches;
                return View(model);
            }

        public void Accept(string profileId)
        {
            var userId = User.Identity.GetUserId();
            DbContext.Pairings.Add(new Pairing
            {
                UserId = userId,
                PairedUser = profileId,
                Paired = true,
                MatchedOn = DateTime.Now
            });

            DbContext.SaveChanges();
        }

        public void Reject(string profileId)
        {
            var userId = User.Identity.GetUserId();
            DbContext.Pairings.Add(new Pairing
            {
                UserId = userId,
                PairedUser = profileId,
                Paired = false,
                MatchedOn = DateTime.Now
            });

            DbContext.SaveChanges();
        }

        private string CalculateDistance(GeoCoordinate userLocation, AspNetUser aspNetUser)
        {
            if (aspNetUser.Longitude.HasValue && aspNetUser.Latitude.HasValue)
            {
                var pairedLocation = new GeoCoordinate(aspNetUser.Latitude.Value, aspNetUser.Longitude.Value);
                var distance = userLocation.GetDistanceTo(pairedLocation) * 0.0016; // metres
                return string.Format("{0} miles from you", distance.ToString("N0"));
            }

            return string.Empty;
        }
    }
}