using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClinicManagementSystem.BusinessLogic;
using ClinicApp.DTO;
using System.Numerics;
namespace ClinicManagementSystem.API.Controllers
{
    [Route("api/Doctors")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        [HttpGet("Doctors")]
        public ActionResult<IEnumerable<DoctorDTO>> GetAllDoctors()
        {
            var doctors = DoctorService.GetAllDoctors();
            if ( doctors.Count == 0)
            {
                return NotFound(new { message = "No doctors found" });
            }
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public ActionResult<DoctorDTO> GetDoctorById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid doctor id" });
            }
            var doctor = DoctorService.GetDoctorById(id);
            if (doctor == null)
                return NotFound(new { message = "Doctor not found" });

            return Ok(doctor);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteDoctor(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid doctor id" });
            }
            if (!DoctorService.DoctorExists(id))
                return NotFound(new { message = "Doctor not found" });

            int result = DoctorService.DeleteDoctor(id);
            if (result == -99)
                return StatusCode(500, new { message = "Server error" });
            if (result == 0)
                return BadRequest(new { message = "Cannot delete doctor (has dependencies?)" });

            return Ok(new { message = "Doctor deleted successfully" });
        }

        [HttpPost]
        public ActionResult Add(DoctorDTO doctor)
        {
            int result = DoctorService.AddDoctor(doctor);

            if (result == -1)
                return BadRequest(new { message = "Invalid doctor data" });

            if (result == -99)
                return StatusCode(500, new { message = "Server error" });

            return Ok(new { message = "Doctor added successfully", id = result });
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, DoctorDTO doctor)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid doctor id" });

            if (!DoctorService.DoctorExists(id))
                return NotFound(new { message = "Doctor not found" });

            int result = DoctorService.UpdateDoctor(id, doctor);

            if (result == -1)
                return BadRequest(new { message = "Invalid doctor data" });

            if (result == -99)
                return StatusCode(500, new { message = "Server error" });
    
            return Ok(new { message = "Doctor updated successfully" });
        }

    }
}
