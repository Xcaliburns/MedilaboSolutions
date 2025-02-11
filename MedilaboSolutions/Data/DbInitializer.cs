using MedilaboSolutionsBack1.Models;

namespace MedilaboSolutionsBack1.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Vérifie si la table Patients contient des données
            if (context.Patients.Any())
            {
                return; // La base de données est déjà initialisée
            }

            var patients = new Patient[]
            {
            new Patient
            {
                Prenom = "Test",
                Nom = "TestNone",
                DateDeNaissance = new DateTime(1966, 12, 31),
                Genre = "F",
                Adresse = "1 Brookside St",
                Telephone = "100-222-3333"
            },
            new Patient
            {
                Prenom = "Test",
                Nom = "TestBorderline",
                DateDeNaissance = new DateTime(1945, 06, 24),
                Genre = "M",
                Adresse = "2 High St",
                Telephone = "200-333-4444"
            },
            new Patient
            {
                Prenom = "Test",
                Nom = "TestInDanger",
                DateDeNaissance = new DateTime(2004, 06, 18),
                Genre = "M",
                Adresse = "3 Club Road",
                Telephone = "300-444-5555"
            },
            new Patient
            {
                Prenom = "Test",
                Nom = "TestEarlyOnset",
                DateDeNaissance = new DateTime(2002, 06, 28),
                Genre = "F",
                Adresse = "4 Valley Dr",
                Telephone = "400-555-6666"
            }
            };

            foreach (var p in patients)
            {
                context.Patients.Add(p);
            }

            context.SaveChanges();
        }
    }

}
