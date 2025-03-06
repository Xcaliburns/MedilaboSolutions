using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientNotes.Models;
using PatientNotes.Services;


namespace PatientNotes.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Praticien")]
    public class NoteController : ControllerBase
    {
        private readonly NotesService _notesService;

        public NoteController(NotesService notesService)
        {
            _notesService = notesService;
        }

        //[HttpGet]
        //public async Task<ActionResult<List<Note>>> GetAsync()
        //{
        //    var notes = await _notesService.GetAsync();
        //    return notes;
        //}

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Note>> GetAsync(string id)
        //{
        //    var note = await _notesService.GetAsync(id);
        //    if (note == null)
        //    {
        //        return new NotFoundResult();
        //    }
        //    return note;
        //}

        [HttpGet("patient/{PatientId}")]
        public async Task<ActionResult<List<Note>>> GetByPatientId(int PatientId)
        {
            var notes = await _notesService.GetByPatientId(PatientId);
            return notes;
        }

        //[HttpPost]
        //public async Task<ActionResult<Note>> CreateAsync(Note newNote)
        //{
        //    await _notesService.CreateAsync(newNote);
        //    return newNote;
        //}

        //[HttpPut("{id}")]
        //public async Task<ActionResult<Note>> UpdateAsync(string id, Note updatedNote)
        //{
        //    var note = await _notesService.GetAsync(id);
        //    if (note == null)
        //    {
        //        return new NotFoundResult();
        //    }
        //    await _notesService.UpdateAsync(id, updatedNote);
        //    return updatedNote;
        //}

        //[HttpDelete("{id}")]
        //public async Task<ActionResult> RemoveAsync(string id)
        //{
        //    var note = await _notesService.GetAsync(id);
        //    if (note == null)
        //    {
        //        return new NotFoundResult();
        //    }
        //    await _notesService.RemoveAsync(id);
        //    return new OkResult();
        //}
    }
}
