using AutoMapper;
using Flock.API.Models;
using Flock.Common.Helpers;
using Flock.Common.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ApiController
    {
        private readonly IUserService service;
        private readonly AppSettings appSettings;

        public HomeController(IUserService service, IWebHostEnvironment environment, ILogger<HomeController> logger, IMapper mapper, IOptions<AppSettings> appSettings)
            : base(environment, logger, mapper)
        {
            this.service = service;
            this.appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] UserLoginModel model)
        {
            var user = service.Login(model.Username, model.Password);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var result = Mapper.Map<UserModel>(user);
            result.Token = tokenString;
            return Ok(result);
        }
    }
}
