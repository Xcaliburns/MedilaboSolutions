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
            } : null;
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
                        patient.Adresse = new AdresseDto { Libele = existingAdresse.Libele };

                    }
                    else
                    {
                        var newAdresse = new Adresse { Libele = patientDto.Adresse.Libele };
                        _patientRepository.CreateAdresse(newAdresse);
                        patient.Adresse = new AdresseDto { Libele = newAdresse.Libele };
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
                DateDeNaissance = patientDto.DateDeNaissance,
                Telephone = patientDto.Telephone
            };

            if (patientDto.Adresse != null)
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

            _patientRepository.CreatePatient(patientDto);
        }

    }
}
