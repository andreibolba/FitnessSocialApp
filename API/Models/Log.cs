using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Log
{
    public int LogId { get; set; }

    public string LogLevel { get; set; } = null!;

    public DateTime Message { get; set; }

    public bool Deleted { get; set; }
}
