using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Dtos
{
    public class GroupDto
    {
        public string Name { get; set; }
        public LoggedPersonDto Trainer { get; set; }
    }
}