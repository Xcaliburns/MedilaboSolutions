using PatientNotes.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PatientNotes.Interfaces;

namespace PatientNotes.Services;

public class NotesService : INotesService
{
    private readonly INotesRepository _notesRepository;

    public NotesService(INotesRepository notesRepository)
    {
        _notesRepository = notesRepository;
    }

    //toutes les notes
    public async Task<List<Note>> GetAsync() =>
        await _notesRepository.GetAsync();
    // une note
    public async Task<Note> GetAsync(string id) =>
        await _notesRepository.GetAsync(id);
    // les notes d'un patient
    public async Task<List<Note>> GetByPatientId(int patientId) =>
        await _notesRepository.GetByPatientId(patientId);
    // ajouter une note
    public async Task CreateAsync(Note newNote) =>
        await _notesRepository.CreateAsync(newNote);
    // modifier une note
    public async Task UpdateAsync(string id, Note updatedNote) =>
        await _notesRepository.UpdateAsync(id, updatedNote);
    // supprimer une note
    public async Task RemoveAsync(string id) =>
        await _notesRepository.RemoveAsync(id);
}
