using MongoDB.Driver;
using PatientNotes.Models;
using System.Collections.Generic;

public static class MongoDbInitializer
{
    public static void Initialize(IMongoDatabase database)
    {
        var notesCollection = database.GetCollection<Note>("PatientNotes");

        try
        {
            // Vérifier si la collection est vide avant d'insérer les données de test
            if (notesCollection.CountDocuments(_ => true) == 0)
            {
                var testNotes = new List<Note>
                {
                    new Note { PatientId = 1, PatientNote = "Aucun risque détecté." },
                    new Note { PatientId = 2, PatientNote = "Facteurs de risque modérés." },
                    new Note { PatientId = 3, PatientNote = "Situation à surveiller." },
                    new Note { PatientId = 4, PatientNote = "Signes précoces de diabète." }
                };

                notesCollection.InsertMany(testNotes);
                Console.WriteLine(" Données de test insérées avec succès !");
            }
        }
        catch (MongoCommandException ex)
        {
            Console.WriteLine($" Erreur MongoDB lors de l'insertion des notes : {ex.Message}");
        }
    }
}
