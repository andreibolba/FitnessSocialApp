using API.Dtos;
using API.Interfaces.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class NoteController:BaseAPIController
    {
        private readonly INoteRepository _noteRepository;
        private readonly IPersonRepository _personRepository;

        public NoteController(INoteRepository noteRepository, IPersonRepository personRepository)
        {
            _noteRepository = noteRepository;
            _personRepository = personRepository;
        }

        [HttpGet]
        public ActionResult GetAllNotes()
        {
            return Ok(_noteRepository.GetAllNotes());
        }

        [HttpGet("{noteId}")]
        public ActionResult GetNoteById(int noteId)
        {
            return Ok(_noteRepository.GetNoteById(noteId));
        }

        [HttpPost("add")]
        public ActionResult AddNote([FromBody] NoteDto note)
        {
            var res = _noteRepository.CreateNote(note);
            if(res==null)
                return BadRequest("Internal Server Error");
            res.Person = _personRepository.GetPersonById(note.PersonId);
            return Ok(res);
        }

        [HttpPost("edit")]
        public ActionResult EditNote([FromBody] NoteDto note)
        {
            var res = _noteRepository.UpdateNote(note);
            if (res == null)
                return BadRequest("Internal Server Error");
            res.Person = _personRepository.GetPersonById(note.PersonId);
            return Ok(res);
        }

        [HttpPost("delete/{noteId}")]
        public ActionResult DeleteNote(int noteId)
        {
            _noteRepository.DeleteNote(noteId);

            return _noteRepository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }
    }
}
