using AcademicRecordsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace AcademicRecordsApp
{
    public class StartUp
    {
        static void Main()
        {
            using AcademicRecordsDBContext dbContext = new();

            // Every time the app is started, the DB will be migrated to the latest migration available
            // This is not suitable for PRODUCTION!!! Only development purposes
            // For PRODUCTION, use Migration Bundles -> easier CI/CD integration
            dbContext.Database.Migrate();
        }
    }
}
