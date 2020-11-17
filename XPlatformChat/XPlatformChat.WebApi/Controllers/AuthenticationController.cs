using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using XPlatformChat.Lib.Models;

namespace XPlatformChat.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<ChatUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<ChatUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration) {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || await _userManager.CheckPasswordAsync(user, model.Password) == false) 
                return Unauthorized();
            
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return Ok(new TokenGrant
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                Username = user.UserName
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            var user = new ChatUser
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            return result.Succeeded ? 
                Ok(new Response { Status = "Success", Message = "User created successfully!" }) :
                StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error", Message = "User creation failed! Please check user details and try again."
                });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            bool userExists = await _userManager.FindByNameAsync(model.Username) != null;
            if (userExists)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error", Message = "User already exists!"
                });

            var user = new ChatUser
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded == false)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error", Message = "User creation failed! Please check user details and try again."
                });

            if(await EnsureRolesExist())
                await _userManager.AddToRoleAsync(user, ChatUserRoles.Admin);

            return Ok(new Response
            {
                Status = "Success", Message = "User created successfully!"
            });
        }

        private async Task<bool> EnsureRolesExist()
        {
            try
            {
                // TODO: Move to database initialization instead.
                if (!await _roleManager.RoleExistsAsync(ChatUserRoles.Admin))
                    await _roleManager.CreateAsync(new IdentityRole(ChatUserRoles.Admin));
                if (!await _roleManager.RoleExistsAsync(ChatUserRoles.User))
                    await _roleManager.CreateAsync(new IdentityRole(ChatUserRoles.User));
                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                return false;
            }
        }
    }
}
