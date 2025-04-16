using MedilaboSolutionsBack1.Models;

public interface IPatientRepository
{
    List<PatientDto> GetAllPatients();
    PatientDto GetPatientById(int id);
    void UpdatePatient(PatientDto patient);
    void CreatePatient(PatientDto patient);    
    void CreateAdresse(Adresse adresse);
    void UpdateAdresse(Adresse adresse);   
    Adresse GetAdresseByLibele(string libele);
}
