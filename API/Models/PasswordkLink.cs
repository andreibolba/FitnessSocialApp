using System;
using System.Collections.Generic;

namespace API.Models;

public partial class PasswordkLink
{
    public int PasswordLinkId { get; set; }

    public int PersonUsername { get; set; }

    public DateTime Time { get; set; }

    public bool Deleted { get; set; }
}
