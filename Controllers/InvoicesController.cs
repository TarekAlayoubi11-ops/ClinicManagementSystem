using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClinicManagementSystem.BusinessLogic;
using ClinicApp.DTO;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ClinicManagementSystem.API.Controllers
{
    [Route("api/Invoices")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        [HttpGet("Invoices")]
        public ActionResult<IEnumerable<InvoiceDTO>> GetAllInvoices()
        {
            var invoices = InvoiceService.GetAllInvoices();
            if (invoices.Count == 0)
                return NotFound(new { message = "No invoices found" });

            return Ok(invoices);
        }

        [HttpGet("{id}")]
        public ActionResult<InvoiceDTO> GetInvoiceById(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid invoice id" });
            }
            var invoice = InvoiceService.GetInvoiceById(id);
            if (invoice == null)
                return NotFound(new { message = "Invoice not found" });

            return Ok(invoice);
        }

        [HttpPost]
        public ActionResult AddInvoice(InvoiceDTO invoice)
        {
            int result = InvoiceService.AddInvoice(invoice);

            return result switch
            {
                -1 => BadRequest(new { message = "Invalid amount" }),
                -2 => BadRequest(new { message = "Appointment not found" }),
                -3 => BadRequest(new { message = "Invoice already exists for this appointment and date" }),
                -99 => StatusCode(500, new { message = "Server error" }),
                _ => Ok(new { message = "Invoice added successfully", id = result })
            };
        }

        [HttpPut("{id}")]
        public ActionResult UpdateInvoice(int invoiceId, InvoiceDTO invoice)
        {
            if (invoiceId <= 0)
            {
                return BadRequest(new { message = "Invalid invoice id" });
            }
            if (!InvoiceService.InvoiceExists(invoiceId))
                return NotFound(new { message = "Invoice not found" });

            int result = InvoiceService.UpdateInvoice(invoiceId, invoice);

            return result switch
            {
                -1 => BadRequest(new { message = "Invalid amount" }),
                -2 => BadRequest(new { message = "Invalid appointment id" }),
                -3 => BadRequest(new { message = "Appointment not found" }),
                -4 => BadRequest(new { message = "Duplicate invoice for this appointment and date" }),
                -99 => StatusCode(500, new { message = "Server error" }),
                _ => Ok(new { message = "Invoice updated successfully" })
            };
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteInvoice(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid invoice id" });
            }
            bool result = InvoiceService.DeleteInvoice(id);

            if (!result)
                return BadRequest(new { message = "Cannot delete invoice" });

            return Ok(new { message = "Invoice deleted successfully" });
        }
    }
}
