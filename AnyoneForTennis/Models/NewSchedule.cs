using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnyoneForTennis.Models;

public partial class NewSchedule
{
    [Key]
    public int ScheduleId { get; set; }

    public string Name { get; set; } = null!;

    public string? Location { get; set; }

    public string? Description { get; set; }
    [DataType(DataType.Date)]
    public DateOnly Date  { get; set; }
    public string? CoachId { get; set; }

    public ApplicationUser? Coach {  get; set; }

    public List<ApplicationUser>? Members { get; set; }

}
