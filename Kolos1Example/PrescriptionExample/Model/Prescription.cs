namespace PrescriptionExample.Model;

public class Prescription {
    public string Date { get; set; }
    public string DueDate { get; set; }
    public int IdPatient { get; set; }
    public int IdDoctor { get; set; }
}