using System;
using System.Collections.Generic;

namespace API.Models;

public partial class ActivityRecord
{
    public int ActivityRecordId { get; set; }

    public int ActivityId { get; set; }

    public int PersonId { get; set; }

    public decimal? Distance { get; set; }

    public int BurnedKalories { get; set; }

    public TimeSpan? Time { get; set; }

    public bool Deleted { get; set; }

    public virtual Activity Activity { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;
}
