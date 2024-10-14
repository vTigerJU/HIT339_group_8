using Microsoft.AspNetCore.Identity;

namespace AnyoneForTennis.Models
{
    public interface IDataMigrationService
    {

        Task MigrateData(UserManager<ApplicationUser> manager);
    }
}