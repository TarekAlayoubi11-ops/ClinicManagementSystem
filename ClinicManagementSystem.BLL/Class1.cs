using BCrypt.Net;
using ClincManagementSystem.DAL;
using ClincManagementSystem.DTOs;

namespace ClincManagementSystem.BLL
{
    public static class UserBll
    {
        public static OperationResult<UserDTO> GetUserByEmail(string Email, string connectionString)
        {
            return UserDal.GetUserByEmail(Email, connectionString);
        }
        public static OperationResult<int> CreateUser(CreateUserDTO dto, string connectionString)
        {
            var result = new OperationResult<int>();
            try
            {
                dto.Role = dto.Role!.ToLower();
                if (string.IsNullOrWhiteSpace(dto.Username) ||
                    string.IsNullOrWhiteSpace(dto.Password) ||
                    (dto.Role != "admin" && dto.Role != "teacher" && dto.Role != "student"))
                {
                    result.Success = false;
                    result.Message = "Invalid input data.";
                    return result;
                }
                dto.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                int code = UserDal.CreateUser(dto, connectionString);

                switch (code)
                {
                    case > 0:
                        result.Success = true;
                        result.Message = "User created successfully.";
                        result.Data = code;
                        break;

                    case -2:
                        result.Success = false;
                        result.Message = "Unable to create user. Please check your information.";
                        break;


                    default:
                        result.Success = false;
                        result.Message = "System error occurred.";
                        break;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
        public static OperationResult<bool> UpdateUser(UpdateUserDTO dto, string connectionString)
        {
            var result = new OperationResult<bool>();

            try
            {
                if (dto.UserId <= 0 ||
                    string.IsNullOrWhiteSpace(dto.Username) ||
                    string.IsNullOrWhiteSpace(dto.Role))
                {
                    result.Success = false;
                    result.Message = "Invalid input data.";
                    return result;
                }

                int code = UserDal.UpdateUser(dto, connectionString);

                switch (code)
                {
                    case 1:
                        result.Success = true;
                        result.Message = "User updated successfully.";
                        result.Data = true;
                        break;

                    case -2:
                        result.Success = false;
                        result.Message = "User not found.";
                        break;

                    case -4:
                        result.Success = false;
                        result.Message = "Unable to update user. Please check your information.";
                        break;

                    default:
                        result.Success = false;
                        result.Message = "System error occurred.";
                        break;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
        public static OperationResult<bool> DeleteUser(int userId, string connectionString)
        {
            var result = new OperationResult<bool>();
            try
            {
                if (userId <= 0)
                {
                    result.Success = false;
                    result.Message = "Invalid user id.";
                    return result;
                }

                int code = UserDal.DeleteUser(userId, connectionString);
                switch (code)
                {
                    case 1:
                        result.Success = true;
                        result.Message = "User deleted successfully.";
                        result.Data = true;
                        break;

                    case -1:
                        result.Success = false;
                        result.Message = "User not found.";
                        break;


                    default:
                        result.Success = false;
                        result.Message = "System error occurred.";
                        break;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }
            return result;
        }
        public static OperationResult<UserDTO> GetUserById(int userId, string connectionString)
        {
            return UserDal.GetUserById(userId, connectionString);
        }
        public static OperationResult<List<UserDTO>> GetAllUsers(string connectionString)
        {
            return UserDal.GetAllUsers(connectionString);
        }
    }
    public static class PatientService
    {
        public static int AddPatient(PatientDTO patient, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(patient.Name) ||
                patient.Age <= 0 ||
                string.IsNullOrWhiteSpace(patient.Phone) ||
                string.IsNullOrWhiteSpace(patient.Email))
            {
                return -1; // Invalid data
            }

            return PatientDal.AddPatient(patient, connectionString);
        }

        public static int DeletePatient(int id, string connectionString)
        {
            if (id <= 0) return -1;

            return PatientDal.DeletePatient(id, connectionString);
        }

        public static OperationResult<PatientDTO> GetPatientById(int id, string connectionString)
        {
            if (id <= 0)
                return new OperationResult<PatientDTO> { Success = false, Message = "Invalid patient id" };

            return PatientDal.GetPatientById(id, connectionString);
        }

        public static OperationResult<List<PatientDTO>> GetAllPatients(string connectionString)
        {
            return PatientDal.GetAllPatients(connectionString);
        }

        public static bool PatientExists(int id, string connectionString)
        {
            var patient = PatientDal.GetPatientById(id, connectionString);
            return patient.Success && patient.Data != null;
        }

        public static int UpdatePatient(int id, PatientDTO patient, string connectionString)
        {
            if (id <= 0 ||
                string.IsNullOrWhiteSpace(patient.Name) ||
                patient.Age <= 0 ||
                string.IsNullOrWhiteSpace(patient.Phone) ||
                string.IsNullOrWhiteSpace(patient.Email))
            {
                return -1; // Invalid data
            }

            return PatientDal.UpdatePatient(id, patient, connectionString);
        }
    }
    public static class InvoiceService
    {
        public static int AddInvoice(InvoiceDTO invoice, string connectionString)
        {
            return InvoiceDal.CreateInvoice(invoice, connectionString);
        }

        public static bool DeleteInvoice(int invoiceId, string connectionString)
        {
            return InvoiceDal.DeleteInvoice(invoiceId, connectionString);
        }

        public static InvoiceDTO? GetInvoiceById(int invoiceId, string connectionString)
        {
            return InvoiceDal.GetInvoiceById(invoiceId, connectionString);
        }

        public static List<InvoiceDTO> GetAllInvoices(string connectionString)
        {
            return InvoiceDal.GetAllInvoices(connectionString);
        }

        public static int UpdateInvoice(int invoiceId, InvoiceDTO invoice, string connectionString)
        {
            invoice.Id = invoiceId;
            return InvoiceDal.UpdateInvoice(invoice, connectionString);
        }

        public static bool InvoiceExists(int invoiceId, string connectionString)
        {
            return InvoiceDal.GetInvoiceById(invoiceId, connectionString) != null;
        }
    }
    public static class DoctorService
    {
        public static int AddDoctor(DoctorDTO doctor, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(doctor.Name) ||
                string.IsNullOrWhiteSpace(doctor.Email) ||
                string.IsNullOrWhiteSpace(doctor.Phone) ||
                string.IsNullOrWhiteSpace(doctor.Specialty))
                return -1;

            return DoctorDal.AddDoctor(doctor, connectionString);
        }

        public static int UpdateDoctor(int doctorId, DoctorDTO doctor, string connectionString)
        {
            if (string.IsNullOrWhiteSpace(doctor.Name) ||
                string.IsNullOrWhiteSpace(doctor.Email) ||
                string.IsNullOrWhiteSpace(doctor.Phone) ||
                string.IsNullOrWhiteSpace(doctor.Specialty))
                return -1;

            return DoctorDal.UpdateDoctor(doctorId, doctor, connectionString);
        }

        public static int DeleteDoctor(int doctorId, string connectionString)
        {
            return DoctorDal.DeleteDoctor(doctorId, connectionString);
        }

        public static DoctorDTO? GetDoctorById(int doctorId, string connectionString)
        {
            return DoctorDal.GetDoctorById(doctorId, connectionString);
        }

        public static List<DoctorDTO> GetAllDoctors(string connectionString)
        {
            return DoctorDal.GetAllDoctors(connectionString);
        }

        public static bool DoctorExists(int doctorId, string connectionString)
        {
            return DoctorDal.DoctorExists(doctorId, connectionString);
        }
    }
    public static class AppointmentService
    {
        public static int AddAppointment(AppointmentDTO appointment, string connectionString)
        {
            if (appointment.PatientId <= 0 || appointment.DoctorId <= 0 || appointment.AppointmentDate == default)
                return -1;

            return AppointmentDal.AddAppointment(appointment, connectionString);
        }

        public static int UpdateAppointment(int appointmentId, AppointmentDTO appointment, string connectionString)
        {
            if (appointment.PatientId <= 0)
                return -1;
            if (appointment.DoctorId <= 0)
                return -2;
            if (appointment.AppointmentDate == default)
                return -3;

            return AppointmentDal.UpdateAppointment(appointmentId, appointment, connectionString);
        }

        public static int DeleteAppointment(int appointmentId, string connectionString)
        {
            return AppointmentDal.DeleteAppointment(appointmentId, connectionString);
        }

        public static AppointmentDTO? GetAppointmentById(int appointmentId, string connectionString)
        {
            return AppointmentDal.GetAppointmentById(appointmentId, connectionString);
        }

        public static List<AppointmentDTO> GetAllAppointments(string connectionString)
        {
            return AppointmentDal.GetAllAppointments(connectionString);
        }

        public static bool AppointmentExists(int appointmentId, string connectionString)
        {
            return AppointmentDal.AppointmentExists(appointmentId, connectionString);
        }
    }
}



