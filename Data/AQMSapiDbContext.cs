using AQMS.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace AQMS.Data
{
    public class AQMSapiDbContext : DbContext
    {
        public AQMSapiDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<AQMSdata> aQMSdatas { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
