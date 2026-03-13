namespace HospitalManagementAPI.Models
{
    public class Doctor
    {
        public int DoctorId { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? Phone { get; set; }

        public string? Specialization { get; set; }

        public string? ProfilePhoto { get; set; }  // image path or url

        public string? Status { get; set; }  // Pending Approved Rejected

        public DateTime? CreatedDate { get; set; }
    }
}
