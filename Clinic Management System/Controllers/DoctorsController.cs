using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClinicManagementSystem.DTOs;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using ClincManagementSystem.DTOs;
using ClincManagementSystem.BLL;
namespace ClinicManagementSystem.API.Controllers
{
    [Authorize]
    [Route("api/Doctors")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly string _connectionString;
        public DoctorsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string 'DefaultConnection' not found.");
            ;
        }



        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet("Doctors")]
        public ActionResult<IEnumerable<DoctorDTO>> GetAllDoctors()
        {
            var doctors = DoctorService.GetAllDoctors(_connectionString);
            if (doctors.Count == 0)
            {
                return NotFound(new { message = "No doctors found" });
            }
            return Ok(doctors);
        }



        [Authorize(Roles = "Admin,Doctor,Patient")]
        [HttpGet("{id}")]
        public ActionResult<DoctorDTO> GetDoctorById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid doctor id" });
            }
            var doctor = DoctorService.GetDoctorById(id, _connectionString);
            if (doctor == null)
                return NotFound(new { message = "Doctor not found" });

            return Ok(doctor);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult DeleteDoctor(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid doctor id" });
            }
            if (!DoctorService.DoctorExists(id, _connectionString))
                return NotFound(new { message = "Doctor not found" });

            int result = DoctorService.DeleteDoctor(id, _connectionString);
            if (result == -99)
                return StatusCode(500, new { message = "Server error" });
            if (result == 0)
                return BadRequest(new { message = "Cannot delete doctor (has dependencies?)" });

            return Ok(new { message = "Doctor deleted successfully" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Add(DoctorDTO doctor)
        {
            int result = DoctorService.AddDoctor(doctor, _connectionString);

            if (result == -1)
                return BadRequest(new { message = "Invalid doctor data" });

            if (result == -99)
                return StatusCode(500, new { message = "Server error" });

            return Ok(new { message = "Doctor added successfully", id = result });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public ActionResult Update(int id, DoctorDTO doctor)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid doctor id" });

            if (!DoctorService.DoctorExists(id, _connectionString))
                return NotFound(new { message = "Doctor not found" });

            int result = DoctorService.UpdateDoctor(id, doctor, _connectionString);

            if (result == -1)
                return BadRequest(new { message = "Invalid doctor data" });

            if (result == -99)
                return StatusCode(500, new { message = "Server error" });

            return Ok(new { message = "Doctor updated successfully" });
        }

    }
}
