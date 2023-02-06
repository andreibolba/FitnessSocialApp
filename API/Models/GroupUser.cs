﻿using System;
using System.Collections.Generic;

namespace API.Models;

public partial class GroupUser
{
    public int GroupUserId { get; set; }

    public int PersonId { get; set; }

    public int GroupId { get; set; }

    public bool Deleted { get; set; }

    public virtual Group Group { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;
}
