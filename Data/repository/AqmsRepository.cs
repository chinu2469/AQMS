using AQMS.Model;
using System.Linq;

namespace AQMS.Data.repository
{
    public class AqmsRepository : IAqmsRepository
    {
        private readonly AQMSapiDbContext _dbContext;
        public AqmsRepository(AQMSapiDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public List<AQMSdata> Getall()                                                  //sends all data in table
        { 
            return _dbContext.aQMSdatas.ToList();
        }

        public void Post(AQMSdata obj)                                                  //recieves an obj and saves in database
        {
            _dbContext.aQMSdatas.Add(obj);
            _dbContext.SaveChanges();
        }
        public void Delete(AQMSdata obj)                                                //deletes specific object in database
        {
            _dbContext.aQMSdatas.Remove(obj);
            _dbContext.SaveChanges();
        }
        public AQMSdata? Get(int id)                                                       //get a specific object as per id
        {
            return _dbContext.aQMSdatas.FirstOrDefault(x => x.ID == id);
        }
        public void Update(int id,AQMSdata obj)                                         // replace the given object in database
        {
            var NewData = _dbContext.aQMSdatas.FirstOrDefault(x => x.ID == id);
            NewData.date = obj.date;
            NewData.floor = obj.floor;
            NewData.SensorID = obj.SensorID;
            NewData.CO = obj.CO;
            NewData.co2 = obj.co2;
            NewData.C = obj.C;
            NewData.SO2 = obj.SO2;
            NewData.O2 = obj.O2;
            NewData.Temp = obj.Temp;
            NewData.PM = obj.PM;

            _dbContext.SaveChanges();
        }
        public bool Exists(int id)                                  //checks tha data exist in table or not useful for get by id
        {
            if(_dbContext.aQMSdatas.Any(x => x.ID == id))
                return true;
            else
                return false;
        }
        public AQMSdata LastRow()                                    //to get last value in table                                
        {
            return _dbContext.aQMSdatas.OrderBy(x => x.ID).Last();
        }
        public AQMSdata LastFloor(int floor)                            //returns last data of given floor
        {
            return _dbContext.aQMSdatas.OrderBy(x => x.ID).LastOrDefault(x=> x.floor == floor);
        }
    }
}
