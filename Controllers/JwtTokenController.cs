using AQMS.Data;
using AQMS.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
//using Jose;

namespace AQMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtTokenController : Controller
    {
        public IConfiguration _configuration;
        private readonly AQMSapiDbContext _dbContext;
        public JwtTokenController(AQMSapiDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            this._configuration = configuration;
        }

        [HttpPost]
        [Route("/[controller]/Login")]
        public IActionResult Login([FromBody] Users obj)
        {
            var currentUser = _dbContext.Users.FirstOrDefault(u => u.UserName == obj.UserName && u.Password == obj.Password);
            if (currentUser == null)
            {
                return NotFound();
            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email , currentUser.Email)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(jwt);

        }
        [HttpPost]
        [Route("/[controller]/Register")]
        public IActionResult Register([FromBody]Users user)
        {
            var userExists = _dbContext.Users.FirstOrDefault(u => u.UserName == user.UserName);
            if (userExists != null)
            {
                return BadRequest("User with same email already exists");
            }
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
            
        [HttpGet]
        public async Task<Users> GetUser( string username, string password)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
        }

    }
}
