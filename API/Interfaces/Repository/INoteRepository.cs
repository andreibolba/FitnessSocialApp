using API.Dtos;

namespace API.Interfaces.Repository
{
    public interface INoteRepository
    {
        public NoteDto CreateNote(NoteDto note);
        public NoteDto UpdateNote(NoteDto note);
        public void DeleteNote(int noteId);
        public IEnumerable<NoteDto> GetAllNotes();
        public NoteDto GetNoteById(int noteId);
        public bool SaveAll();
    }
}
