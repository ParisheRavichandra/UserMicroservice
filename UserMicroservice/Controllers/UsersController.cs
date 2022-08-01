using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using UserManagement.Models;
namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        public UsersController(UserManager<AppUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }
        //byte[] gb = Guid.NewGuid().ToByteArray();
        //int i = BitConverter.ToInt32(gb, 0);
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequestDTO dto)
        {
            AppUser user = new AppUser
            {
               // Id=dto.UserId,
                UserName=dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                
                
                
            };

            IdentityResult result = await userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded)
            {
                bool IsRolePresent = await roleManager.RoleExistsAsync("Customer");
               
                result = await userManager.AddToRoleAsync(user, "Customer");
                if (result.Succeeded)
                {
                    return StatusCode(201);
                }
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> SignIn(LoginRequestDTO dto)
        {
            AppUser user = await userManager.FindByNameAsync(dto.UserName);
            if (user == null)
                return BadRequest("Invalid username/password");
            bool IsValidPassword = await userManager.CheckPasswordAsync(user, dto.Password);
            if (!IsValidPassword)
                return BadRequest("Invalid username/password");

            string key = configuration["JwtSettings:key"];
            string issuer = configuration["JwtSettings:issuer"];
            string audience = configuration["JwtSettings:audience"];
            byte[] keyBytes = Encoding.ASCII.GetBytes(key);
            DateTime expires = DateTime.Now.AddMinutes(30);
            SecurityKey securityKey = new SymmetricSecurityKey(keyBytes);

            var userClaims = await userManager.GetClaimsAsync(user);
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserName));

            var userRoles = await userManager.GetRolesAsync(user);
            var role = userRoles.First();
            userClaims.Add(new Claim(ClaimTypes.Role, role));

            SigningCredentials credentails = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: userClaims,
                signingCredentials: credentails,
                expires: expires);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string jwt = handler.WriteToken(token);

            var response = new LoginResponseDTO() { Role = role, Token = jwt};
            return Ok(response);
        }
    }
}
