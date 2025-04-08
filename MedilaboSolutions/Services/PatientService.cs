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

        public List<Patient> GetAllPatients()
        {
            return _patientRepository.GetAllPatients();
        }

        public Patient GetPatientById(int id)
        {
            return _patientRepository.GetPatientById(id);
        }

        public void UpdatePatient(Patient patient)
        {
            _patientRepository.UpdatePatient(patient);
        }

        public void CreatePatient(Patient patient) // Implémentation de la méthode CreatePatient
        {
            _patientRepository.CreatePatient(patient);
        }
    }
}
