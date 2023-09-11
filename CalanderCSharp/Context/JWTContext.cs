using CalanderCSharp.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CalanderCSharp.Context
{
    public class JWTContext
    {
        public static string ValidIssuer;
        public static string ValidAudience;
        public static string secret;

        public static string GenerateToken(string userName, int expireMinutes = 30)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, userName), // You can set the username here
            };

            var token = new JwtSecurityToken(
                issuer: ValidIssuer,
                audience: ValidAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public static int GetUser(ClaimsPrincipal User)
        {
            var claims = User.Claims.ToList();
            var t = (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier));
            return int.Parse(t.Value);
        }
    }
}
