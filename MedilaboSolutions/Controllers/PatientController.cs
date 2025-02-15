using MedilaboSolutionsBack1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using MedilaboSolutionsBack1.Interfaces;
using Microsoft.AspNetCore.Authorization; 

namespace MedilaboSolutionsBack1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Organisateur,Praticien")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: api/Patient
        [HttpGet]
        public ActionResult<IEnumerable<Patient>> GetAllPatients()
        {
            //retrieve all patients from the database
            List<Patient> patients = _patientService.GetAllPatients();
            return Ok(patients);
        }

        // GET: api/Patient/5
        [HttpGet("{id}")]
        public ActionResult<Patient> GetPatientById(int id)
        {
            //retrieve the patient with the given id from the database
            Patient patient = _patientService.GetPatientById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return Ok(patient);
        }


     
        // PUT: api/Patient/Edit/5   
        [HttpPut("Edit/{id}")]
        public ActionResult Edit(int id, [FromBody] Patient updatedPatient)
        {
            try
            {
                // Validate that the ID in the URL matches the ID in the request body
                if (id != updatedPatient.Id)
                {
                    return BadRequest("ID in URL does not match ID in request body");
                }

                // Retrieve the patient with the given id from the database
                Patient patient = _patientService.GetPatientById(id);
                if (patient == null)
                {
                    return NotFound();
                }

                // Update patient properties based on the provided model
                patient.Nom = updatedPatient.Nom;
                patient.Prenom = updatedPatient.Prenom;
                patient.DateDeNaissance = updatedPatient.DateDeNaissance;
                patient.Genre = updatedPatient.Genre;
                patient.Adresse = updatedPatient.Adresse;
                patient.Telephone = updatedPatient.Telephone;

                // Update the patient in the database
                _patientService.UpdatePatient(patient);

                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        //// DELETE: api/Patient/Delete/5
        //[HttpDelete("{id}")]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        var patient = _patientService.GetPatientById(id);
        //        if (patient == null)
        //        {
        //            return NotFound();
        //        }

        //        _patientService.DeletePatient(id);
        //        return NoContent();
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}
    }
}
