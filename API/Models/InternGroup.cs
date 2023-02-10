using System;
using System.Collections.Generic;

namespace API.Models;

public partial class InternGroup
{
    public int InternGroupId { get; set; }

    public int GroupId { get; set; }

    public int InternId { get; set; }

    public bool Deleted { get; set; }

    public virtual Group Group { get; set; }

    public virtual Person Intern { get; set; }
}
