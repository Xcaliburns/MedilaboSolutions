using PatientNotes.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace PatientNotes.Services;

public class NotesService
{
    private readonly IMongoCollection<Note> _notesCollection;

    public NotesService(
        IOptions<PatientNotesDatabaseSettings> patientNotesDatabaseSettings)
    {
        var mongoClient = new MongoClient(
            patientNotesDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            patientNotesDatabaseSettings.Value.DatabaseName);

        _notesCollection = mongoDatabase.GetCollection<Note>(
            patientNotesDatabaseSettings.Value.CollectionName);
    }

    public async Task<List<Note>> GetAsync() =>
        await _notesCollection.Find(_ => true).ToListAsync();

    public async Task<Note> GetAsync(string id) =>
        await _notesCollection.Find(x => x._id == id).FirstOrDefaultAsync();

    public async Task<List<Note>> GetByPatientId(int patientId) =>
        await _notesCollection.Find(x => x.PatientId == patientId).ToListAsync();

    public async Task CreateAsync(Note newNote) =>
        await _notesCollection.InsertOneAsync(newNote);

    public async Task UpdateAsync(string id, Note updatedNote) =>
        await _notesCollection.ReplaceOneAsync(x => x._id == id, updatedNote);

    public async Task RemoveAsync(string id) =>
        await _notesCollection.DeleteOneAsync(x => x._id == id);
}