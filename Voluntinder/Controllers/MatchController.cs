using System;
using System.Collections;
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
            var matches = new List<ProfileViewModel>();
            //var allUsers = DbContext.AspNetUsers;
            //var userSkills = DbContext.skills_list.Where(x => x.UserId == user.Id);
            //var allSkills = DbContext.skills_list;

            //if (user.IsCharity.Value)
            //{
            //    var charityMatches = DbContext.Pairings.Where(x => x.UserId == user.Id).Select(y => y.AspNetUser1); //Get those liked

            //    var matchedSkills = userSkills.Intersect(allSkills); //Only users with matched skills

            //    var matchedUsers = matchedSkills.Select(x => x.AspNetUser).Distinct(); //Get all distinct users with matched skills


            //    var unLiked = allUsers.Except(charityMatches);

            //    var unLikedWithMatchedSkills = unLiked.Intersect(matchedUsers);

            //}

            if (!user.IsCharity.Value)
            {
                var yourSkills = DbContext.skills_list.Where(x => x.UserId == user.Id).ToList();
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
                var charities = DbContext.AspNetUsers.Where(x => x.IsCharity == true).ToList();
                var pairings = DbContext.Pairings.Where(y => y.UserId == user.Id);
                foreach (var pair in charities)
                {
                    var theirSkills = DbContext.skills_list.Where(x => x.UserId == pair.Id);
                    if (!pairings.Any(x => x.PairedUser == pair.Id && theirSkills.Select(y=>y.SkillId).Intersect(yourSkills.Select(c=>c.SkillId)).Any()))
                    {
                        matches.Add(new ProfileViewModel
                        {
                            Name = pair.Name,
                            Description = pair.Description,
                            ImageUrl = pair.ImageUrl,
                            Skills = userSkills,
                            UserId = pair.Id,
                            UserName = pair.UserName

                        });
                    }
                }
            }
            else
            {
                model.PageTitle = "Find a volunteer";
                var users = DbContext.AspNetUsers.Where(x => x.IsCharity == false).ToList();
                var pairings = DbContext.Pairings.Where(y => y.UserId == user.Id);
                foreach (var pair in users)
                {
                    var skill_list = DbContext.skills_list.Where(x => x.UserId == user.Id).ToList();
                    var skills = DbContext.Skills;
                    var userSkills = new List<Skill>();

                    foreach (var skill in skills)
                    {
                        if (skill_list.Any(y => y.SkillId == skill.Id))
                        {
                            userSkills.Add(skill);
                        }
                    }
                    if (!pairings.Any(x => x.PairedUser == pair.Id))
                        matches.Add(new ProfileViewModel
                        {
                            Name = pair.Name,
                            Description = pair.Description,
                            ImageUrl = pair.ImageUrl,
                            Skills = userSkills,
                            UserId = pair.Id,
                            UserName = pair.UserName
                        });
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
    }
}