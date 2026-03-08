namespace ClincManagementSystem.DTOs
{


    // ======================= USERS =======================
    public class UserDTO
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? RefreshTokenHash { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public DateTime? RefreshTokenRevokedAt { get; set; }
        public string? Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateUserDTO
    {
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }

    public class UpdateUserDTO
    {
        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Role { get; set; }
    }

    public class OperationResult<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }

    public class PatientDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Age { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

    }
    public class DoctorDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Specialty { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

    }
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? Status { get; set; }

    }
    public class InvoiceDTO
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string? Status { get; set; }

    }
}
