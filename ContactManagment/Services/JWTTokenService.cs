using ContactManagment.DTOs;
using ContactManagment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactManagment.Services
{
    public class JWTTokenService
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public JWTTokenService(UserManager<User> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }
        public async Task<JwtTokenDTO> GenerateToken(User targetUser)
        {
            

            List<Claim> UserClaims = new List<Claim>();
            UserClaims.Add(new Claim(ClaimTypes.NameIdentifier, targetUser.Id.ToString()));
            UserClaims.Add(new Claim(ClaimTypes.Name, targetUser?.UserName));
            IList<string> listRoles = await userManager.GetRolesAsync(targetUser);
            foreach (var role in listRoles)
            {
                UserClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            UserClaims.Add(new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            // ---> make signin credentials
            var SignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]));
            SigningCredentials signingCredentials = new SigningCredentials(SignInKey, SecurityAlgorithms.HmacSha256);




            JwtSecurityToken myToken = new JwtSecurityToken(
                audience: configuration["JWT:Audience"], // My local host 
                issuer: configuration["JWT:Issuer"], // angular local host
                expires: DateTime.Now.AddHours(1),
                claims: UserClaims,
                signingCredentials: signingCredentials
                );

            return new JwtTokenDTO() { Token=new JwtSecurityTokenHandler().WriteToken(myToken), ExpireDate= DateTime.Now.AddHours(1) };
        }
    }
}
