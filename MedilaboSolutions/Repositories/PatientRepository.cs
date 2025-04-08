using MedilaboSolutionsBack1.Data;
using MedilaboSolutionsBack1.Interfaces;
using MedilaboSolutionsBack1.Models;


namespace MedilaboSolutionsBack1.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Patient> GetAllPatients()
        {
            return _context.Patients.ToList();
        }

        public Patient GetPatientById(int id)
        {
            return _context.Patients.FirstOrDefault(p => p.Id == id);
        }

        public void UpdatePatient(Patient patient)
        {
            _context.Patients.Update(patient);
            _context.SaveChanges();
        }

        public void CreatePatient(Patient patient) // Implémentation de la méthode CreatePatient
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();
        }
    }
}
