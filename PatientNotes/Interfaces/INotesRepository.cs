using PatientNotes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PatientNotes.Interfaces
{
    public interface INotesRepository
    {
        Task<List<Note>> GetAsync();
        Task<Note> GetAsync(string id);
        Task<List<Note>> GetByPatientId(int patientId);
        Task CreateAsync(Note newNote);
        Task UpdateAsync(string id, Note updatedNote);
        Task RemoveAsync(string id);
    }
}
