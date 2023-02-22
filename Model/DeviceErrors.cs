using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AQMS.Model
{
    public class DeviceErrors
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ErrorID { get; set; }
        public int floor { get; set; }
        public int SensorID { get; set; }
    }
}
