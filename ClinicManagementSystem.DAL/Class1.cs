using ClincManagementSystem.DTOs;
using System.Data;
using Microsoft.Data.SqlClient;

namespace ClincManagementSystem.DAL
{
    public static class UserDal
    {
        public static OperationResult<UserDTO> GetUserByEmail(string Email, string connectionString)
        {
            var result = new OperationResult<UserDTO>();
            result.Data = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetUserByEmail", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", Email);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Data = new UserDTO
                            {
                                Username = reader["FullName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Role = reader["Role"].ToString(),
                                UserId = (int)reader["UserId"],
                                IsActive = (bool)reader["IsActive"],
                                CreatedAt = (DateTime)reader["CreatedAt"]
                            };
                            result.Success = true;
                            result.Message = "User found successfully.";
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = "User not found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }
            return result;
        }
        public static int CreateUser(CreateUserDTO user, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_CreateUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FullName", user.Username);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.Password);
                    cmd.Parameters.AddWithValue("@Role", user.Role);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static int UpdateUser(UpdateUserDTO user, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_UpdateUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", user.UserId);
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Role", user.Role);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static int DeleteUser(int userId, string connectionString)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_DeleteUser", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    return Convert.ToInt32(result);
                }
            }
            catch
            {
                return -99;
            }
        }
        public static OperationResult<UserDTO> GetUserById(int userId, string connectionString)
        {
            var result = new OperationResult<UserDTO>();
            result.Data = null;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetUserById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result.Data = new UserDTO
                            {
                                UserId = (int)reader["UserId"],
                                Username = reader["FullName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Role = reader["Role"].ToString(),
                                IsActive = (bool)reader["IsActive"],
                                CreatedAt = (DateTime)reader["CreatedAt"]
                            };
                            result.Success = true;
                            result.Message = "Teacher found successfully.";
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = "Teacher not found.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }
            return result;
        }
        public static OperationResult<List<UserDTO>> GetAllUsers(string connectionString)
        {
            var result = new OperationResult<List<UserDTO>>()
            {
                Data = new List<UserDTO>()
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("sp_GetAllActiveUsers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Data.Add(new UserDTO
                            {
                                UserId = (int)reader["UserId"],
                                Username = reader["FullName"].ToString(),
                                Email = reader["Email"].ToString(),
                                Role = reader["Role"].ToString(),
                                IsActive = (bool)reader["IsActive"],
                                CreatedAt = (DateTime)reader["CreatedAt"]
                            });
                        }
                    }
                }

                if (result.Data.Count > 0)
                {
                    result.Success = true;
                    result.Message = "Teachers retrieved successfully.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "No teachers found.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }
    }
    public static class PatientDal
    {
        public static int AddPatient(PatientDTO patient, string connectionString)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("sp_AddPatient", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Name", patient.Name);
                cmd.Parameters.AddWithValue("@Age", patient.Age);
                cmd.Parameters.AddWithValue("@Phone", patient.Phone);
                cmd.Parameters.AddWithValue("@Email", patient.Email);
                cmd.Parameters.AddWithValue("@Address", patient.Address);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
            catch
            {
                return -99;
            }
        }

        public static int DeletePatient(int id, string connectionString)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("sp_DeletePatientWithCheck", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
            catch
            {
                return -99;
            }
        }

        public static OperationResult<PatientDTO> GetPatientById(int id, string connectionString)
        {
            var result = new OperationResult<PatientDTO>() { Data = null };
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("sp_FindPatientById", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result.Data = new PatientDTO
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Age = (int)reader["Age"],
                        Phone = reader["Phone"].ToString(),
                        Email = reader["Email"].ToString(),
                        Address = reader["Address"].ToString()
                    };
                    result.Success = true;
                    result.Message = "Patient found successfully.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Patient not found.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }
            return result;
        }

        public static OperationResult<List<PatientDTO>> GetAllPatients(string connectionString)
        {
            var result = new OperationResult<List<PatientDTO>>() { Data = new List<PatientDTO>() };
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("Sp_GetAllPatients", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Data.Add(new PatientDTO
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Age = (int)reader["Age"],
                        Phone = reader["Phone"].ToString(),
                        Email = reader["Email"].ToString(),
                        Address = reader["Address"].ToString()
                    });
                }

                result.Success = result.Data.Count > 0;
                result.Message = result.Success ? "Patients retrieved successfully." : "No patients found.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"System error: {ex.Message}";
            }

            return result;
        }

        public static int UpdatePatient(int id, PatientDTO patient, string connectionString)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                using var cmd = new SqlCommand("sp_UpdatePatient", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Name", patient.Name);
                cmd.Parameters.AddWithValue("@Age", patient.Age);
                cmd.Parameters.AddWithValue("@Phone", patient.Phone);
                cmd.Parameters.AddWithValue("@Email", patient.Email);
                cmd.Parameters.AddWithValue("@Address", patient.Address);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
            catch
            {
                return -99;
            }
        }
    }
    public static class InvoiceDal
    {
        public static int CreateInvoice(InvoiceDTO invoice, string connectionString)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("sp_AddInvoice", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@AppointmentId", invoice.AppointmentId);
                cmd.Parameters.AddWithValue("@Amount", invoice.Amount);
                cmd.Parameters.AddWithValue("@InvoiceDate", invoice.InvoiceDate);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
            catch
            {
                return -99;
            }
        }

        public static bool DeleteInvoice(int invoiceId, string connectionString)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("sp_DeleteInvoice", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", invoiceId);

                conn.Open();
                int rows = Convert.ToInt32(cmd.ExecuteScalar());
                return rows > 0;
            }
            catch
            {
                return false;
            }
        }

        public static InvoiceDTO? GetInvoiceById(int invoiceId, string connectionString)
        {
            InvoiceDTO? invoice = null;
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("sp_FindInvoiceById", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", invoiceId);

                conn.Open();
                using SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    invoice = new InvoiceDTO
                    {
                        Id = (int)reader["Id"],
                        AppointmentId = (int)reader["AppointmentId"],
                        Amount = (decimal)reader["Amount"],
                        InvoiceDate = (DateTime)reader["Date"]
                    };
                }
            }
            catch
            {
                invoice = null;
            }
            return invoice;
        }

        public static List<InvoiceDTO> GetAllInvoices(string connectionString)
        {
            List<InvoiceDTO> invoices = new List<InvoiceDTO>();
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("Sp_GetAllInvoices", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    invoices.Add(new InvoiceDTO
                    {
                        Id = (int)reader["Id"],
                        AppointmentId = (int)reader["AppointmentId"],
                        Amount = (decimal)reader["Amount"],
                        InvoiceDate = (DateTime)reader["Date"]
                    });
                }
            }
            catch
            {
                invoices = new List<InvoiceDTO>();
            }
            return invoices;
        }

        public static int UpdateInvoice(InvoiceDTO invoice, string connectionString)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("UpdateInvoice", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", invoice.Id);
                cmd.Parameters.AddWithValue("@AppointmentId", invoice.AppointmentId);
                cmd.Parameters.AddWithValue("@Amount", invoice.Amount);
                cmd.Parameters.AddWithValue("@InvoiceDate", invoice.InvoiceDate);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
            catch
            {
                return -99;
            }
        }
    }
    public static class DoctorDal
    {
        public static int AddDoctor(DoctorDTO doctor, string connectionString)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("sp_AddDoctor", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Name", doctor.Name);
                cmd.Parameters.AddWithValue("@Specialty", doctor.Specialty);
                cmd.Parameters.AddWithValue("@Phone", doctor.Phone);
                cmd.Parameters.AddWithValue("@Email", doctor.Email);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
            catch
            {
                return -99;
            }
        }

        public static int UpdateDoctor(int doctorId, DoctorDTO doctor, string connectionString)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("sp_UpdateDoctor", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", doctorId);
                cmd.Parameters.AddWithValue("@Name", doctor.Name);
                cmd.Parameters.AddWithValue("@Specialty", doctor.Specialty);
                cmd.Parameters.AddWithValue("@Phone", doctor.Phone);
                cmd.Parameters.AddWithValue("@Email", doctor.Email);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
            catch
            {
                return -99;
            }
        }

        public static int DeleteDoctor(int doctorId, string connectionString)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("sp_DeleteDoctorWithCheck", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", doctorId);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
            catch
            {
                return -99;
            }
        }

        public static DoctorDTO? GetDoctorById(int doctorId, string connectionString)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("sp_FindDoctorById", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Id", doctorId);

                conn.Open();
                using SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new DoctorDTO
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Specialty = reader["Specialty"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Email = reader["Email"].ToString()
                    };
                }
            }
            catch { }

            return null;
        }

        public static List<DoctorDTO> GetAllDoctors(string connectionString)
        {
            var doctors = new List<DoctorDTO>();

            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("Sp_GetAllDoctors", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                conn.Open();
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    doctors.Add(new DoctorDTO
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Specialty = reader["Specialty"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Email = reader["Email"].ToString()
                    });
                }
            }
            catch { }

            return doctors;
        }

        public static bool DoctorExists(int doctorId, string connectionString)
        {
            return GetDoctorById(doctorId, connectionString) != null;
        }
    }
    public static class AppointmentDal
    {
        public static int AddAppointment(AppointmentDTO appointment, string connectionString)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("sp_AddAppointment", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PatientId", appointment.PatientId);
                cmd.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
                cmd.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                cmd.Parameters.AddWithValue("@Status", appointment.Status);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
            catch
            {
                return -99;
            }
        }

        public static int UpdateAppointment(int appointmentId, AppointmentDTO appointment, string connectionString)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("sp_UpdateAppointment", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", appointmentId);
                cmd.Parameters.AddWithValue("@PatientId", appointment.PatientId);
                cmd.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
                cmd.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
                cmd.Parameters.AddWithValue("@Status", appointment.Status);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
            catch
            {
                return -99;
            }
        }

        public static int DeleteAppointment(int appointmentId, string connectionString)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("sp_DeleteAppointmentWithCheck", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", appointmentId);

                conn.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
            catch
            {
                return -99;
            }
        }

        public static AppointmentDTO? GetAppointmentById(int appointmentId, string connectionString)
        {
            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("sp_FindAppointmentById", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", appointmentId);
                conn.Open();

                using SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new AppointmentDTO
                    {
                        Id = (int)reader["Id"],
                        PatientId = (int)reader["PatientId"],
                        DoctorId = (int)reader["DoctorId"],
                        AppointmentDate = (DateTime)reader["Date"],
                        Status = reader["Status"].ToString()
                    };
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static List<AppointmentDTO> GetAllAppointments(string connectionString)
        {
            List<AppointmentDTO> appointments = new List<AppointmentDTO>();

            try
            {
                using SqlConnection conn = new SqlConnection(connectionString);
                using SqlCommand cmd = new SqlCommand("SELECT * FROM Appointments", conn); // ممكن تستخدم stored procedure إذا تحب
                conn.Open();

                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    appointments.Add(new AppointmentDTO
                    {
                        Id = (int)reader["Id"],
                        PatientId = (int)reader["PatientId"],
                        DoctorId = (int)reader["DoctorId"],
                        AppointmentDate = (DateTime)reader["Date"],
                        Status = reader["Status"].ToString()
                    });
                }
            }
            catch { }

            return appointments;
        }

        public static bool AppointmentExists(int appointmentId, string connectionString)
        {
            return GetAppointmentById(appointmentId, connectionString) != null;
        }
    }
}



