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
        private readonly ILogger<AQMSapiDbContext> _logger;
        public JwtTokenController(AQMSapiDbContext dbContext, IConfiguration configuration, ILogger<AQMSapiDbContext> logger)
        {
            _dbContext = dbContext;
            this._configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("/[controller]/Login")]
        public IActionResult Login([FromBody] Users obj)        //login method for user login
        {
            try 
            {
                _logger.LogInformation($"-------********************-------logine attempt by {obj.UserName}-------****************-------");
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
                _logger.LogInformation($"-------**-------logine success for {obj.UserName}-------**-------");
                return Ok(jwt);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return StatusCode(404);
            }

        }
        [HttpPost]
        [Route("/[controller]/Register")]
        public IActionResult Register([FromBody]Users user)
        {
            try
            {
                var userExists = _dbContext.Users.FirstOrDefault(u => u.UserName == user.UserName);
                if (userExists != null)
                {
                    return BadRequest("User with same email already exists");
                }
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                _logger.LogInformation($"-------**-------Register  success for {user.UserName}-------**-------");
                return StatusCode(StatusCodes.Status201Created);
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }
            
        [HttpGet]
        public async Task<Users> GetUser( string username, string password)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username && u.Password == password);
        }

    }
}
