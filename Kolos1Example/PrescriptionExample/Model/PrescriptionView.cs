using System.ComponentModel.DataAnnotations;

namespace PrescriptionExample.Model;

public class PrescriptionView {
    public int IdPrescription { get; set; }
    [Required] public string Date { get; set; }
    [Required] public string DueDate { get; set; }
    [Required] public string PatientLastName { get; set; }
    [Required] public string DoctorLastName { get; set; }
}