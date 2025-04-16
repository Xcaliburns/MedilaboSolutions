using MedilaboSolutionsBack1.Data;
using MedilaboSolutionsBack1.Interfaces;
using MedilaboSolutionsBack1.Models;
using Microsoft.EntityFrameworkCore;


namespace MedilaboSolutionsBack1.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<PatientDto> GetAllPatients()
        {
            var patients = _context.Patients.Include(p => p.Adresse).ToList();

            return patients.Select(patient => new PatientDto
            {
                Id = patient.Id,
                Nom = patient.Nom,
                Prenom = patient.Prenom,
                Genre = patient.Genre,
                DateDeNaissance = patient.DateDeNaissance,
                Telephone = patient.Telephone,
                Adresse = patient.Adresse != null ? new AdresseDto { Libele = patient.Adresse.Libele } : null
            }).ToList();
        }


        public PatientDto? GetPatientById(int id) 
        {
            var patient = _context.Patients.Include(p => p.Adresse).FirstOrDefault(p => p.Id == id);

            return patient != null ? new PatientDto
            {
                Id = patient.Id,
                Nom = patient.Nom,
                Prenom = patient.Prenom,
                Genre = patient.Genre,
                DateDeNaissance = patient.DateDeNaissance,
                Telephone = patient.Telephone,
                Adresse = patient.Adresse != null ? new AdresseDto { Libele = patient.Adresse.Libele } : null
            } : null;
        }



        public void UpdatePatient(PatientDto patientDto)
        {
            var patient = _context.Patients.Include(p => p.Adresse).FirstOrDefault(p => p.Id == patientDto.Id);

            if (patient != null)
            {
                patient.Nom = patientDto.Nom;
                patient.Prenom = patientDto.Prenom;
                patient.DateDeNaissance = patientDto.DateDeNaissance;
                patient.Genre = patientDto.Genre;
                patient.Telephone = patientDto.Telephone;

                // Vérifier et mettre à jour l'adresse du patient
                if (patientDto.Adresse != null)
                {
                    var existingAdresse = _context.Adresses.FirstOrDefault(a => a.Libele == patientDto.Adresse.Libele);
                    if (existingAdresse != null)
                    {
                        patient.AdresseId = existingAdresse.Id;
                    }
                    else
                    {
                        var newAdresse = new Adresse { Libele = patientDto.Adresse.Libele };
                        _context.Adresses.Add(newAdresse);
                        _context.SaveChanges();
                        patient.AdresseId = newAdresse.Id;
                    }
                }

                _context.Patients.Update(patient);
                _context.SaveChanges();
            }
        }



        public void CreatePatient(PatientDto patientDto)
        {
            var patient = new Patient
            {
                Nom = patientDto.Nom,
                Prenom = patientDto.Prenom,
                Genre = patientDto.Genre,
                DateDeNaissance = patientDto.DateDeNaissance
            };

            // Ajouter le téléphone uniquement s'il est fourni
            if (!string.IsNullOrEmpty(patientDto.Telephone))
            {
                patient.Telephone = patientDto.Telephone;
            }

            // Vérifier si l'adresse existe avant de l'ajouter
            if (patientDto.Adresse != null && !string.IsNullOrEmpty(patientDto.Adresse.Libele))
            {
                var existingAdresse = _context.Adresses.FirstOrDefault(a => a.Libele == patientDto.Adresse.Libele);
                if (existingAdresse != null)
                {
                    patient.AdresseId = existingAdresse.Id;
                }
                else
                {
                    var newAdresse = new Adresse { Libele = patientDto.Adresse.Libele };
                    _context.Adresses.Add(newAdresse);
                    _context.SaveChanges();
                    patient.AdresseId = newAdresse.Id;
                }
            }

            _context.Patients.Add(patient);
            _context.SaveChanges();
        }


       


        public void CreateAdresse(Adresse adresse)
        {
            _context.Adresses.Add(adresse);
            _context.SaveChanges();
        }

        public Adresse GetAdresseByLibele(string libele)
        {
            return _context.Adresses.FirstOrDefault(a => a.Libele == libele);
        }

        public void UpdateAdresse(Adresse adresse)
        {
            var existingAdresse = _context.Adresses.FirstOrDefault(a => a.Id == adresse.Id);
    if (existingAdresse != null)
    {
        existingAdresse.Libele = adresse.Libele;
        _context.Adresses.Update(existingAdresse);
        _context.SaveChanges();
    }
        }


    }
}
