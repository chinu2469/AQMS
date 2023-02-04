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
        public List<AQMSdata> ByDay()                            //returns last data of day
        {
            return _dbContext.aQMSdatas.Where(x => x.date.Day == DateTime.Now.Day).ToList();
        }
        public List<AQMSdata> ByMonth(string? month = null)                                                  //sends all data in table
        {
            var lastMonth = DateTime.Now.AddMonths(-1);
            List<AQMSdata> dataByMonth = _dbContext.aQMSdatas.Where(x => x.date >= lastMonth && x.date <= DateTime.Now).ToList();
            if (!string.IsNullOrEmpty(month))
            {
                switch (month)
                {
                    case "January":
                        dataByMonth = _dbContext.aQMSdatas.Where(x => x.date.Month == 1).ToList();
                        break;
                    case "February":
                        dataByMonth = _dbContext.aQMSdatas.Where(x => x.date.Month == 2).ToList();
                        break;
                    case "March":
                        dataByMonth = _dbContext.aQMSdatas.Where(x => x.date.Month == 3).ToList();
                        break;
                    case "April":
                        dataByMonth = _dbContext.aQMSdatas.Where(x => x.date.Month == 4).ToList();
                        break;
                    case "May":
                        dataByMonth = _dbContext.aQMSdatas.Where(x => x.date.Month == 5).ToList();
                        break;
                    case "June":
                        dataByMonth = _dbContext.aQMSdatas.Where(x => x.date.Month == 6).ToList();
                        break;
                    case "July":
                        dataByMonth = _dbContext.aQMSdatas.Where(x => x.date.Month == 7).ToList();
                        break;
                    case "August":
                        dataByMonth = _dbContext.aQMSdatas.Where(x => x.date.Month == 8).ToList();
                        break;
                    case "September":
                        dataByMonth = _dbContext.aQMSdatas.Where(x => x.date.Month == 9).ToList();
                        break;
                    case "October":
                        dataByMonth = _dbContext.aQMSdatas.Where(x => x.date.Month == 10).ToList();
                        break;
                    case "November":
                        dataByMonth = _dbContext.aQMSdatas.Where(x => x.date.Month == 11).ToList();
                        break;
                    case "December":
                        dataByMonth = _dbContext.aQMSdatas.Where(x => x.date.Month == 12).ToList();
                        break;
                    
                    default:
                    
                        throw new ArgumentException("Invalid month value");

                        break;
                }
            }
            return dataByMonth;
        }

        public List<AQMSdata> GetDataByYear(int year)
        {
            var startDate = new DateTime(year, 1, 1);
            var endDate = new DateTime(year, 12, 31);
            var dataByYear = _dbContext.aQMSdatas.Where(x => x.date >= startDate && x.date <= endDate).ToList();
            return dataByYear;
        }

    }
}
