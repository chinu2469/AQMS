using AQMS.Data;
using AQMS.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Net;
using System.Net.Http;




namespace AQMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AQMSdataController : Controller
    {
        // creting private object for this class only
        private readonly AQMSapiDbContext _dbContext;
        // logger
        private readonly ILogger<AQMSdataController> _logger;
        public AQMSdataController(AQMSapiDbContext dbContext , ILogger<AQMSdataController> logger)
        {
            this._dbContext = dbContext;
            this._logger = logger;
        }

      


        [HttpGet]
        public IActionResult Getdata() 
        {

            try
            {
                _logger.LogInformation("-------***************************getting all data*************************-------");
                //Log.Information("get all data'");
                IEnumerable<AQMSdata>? data = _dbContext.aQMSdatas.ToList();
                if (data.Count() == 0)
                {
                    throw new Exception("data not found");
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        

        [HttpPost]
        public ActionResult Postdata(AQMSdata aqmsObj)
        {
            try
            {
                //if(aqmsObj != null)
                //{
                _logger.LogInformation("-------***************************posting all data*************************-------");
                _dbContext.aQMSdatas.Add(aqmsObj);
                _dbContext.SaveChanges();
                return Ok(aqmsObj);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id) 
        {
            try
            {
                if (id != null && id != 0 ) // how to ccheck if it exist or not
                {
                    _logger.LogInformation("-------***************************gatting single data*************************-------");
                    return (Ok(_dbContext.aQMSdatas.FirstOrDefault(x => x.ID == id)));
                    
                }
                else
                {
                    string msg = "You have entered Wrong Id";
                    _logger.LogInformation($"error {msg}/n {BadRequest()}");
                    return BadRequest(msg); //Request.CreateResponse(HttpStatusCode.BadRequest, msg);//
                }   // have included system.net.http
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")] //  no one is allowed  to change the data in database as the is realtime
        public void Updatedata(int id , AQMSdata aqmsObj)
        {
            try
            {
                _logger.LogInformation("-------***************************updating data*************************-------");
                if (_dbContext.aQMSdatas.Any(x => x.ID == id))
                {
                    var NewData = _dbContext.aQMSdatas.FirstOrDefault(x => x.ID == id);
                    NewData.date = aqmsObj.date;
                    NewData.CO = aqmsObj.CO;
                    NewData.co2 = aqmsObj.co2;
                    NewData.C = aqmsObj.C;
                    NewData.SO2 = aqmsObj.SO2;
                    NewData.O2 = aqmsObj.O2;
                    NewData.Temp = aqmsObj.Temp;
                    NewData.PM = aqmsObj.PM;

                    _dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("Intered id does not exist in database");
                }
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                //return BadRequest(ex.Message);
            }
            
        }



        [HttpDelete("{id}")]
        public void Deletedata(int id) 
        {
            try
            {
                _logger.LogInformation("-------***************************deleting data*************************-------");
                AQMSdata aqmsObj = _dbContext.aQMSdatas.FirstOrDefault(x => x.ID == id);
                if(aqmsObj != null)                   //_dbContext.aQMSdatas.Any(x => x.ID == id)
                {
                    
                    _dbContext.aQMSdatas.Remove(aqmsObj);
                    _dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("Entered id does not exist");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
               
            }
        }
       
    }
}
