using AQMS.Data;
using AQMS.Data.repository;
using AQMS.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Core;
using System.Net;
using System.Net.Http;




namespace AQMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AQMSdataController : Controller
    {
        // creting private instance of repository to acces its method in this class only
        private readonly IAqmsRepository _dataset;
        private readonly ILogger<AQMSapiDbContext> _logger;             //logger instance to get all logger method
        public AQMSdataController(IAqmsRepository dataset, ILogger<AQMSapiDbContext> logger)
        {
            this._dataset = dataset;                                    //assigning instance
            this._logger = logger;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Getalldata() 
        {
            try
            {
                _logger.LogInformation("-------********************-------getting all data-------****************-------");
                IEnumerable<AQMSdata>? data = _dataset.Getall();        //use getall method to retrive data from database
                if (data.Count() == 0)
                {
                    throw new Exception("data not found");
                }
                return Ok(data);                                    //return enumerable object with 200
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);                       //returns badrequest
            }
        }

        

        [HttpPost]
        public ActionResult Postdata(AQMSdata aqmsObj)
        {
            try
            {
                if(aqmsObj != null)
                {
                _logger.LogInformation("-------********************-------posting all data-------******************-------");
                _dataset.Post(aqmsObj);
                return Ok(aqmsObj);                                 //uses post method from repository to post whole object
                }
                else
                {
                    throw (new Exception("data is null here"));     //throws specific exeption
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult Getbyid(int id) 
        {
            try
            {
                if (id != null && id != 0 && _dataset.Exists(id)) //  to ccheck if it exist or not
                {
                    _logger.LogInformation("-------*********************-------gatting single data-------******************-------");
                    return (Ok(_dataset.Get(id)));   //_we get only one oject
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
                _logger.LogInformation("-------*******************-------updating data-------*****************-------");
                if (_dataset.Exists(id)) //checks that id exist or not
                {
                    _dataset.Update(id, aqmsObj);                             //uses update method in repository to update data
                }
                else
                {
                    throw new Exception("Intered id does not exist in database");
                }
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
            
        }



        [HttpDelete("{id}")]
        public void Deletedata(int id) 
        {
            try
            {
                _logger.LogInformation("-------***************************deleting data*************************-------");
                AQMSdata aqmsObj = _dataset.Get(id);//_dbContext.aQMSdatas.FirstOrDefault(x => x.ID == id);
                if(aqmsObj != null)                   //_dbContext.aQMSdatas.Any(x => x.ID == id)
                {
                    _dataset.Delete(aqmsObj);
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
        [Authorize]
        [HttpGet]           //last row of the table is returned by this api call so the=at we can display live data on screen
        [Route("/[controller]/LastRowdata")]                        //specify the adress to avoid confussion of multiple get methods
        public ActionResult LastRowdata()
        {
            try
            {
                _logger.LogInformation("-------***************************getting last row of data*************************-------");
                return (Ok(_dataset.LastRow()));
                 
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]                                                //last row of given floor returns
        [Route("/[controller]/LastofFloor")]                    //specify the adress to avoid confussion of multiple get methods
        public ActionResult LastofFloor(int floor)
        {
            try
            {
                _logger.LogInformation("-------***************************getting last row of data*************************-------");
                return (Ok(_dataset.LastFloor(floor)));

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]                                                //data for the day
        [Route("/[controller]/ByDay")]                    //specify the adress to avoid confussion of multiple get methods
        public ActionResult ByDay()
        {
            try
            {
                _logger.LogInformation("-------***************************getting last row of data*************************-------");
                IEnumerable<AQMSdata>? data = _dataset.ByDay();
                return (Ok(data));

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]                                                //data for the month
        [Route("/[controller]/ByMonth")]                    //specify the adress to avoid confussion of multiple get methods
        public ActionResult ByMonth(string month = null)
        {
            try
            {
                _logger.LogInformation("-------***************************getting last row of data*************************-------");
                IEnumerable<AQMSdata>? data = _dataset.ByMonth( month = null);
                return (Ok(data));

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]                                                //data for the year
        [Route("/[controller]/ByYear")]                    //specify the adress to avoid confussion of multiple get methods
        public ActionResult ByYear(int year)
        {
            try
            {
                _logger.LogInformation("-------***************************getting last row of data*************************-------");
                IEnumerable<AQMSdata>? data = _dataset.GetDataByYear( year);
                return (Ok(data));

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
