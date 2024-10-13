namespace AnyoneForTennis.Models.ViewModels
{
    public class UserScheduleViewModel
    {
        public ApplicationUser? User { get; set; }
        public List<NewSchedule> Schedules = new List<NewSchedule>();

    }
}
