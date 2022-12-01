using AQMS.Data;
using AQMS.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AQMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AQMSdataController : Controller
    {
        private readonly AQMSapiDbContext _dbContext;
        public AQMSdataController(AQMSapiDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Getdata() 
        {
            return Ok(_dbContext.aQMSdatas.ToList());
        }

        [HttpPost]
        public void Postdata(AQMSdata aqmsObj) 
        { 
            if(aqmsObj != null)
            {
                _dbContext.aQMSdatas.Add(aqmsObj);
                _dbContext.SaveChanges();
            }  
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id) 
        {
            return (Ok(_dbContext.aQMSdatas.FirstOrDefault(x => x.ID == id)));
        }

        [HttpPut("{id}")] //  no one is allowed  to change the data in database as the is realtime
        public void Updatedata(int id , AQMSdata aqmsObj)
        {
            if (_dbContext.aQMSdatas.Any(x => x.ID == id))
            { 
                var NewData = _dbContext.aQMSdatas.FirstOrDefault(x=> x.ID == id);
                NewData.date = aqmsObj.date;
                NewData.CO = aqmsObj.CO;
                NewData.co2 = aqmsObj.co2;
                NewData.C = aqmsObj.C;  
                NewData.SO2= aqmsObj.SO2;
                NewData.O2= aqmsObj.O2;
                NewData.Temp= aqmsObj.Temp;
                NewData.PM= aqmsObj.PM;

                _dbContext.SaveChanges();
            }
            
        }



        [HttpDelete("{id}")]
        public void Deletedata(int id) 
        {
            if(_dbContext.aQMSdatas.Any( x => x.ID == id)) 
            {
                AQMSdata aqmsObj = _dbContext.aQMSdatas.FirstOrDefault(x => x.ID == id);
                _dbContext.aQMSdatas.Remove(aqmsObj);
                _dbContext.SaveChanges();
            }
        }
       
    }
}
