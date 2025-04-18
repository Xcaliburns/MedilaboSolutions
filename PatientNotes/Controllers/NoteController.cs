﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientNotes.Interfaces;
using PatientNotes.Models;


namespace PatientNotes.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Praticien")]
    public class NoteController : ControllerBase
    {
        private readonly INotesService _notesService;

        public NoteController(INotesService notesService)
        {
            _notesService = notesService;
        }

        [HttpGet("patient/{PatientId}")]
        public async Task<ActionResult<List<Note>>> GetByPatientId(int PatientId)
        {
            try
            {
                var notes = await _notesService.GetByPatientId(PatientId);

                if (notes == null || notes.Count == 0)
                {
                    notes = new List<Note>();
                }

                Console.WriteLine($"Notes trouvées: {notes.Count}");
                return Ok(notes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur interne du serveur : {ex.Message}");
            }
        }

        
        [HttpPost]
        public async Task<ActionResult<Note>> CreateAsync(NoteRequest newNoteRequest)
        {
            if (newNoteRequest == null)
            {
                return BadRequest("Note is null.");
            }

            var newNote = new Note
            {
                PatientId = newNoteRequest.PatientId,
                PatientNote = newNoteRequest.PatientNote
            };
            await _notesService.CreateAsync(newNote);
            return newNote;
        }
     

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveAsync(string id)
        {
            var note = await _notesService.GetAsync(id);
            if (note == null)
            {
                return new NotFoundResult();
            }
            await _notesService.RemoveAsync(id);
            return new OkResult();
        }
    }
}
