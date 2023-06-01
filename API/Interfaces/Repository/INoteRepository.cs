using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface INoteRepository
    {
        public NoteDto CreateNote(NoteDto note);
        public NoteDto UpdateNote(NoteDto note);
        public NoteDto DeleteNote(int noteId);
        public IEnumerable<NoteDto> GetAll();
        public NoteDto GetNote(int noteId);
        public bool SaveAll();
    }
}
