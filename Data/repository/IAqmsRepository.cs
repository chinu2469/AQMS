using AQMS.Model;

namespace AQMS.Data.repository
{
    //this interface is used by another classes to implement any method on database
    public interface IAqmsRepository 
    {
        List<AQMSdata> Getall();
        AQMSdata Get(int id);
        void Post(AQMSdata obj);
        void Update(int id ,AQMSdata data);
        void Delete(AQMSdata obj);
        bool Exists(int id);
        AQMSdata LastRow();
        AQMSdata LastFloor(int floor);
        List<AQMSdata> ByDay();
        List<AQMSdata> ByMonth(string month = null);
        List<AQMSdata> GetDataByYear(int year);
    }
}
