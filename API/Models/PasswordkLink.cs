using System;
using System.Collections.Generic;

namespace API.Models;

public partial class PasswordkLink
{
    public int PasswordLinkId { get; set; }

    public string PersonUsername { get; set; }

    public DateTime Time { get; set; }

    public bool Deleted { get; set; }
}
