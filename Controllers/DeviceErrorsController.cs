using AQMS.Data;
using AQMS.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AQMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceErrorsController : ControllerBase
    {
        private readonly AQMSapiDbContext _dbContext;
        private readonly ILogger<AQMSapiDbContext> _logger;
        public DeviceErrorsController(AQMSapiDbContext dbContext,ILogger<AQMSapiDbContext> logger)
        {
            _dbContext= dbContext;
            _logger= logger;    
        }
        [HttpGet]
        public ActionResult GetAllErrorDevices()
        {
            try
            {
                _logger.LogInformation("getting all error devices");
                IEnumerable<DeviceErrors>? data = _dbContext.deviceErrors.OrderByDescending(x => x.ErrorID).Take(10);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return StatusCode(404);
            }
        }
    }
}
