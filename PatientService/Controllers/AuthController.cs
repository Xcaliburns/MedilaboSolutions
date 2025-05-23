﻿using MedilaboSolutionsBack1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MedilaboSolutionsBack1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IConfiguration configuration, ILogger<AuthController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded)
            {
                var token = GenerateJwtToken(user);

                // Configuration des options du cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, 
                    SameSite = SameSiteMode.Lax, 
                    Expires = DateTime.UtcNow.AddHours(2) 
                };

                // Ajout le cookie à la réponse
                HttpContext.Response.Cookies.Append("authToken", token, cookieOptions);

                // Inclure le token dans la réponse
                return Ok(new { Message = "Login successful", Token = token });
            }

            return Unauthorized();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Logout successful" });
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Attribution des roles 
            var roles = _userManager.GetRolesAsync(user).Result;
            _logger.LogInformation("Roles retrieved for user {UserName}: {Roles}", user.UserName, string.Join(", ", roles));
            claims.AddRange(roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role)));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
