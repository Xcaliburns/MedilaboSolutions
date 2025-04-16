using MedilaboSolutionsBack1.Interfaces;
using MedilaboSolutionsBack1.Models;

namespace MedilaboSolutionsBack1.Services
{
    public class PatientService :IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

       public List<PatientDto> GetAllPatients()
{
    var patients = _patientRepository.GetAllPatients();
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


        public PatientDto GetPatientById(int id)
        {
            var patient = _patientRepository.GetPatientById(id);
            return patient != null ? new PatientDto
            {
                Id = patient.Id,
                Nom = patient.Nom,
                Prenom = patient.Prenom,
                Genre = patient.Genre,
                DateDeNaissance = patient.DateDeNaissance,
                Telephone = patient.Telephone,
                Adresse = patient.Adresse != null ? new AdresseDto { Libele = patient.Adresse.Libele } : null
            } : new PatientDto(); 
        }

        public void UpdatePatient(PatientDto patientDto)
        {
            var patient = _patientRepository.GetPatientById(patientDto.Id);
            if (patient != null)
            {
                patient.Nom = patientDto.Nom;
                patient.Prenom = patientDto.Prenom;
                patient.Genre = patientDto.Genre;
                patient.DateDeNaissance = patientDto.DateDeNaissance;
                patient.Telephone = patientDto.Telephone;

                if (patientDto.Adresse != null)
                {
                    var existingAdresse = _patientRepository.GetAdresseByLibele(patientDto.Adresse.Libele);
                    if (existingAdresse != null)
                    {
                        // Update the existing address
                        existingAdresse.Libele = patientDto.Adresse.Libele;
                        _patientRepository.UpdateAdresse(existingAdresse);
                        patient.Adresse = new AdresseDto { Libele = existingAdresse.Libele ?? string.Empty };
                    }
                    else
                    {
                        // Create a new address if it doesn't exist
                        var newAdresse = new Adresse { Libele = patientDto.Adresse.Libele };
                        _patientRepository.CreateAdresse(newAdresse);
                        patient.Adresse = new AdresseDto { Libele = newAdresse.Libele ?? string.Empty };
                    }
                }

                _patientRepository.UpdatePatient(patient);
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

    // Ajout du téléphone uniquement s'il est fourni
    if (!string.IsNullOrEmpty(patientDto.Telephone))
    {
        patient.Telephone = patientDto.Telephone;
    }

    // Gestion de l'adresse uniquement si elle est fournie
    if (patientDto.Adresse != null && !string.IsNullOrEmpty(patientDto.Adresse.Libele))
    {
        var existingAdresse = _patientRepository.GetAdresseByLibele(patientDto.Adresse.Libele);
        if (existingAdresse != null)
        {
            patient.AdresseId = existingAdresse.Id;
        }
        else
        {
            var newAdresse = new Adresse { Libele = patientDto.Adresse.Libele };
            _patientRepository.CreateAdresse(newAdresse);
            patient.AdresseId = newAdresse.Id;
        }
    }

    // Création du patient via le repository
    _patientRepository.CreatePatient(patientDto);
}


    }
}
