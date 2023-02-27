using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;

namespace API.Dtos
{
    public class GroupDto
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public PersonDto Trainer { get; set; }
        public int TrainerId { get; set; }
        public List<PersonDto> AllInterns {get;set;}
    }
}