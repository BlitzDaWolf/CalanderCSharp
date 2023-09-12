using CalanderCSharp.Context;
using CalanderCSharp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalanderCSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly CalanderContext context;

        public GroupController(CalanderContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IList<User> Index()
        {
            var userId = JWTContext.GetUser(User);
            var u = context.Users.FirstOrDefault(u => u.Id == userId);
            if (u == null) return null;
            if (u.UserGroupId == null) return null;
            return context.Users.Where(x => x.UserGroupId == u.UserGroupId).ToList();
        }

        [HttpPost]
        public string CreateGroup()
        {
            var userId = JWTContext.GetUser(User);
            var u = context.Users.FirstOrDefault(u => u.Id == userId);
            if (u.UserGroupId != null) return "Already in a group";
            Group g = new Group { Name = $"{u.UserName}'s group" };
            context.Groups.Add(g);
            context.SaveChanges();
            u.UserGroupId = g.Id;
            context.Users.Update(u);
            context.SaveChanges();
            return "Group created";
        }

        [HttpPost("adduser/{userId}")]
        [Authorize]
        public void AddUserToGroup(int userId)
        {
            var myUserId = JWTContext.GetUser(User);
            var u = context.Users.FirstOrDefault(u => u.Id == myUserId);
            if (u == null) return;
            if (u.UserGroupId == null) return;

            var updateUser = context.Users.FirstOrDefault(x => x.Id == userId && x.UserGroupId == null);
            if (updateUser == null) return;
            updateUser.UserGroupId = u.UserGroupId;
            context.Users.Update(updateUser);
            context.SaveChanges();
        }
    }
}
