using PatientNotes.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using PatientNotes.Interfaces;


namespace PatientNotes.Repository;



public class NotesRepository : INotesRepository
{
    private readonly IMongoCollection<Note> _notesCollection;

    public NotesRepository(IOptions<PatientNotesDatabaseSettings> patientNotesDatabaseSettings)
    {
        if (patientNotesDatabaseSettings?.Value == null || string.IsNullOrEmpty(patientNotesDatabaseSettings.Value.ConnectionString))
        {
            throw new InvalidOperationException(" `patientNotesDatabaseSettings.Value.ConnectionString` est NULL dans NotesRepository. Vérifie l'injection des paramètres.");
        }

        Console.WriteLine($" ConnectionString dans NotesRepository: {patientNotesDatabaseSettings.Value.ConnectionString}");

        var mongoClient = new MongoClient(patientNotesDatabaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(patientNotesDatabaseSettings.Value.DatabaseName);
        _notesCollection = mongoDatabase.GetCollection<Note>(patientNotesDatabaseSettings.Value.CollectionName);
    }


    //toutes les notes
    public async Task<List<Note>> GetAsync() =>
        await _notesCollection.Find(_ => true).ToListAsync();

    // une note
    public async Task<Note> GetAsync(string id) =>
        await _notesCollection.Find(x => x._id == id).FirstOrDefaultAsync();

    public async Task<List<Note>> GetByPatientId(int patientId)
    {
        try
        {
            Console.WriteLine($" Recherche des notes pour PatientID {patientId}...");
            var result = await _notesCollection.Find(x => x.PatientId == patientId).ToListAsync();

            if (result == null || result.Count == 0)
            {
                Console.WriteLine($" Aucune note trouvée pour PatientID {patientId} !");
            }
            else
            {
                Console.WriteLine($" Notes trouvées: {result.Count} notes récupérées.");
            }

            return result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Erreur MongoDB lors de la récupération des notes: {ex.Message}");
            throw;
        }
    }


    // ajouter une note
    public async Task CreateAsync(Note newNote) =>
        await _notesCollection.InsertOneAsync(newNote);

    // modifier une note
    public async Task UpdateAsync(string id, Note updatedNote) =>
        await _notesCollection.ReplaceOneAsync(x => x._id == id, updatedNote);

    // supprimer une note
    public async Task RemoveAsync(string id) =>
        await _notesCollection.DeleteOneAsync(x => x._id == id);
}

