using System;
using System.Collections.Generic;

namespace API.Models;

public partial class TestGroupIntern
{
    public int TestGroupId { get; set; }

    public int TestId { get; set; }

    public int? GroupId { get; set; }

    public int? InternId { get; set; }

    public bool Deleted { get; set; }

    public virtual Group Group { get; set; }

    public virtual Person Intern { get; set; }

    public virtual Test Test { get; set; }
}
