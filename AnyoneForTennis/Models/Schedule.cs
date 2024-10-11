namespace AnyoneForTennis.Models
{
    public class Schedule
    {
        public int ScheduleId { get; set; }

        public string Name { get; set; } = null!;

        public string? Location { get; set; }

        public string? Description { get; set; }
    }
}
