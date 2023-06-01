using API.Models;

namespace API.Dtos
{
    public class NoteDto
    {
        public int NoteId { get; set; }

        public string NoteTitle { get; set; }

        public string NoteBody { get; set; }

        public int PersonId { get; set; }

        public DateTime PostingDate { get; set; }

        public PersonDto Person { get; set; }
    }
}
