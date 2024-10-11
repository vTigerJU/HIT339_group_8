using Microsoft.AspNetCore.Identity;
namespace AnyoneForTennis.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string? Biography { get; set; }
        public byte[]? Photo { get; set; }
        public bool? Active { get; set; }
        public List<NewSchedule> Schedules { get; set; }
    }
}
