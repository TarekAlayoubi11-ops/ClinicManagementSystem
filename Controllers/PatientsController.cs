using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClinicManagementSystem.BusinessLogic;
using ClinicApp.DTO;
using System.Numerics;
namespace ClinicManagementSystem.API.Controllers
{
    [Route("api/Patients")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        [HttpGet("Patients")]
        public ActionResult<IEnumerable<PatientDTO>> GetAllPatients()
        {
            var patients = PatientService.GetAllPatients();
            if (patients == null || patients.Count == 0)
            {
                return NotFound(new { message = "No patients found" });
            }
            return Ok(patients);
        }
    
        [HttpGet("{id}")]
        public ActionResult<PatientDTO> GetById(int id)
        {
            if (id <= 0) 
            {
                return BadRequest(new { message = "Invalid patient id" });
            }
            var patient = PatientService.GetPatientById(id);
            if (patient == null)
                return NotFound(new { message = "Patient not found" });

            return Ok(patient);
        }
   
        [HttpDelete("{id}")]
        public ActionResult DeletePatient(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid patient id" });
            }
            if (!PatientService.PatientExists(id))
                return NotFound(new { message = "Patient not found" });

            int result = PatientService.DeletePatient(id);
            if (result == -99)
                return StatusCode(500, new { message = "Server error" });
            if (result == 0)
                return BadRequest(new { message = "Cannot delete patient (has dependencies?)" });

            return Ok(new { message = "Patient deleted successfully" });
        }

        [HttpPost]
        public ActionResult Add(PatientDTO patient)
        {
            int result = PatientService.AddPatient(patient);

            if (result == -1)
                return BadRequest(new { message = "Invalid patient data" });

            if (result == -99)
                return StatusCode(500, new { message = "Server error" });

            return Ok(new { message = "Patient added successfully", id = result });
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id,PatientDTO patient)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid patient id" });

            if (!PatientService.PatientExists(id))
                return NotFound(new { message = "Patient not found" });

            int result = PatientService.UpdatePatient(id, patient);

            if (result == -1)
                return BadRequest(new { message = "Invalid patient data" });

            if (result == -99)
                return StatusCode(500, new { message = "Server error" });

            return Ok(new { message = "Patient updated successfully" });
        }
    }
}
