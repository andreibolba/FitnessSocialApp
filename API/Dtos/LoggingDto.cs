using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class LoggingDto
    {
    public string LogType { get; set; }

    public string LogMessage { get; set; }

    public string PersonUsername { get; set; }

    public DateTime DateOfLog { get; set; }
    }
}