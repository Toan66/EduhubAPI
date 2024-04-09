using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EduhubAPI.Helpers
{
    public class JwtService
    {
        private string secureKey = "this is my custom Secret key for authentication";

        public string Generate(int id, IEnumerable<string> roles) // Added roles as a parameter
        {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier, id.ToString()) // Changed to NameIdentifier and converted id to string
            };

            foreach (var role in roles) // Now roles is passed as a parameter
            {
                claims.Add(new Claim(ClaimTypes.Role, role)); // This is correct, just ensure roles is a string enumerable
            }

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);

            var payload = new JwtPayload(id.ToString(), null, claims, null, DateTime.Today.AddDays(1)); // Added claims to payload
            var securityToken = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
        public JwtSecurityToken Verify(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secureKey);
            tokenHandler.ValidateToken(jwt, new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false
            }, out SecurityToken validatedToken);

            return (JwtSecurityToken)validatedToken;
        }

    }
}
