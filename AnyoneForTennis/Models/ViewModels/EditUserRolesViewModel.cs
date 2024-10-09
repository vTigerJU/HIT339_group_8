
namespace AnyoneForTennis.Models.ViewModels
{
    public class EditUserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public IList<string> AvailableRoles { get; set; }
        public IList<string> UserRoles { get; set; }
    }
}
