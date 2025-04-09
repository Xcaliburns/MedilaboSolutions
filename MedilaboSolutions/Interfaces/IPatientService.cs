using MedilaboSolutionsBack1.Models;

namespace MedilaboSolutionsBack1.Interfaces
{
    public interface IPatientService
    {
        List<PatientDto> GetAllPatients();
        PatientDto GetPatientById(int id);
        void UpdatePatient(PatientDto patient);
        void CreatePatient(PatientDto patient);
    }
}
