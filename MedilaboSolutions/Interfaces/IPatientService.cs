using MedilaboSolutionsBack1.Models;
using System.Collections.Generic;

namespace MedilaboSolutionsBack1.Interfaces
{
    public interface IPatientService
    {
        List <Patient> GetAllPatients();
        Patient GetPatientById(int id);
        void UpdatePatient(Patient patient);
        void CreatePatient(Patient patient); // Ajout de la méthode CreatePatient
    }
}
