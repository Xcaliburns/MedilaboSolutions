using MedilaboSolutionsBack1.Models;

public interface IPatientRepository
{
    List<PatientDto> GetAllPatients();
    PatientDto GetPatientById(int id);
    void UpdatePatient(PatientDto patient);
    void CreatePatient(PatientDto patient);

    // Ajout de la méthode pour créer une adresse
    void CreateAdresse(Adresse adresse);

    // Une méthode pour récupérer une adresse par son Libellé
    Adresse GetAdresseByLibele(string libele);
}
