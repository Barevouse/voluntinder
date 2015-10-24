using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using ConsoleApplication1;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Voluntinder.Models;
using VoluntinderDb;

namespace Voluntinder.Controllers
{
    [Authorize]
    public class SkillsController : Controller
    {
        public voluntinder_dbEntities Dbcontext;
        public ApplicationUserManager user;

        public SkillsController()
        {
            Dbcontext = new voluntinder_dbEntities();
        }

        public ActionResult Index()
        {
            var userSkills = GetUserSkills(User.Identity.GetUserId());
            var allSkills = GetSkills(userSkills);
            var model = new SkillModels
            {
                Skills = allSkills
            };

            return View(model);
        }

        public void SaveSkills(string userId, List<long> skills)
        {
            foreach (var skill in skills)
            {
                Dbcontext.skills_list.Add(new skills_list {UserId = userId, SkillId = skill});
            }
            Dbcontext.SaveChanges();
        }

        public List<SkillsWithUser> GetUserSkills(string userId)
        {
            var skillsUserHas = new List<SkillsWithUser>();
            var skills = Dbcontext.skills_list.Where(x => x.UserId == userId).ToList();
            foreach (var skill in skills)
            {
                skillsUserHas.Add(new SkillsWithUser
                {
                    SkillId = skill.SkillId,
                    SkillName = skill.Skill.Name,
                    HasSkill = true
                });
            }
            return skillsUserHas;
        }

        public List<SkillsWithUser> GetSkills(List<SkillsWithUser> skillsWithUser)
        {
            var skills = Dbcontext.Skills;
            foreach (var skill in skills)
            {
                if(!skillsWithUser.Any(x=>x.SkillId == skill.Id))
                {
                    skillsWithUser.Add(new SkillsWithUser
                    {
                        SkillId = skill.Id,
                        SkillName = skill.Name,
                        HasSkill = false
                    });
                }
            }
            return skillsWithUser;
        }
    }
}