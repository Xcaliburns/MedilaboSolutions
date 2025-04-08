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
                    new Note { PatientId = 1, PatientNote = "Le patient déclare qu'il 'se sent très bien' Poids égal ou inférieur au poids recommandé" },
                    new Note { PatientId = 2, PatientNote = "Le patient déclare qu'il ressent beaucoup de stress au travail Il se plaint également que son audition est anormale dernièrement" },
                    new Note { PatientId = 2, PatientNote = "Le patient déclare avoir fait une réaction aux médicaments au cours des 3 derniers mois Il remarque également que son audition continue d'être anormale" },
                    new Note { PatientId = 3, PatientNote = "Le patient déclare qu'il fume depuis peu" },
                    new Note { PatientId = 3, PatientNote = "Le patient déclare qu'il est fumeur et qu'il a cessé de fumer l'année dernière Il se plaint également de crises d’apnée respiratoire anormales Tests de laboratoire indiquant un taux de cholestérol LDL élevé" },
                    new Note { PatientId = 4, PatientNote = "Le patient déclare qu'il lui est devenu difficile de monter les escaliers Il se plaint également d’être essoufflé Tests de laboratoire indiquant que les anticorps sont élevés Réaction aux médicaments" },
                    new Note { PatientId = 4, PatientNote = "Le patient déclare qu'il a mal au dos lorsqu'il reste assis pendant longtemps" },
                    new Note { PatientId = 4, PatientNote = "Le patient déclare avoir commencé à fumer depuis peu Hémoglobine A1C supérieure au niveau recommandé" },
                    new Note { PatientId = 4, PatientNote = "Taille, Poids, Cholestérol, Vertige et Réaction" }
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
