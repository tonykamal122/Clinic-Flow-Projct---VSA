namespace ClinicFlow.Api.Domain.Entities ;

    public class Doctor
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; } = default!;
        public string DoctorSpecialization { get; set; } = default!;
    }