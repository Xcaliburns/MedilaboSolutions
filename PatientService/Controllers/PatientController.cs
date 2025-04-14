using MedilaboSolutionsBack1.Interfaces;
using MedilaboSolutionsBack1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IEnumerable<PatientDto>> GetAllPatients()
        {
            var patientsDto = _patientService.GetAllPatients();
            return Ok(patientsDto);
        }


        // GET: api/Patient/5
        [HttpGet("{id}")]
        public ActionResult<PatientDto> GetPatientById(int id)
        {
            var patientDto = _patientService.GetPatientById(id);
            if (patientDto == null)
                return NotFound();
            return Ok(patientDto);
        }



        // POST: api/Patient/Create
        [HttpPost("Create")]
        public ActionResult Create([FromBody] PatientDto patientDto)
        {
            try
            {
                _patientService.CreatePatient(patientDto);
                return Ok(patientDto);
            }
            catch
            {
                return BadRequest();
            }
        }


        // PUT: api/Patient/Edit/5   
        [HttpPut("Edit/{id}")]
        public ActionResult Edit(int id, [FromBody] PatientDto updatedPatientDto)
        {
            if (id != updatedPatientDto.Id)
                return BadRequest("ID mismatch");

            var patient = _patientService.GetPatientById(id);
            if (patient == null)
                return NotFound();

            _patientService.UpdatePatient(updatedPatientDto);
            return NoContent();
        }

    }
}

