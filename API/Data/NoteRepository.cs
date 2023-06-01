using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class NoteRepository : INoteRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IMapper _mapper;

        public NoteRepository(InternShipAppSystemContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public NoteDto CreateNote(NoteDto note)
        {
            var noteToAdd = _mapper.Map<Note>(note);
            note.PostingDate = DateTime.Now;
            _context.Notes.Add(noteToAdd);

            return SaveAll() ? _mapper.Map<NoteDto>(noteToAdd) : null;
        }

        public void DeleteNote(int noteId)
        {
            var note = _mapper.Map<Note>(GetNoteById(noteId));
            note.Deleted = true;
            _context.SaveChanges();
        }

        public IEnumerable<NoteDto> GetAllNotes()
        {
            var allNotes = _context.Notes.Where(n => n.Deleted == false).Include(n => n.Person);
            return _mapper.Map<IEnumerable<NoteDto>>(allNotes);
        }

        public NoteDto GetNoteById(int noteId)
        {
            return GetAllNotes().FirstOrDefault(n=>n.NoteId == noteId);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public NoteDto UpdateNote(NoteDto note)
        {
            var noteToEdit = _mapper.Map<Note>(GetNoteById(note.NoteId));

            noteToEdit.NoteTitle = note.NoteTitle==null? noteToEdit.NoteTitle : note.NoteTitle;
            noteToEdit.NoteBody = note.NoteBody==null? noteToEdit.NoteBody : note.NoteBody;

            _context.Notes.Update(noteToEdit);

            return SaveAll() ? _mapper.Map<NoteDto>(noteToEdit) : null;
        }
    }
}
