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

        public NoteController(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
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

            return res != null? Ok(res) : BadRequest("Internal Server Error");
        }

        [HttpPost("edit")]
        public ActionResult EditNote([FromBody] NoteDto note)
        {
            var res = _noteRepository.UpdateNote(note);

            return res != null ? Ok(res) : BadRequest("Internal Server Error");
        }

        [HttpPost("delete/{noteId}")]
        public ActionResult DeleteNote(int noteId)
        {
            _noteRepository.DeleteNote(noteId);

            return _noteRepository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }
    }
}
