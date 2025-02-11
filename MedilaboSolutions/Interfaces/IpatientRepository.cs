﻿using MedilaboSolutionsBack1.Models;
using System.Collections.Generic;

namespace MedilaboSolutionsBack1.Interfaces
{
    public interface IPatientRepository
    {
        List<Patient> GetAllPatients();
        Patient GetPatientById(int id);
        void UpdatePatient(Patient patient);
    }
}
