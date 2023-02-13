using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Logging
{
    public int LoggingId { get; set; }

    public string LogType { get; set; }

    public string LogMessage { get; set; }

    public string PersonUsername { get; set; }

    public DateTime DateOfLog { get; set; }

    public bool Deleted { get; set; }
}
