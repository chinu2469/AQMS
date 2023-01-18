using System.ComponentModel.DataAnnotations;

namespace AQMS.Model
{
    public class AQMSdata
    {
        [Key]
        [Required]
        public long ID { get; set; }
        public DateTime date { get; set; }
        //public TimeOnly time { get; set; }
        public int floor { get; set; }
        public int SensorID { get; set; }
        public int O2 { get; set; }
        public int co2 { get; set; }
        public int SO2 { get; set; }
        public int CO { get; set; }
        public int C { get; set; }
        public int Temp { get; set; }
        public float PM { get; set; }

    }
}
