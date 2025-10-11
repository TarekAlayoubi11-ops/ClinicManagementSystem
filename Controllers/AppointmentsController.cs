using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClinicManagementSystem.BusinessLogic;
using ClinicApp.DTO;


namespace ClinicManagementSystem.API.Controllers
{
    [Route("api/Appointments")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        [HttpGet("Appointments")]
        public ActionResult<IEnumerable<AppointmentDTO>> GetAllAppointments()
        {
            var appointments = AppointmentService.GetAllAppointments();
            if (appointments.Count == 0)
                return NotFound(new { message = "No appointments found" });

            return Ok(appointments);
        }

        [HttpPost]
        public ActionResult AddAppointment(AppointmentDTO appointment)
        {
            int result = AppointmentService.AddAppointment(appointment);

            if (result == -1)
                return BadRequest(new { message = "Invalid appointment data" });
            if (result == -99)
                return StatusCode(500, new { message = "Server error" });

            return Ok(new { message = "Appointment added successfully", id = result });
        }

        [HttpPut("{id}")]
        public ActionResult UpdateAppointment(int id, AppointmentDTO appointment)
        {
            if (id <= 0)
                return BadRequest(new { message = "Invalid appointment id" });
            
            if (!AppointmentService.AppointmentExists(id))
                return NotFound(new { message = "Appointment not found" });

            int result = AppointmentService.UpdateAppointment(id, appointment);

            return result switch
            {
                -1 => NotFound(new { message = "Patient not found" }),
                -2 => NotFound(new { message = "Doctor not found" }),
                -3 => BadRequest(new { message = "Doctor already has an appointment at this date" }),
                -99 => StatusCode(500, new { message = "Server error" }),
                0 => BadRequest(new { message = "No rows updated" }),
                _ => Ok(new { message = "Appointment updated successfully" }),
            };
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteAppointment(int id)
        {
            if (id<=0)
                return BadRequest(new { message = "Invalid appointment id" });
            if (!AppointmentService.AppointmentExists(id))
                return NotFound(new { message = "Appointment not found" });

            int result = AppointmentService.DeleteAppointment(id);
            if (result == -99)
                return StatusCode(500, new { message = "Server error" });
            if (result == 0)
                return BadRequest(new { message = "Cannot delete appointment (has dependencies?)" });

            return Ok(new { message = "Appointment deleted successfully" });
        }
    }
}

