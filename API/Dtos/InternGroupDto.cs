using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class InternGroupDto
    {
        public LoggedPersonDto Intern { get; set; }
        public int InternId { get; set; }
        public bool IsChecked { get; set; }
    }
}