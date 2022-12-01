using System.ComponentModel.DataAnnotations;

namespace AQMS.Model
{
    public class AQMSdata
    {
        [Key]
        [Required]
        public int ID { get; set; }
        public DateTime date { get; set; }
        //public TimeOnly time { get; set; }
        public int O2 { get; set; }
        public int co2 { get; set; }
        public int SO2 { get; set; }
        public int CO { get; set; }
        public int C { get; set; }
        public int Temp { get; set; }
        public float PM { get; set; }

    }
}
