using BCrypt.Net;
using CalanderCSharp.Context;
using CalanderCSharp.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CalanderCSharp.Controllers
{
    public struct RegisterContext
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly CalanderContext context;

        public UserController(CalanderContext context)
        {
            this.context = context;
        }

        [HttpPost("register")]
        public bool Register(RegisterContext registerData)
        {
            if (registerData.Password != registerData.RePassword) return false;
            if(context.Users.Where(x => x.UserEmail == registerData.UserEmail || x.UserName == registerData.UserName).Any()) return false;
            Entities.User u = new Entities.User { UserEmail = registerData.UserEmail, UserName = registerData.UserName };
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerData.Password, salt);
            u.Password = hashedPassword;
            context.Users.Add(u);
            context.SaveChanges();
            return true;
        }

        [HttpPost("login")]
        public string Login(string user, string pass)
        {
            User u = context.Users.FirstOrDefault(u => u.UserName == user);
            if (u == null) return "";
            if (!BCrypt.Net.BCrypt.Verify(pass, u.Password)) return "";
            return JWTContext.GenerateToken(u.Id.ToString(), 60 * 24);
        }
    }
}
