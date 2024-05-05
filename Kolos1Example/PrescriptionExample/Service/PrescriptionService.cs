using System.Data;
using System.Runtime.InteropServices.JavaScript;
using PrescriptionExample.Model;
using PrescriptionExample.Repository;

namespace PrescriptionExample.Service;

public class PrescriptionService(IPrescriptionRepository prescriptionRepository) : IPrescriptionsService {
    public IEnumerable<PrescriptionView> GetPrescriptions(string? name) {
        return prescriptionRepository.GetPrescriptionsWithLastNames(name);
    }

    public int CreatePrescription(Prescription prescription) {
        if ((DateTime.Parse(prescription.Date) < DateTime.Parse(prescription.DueDate)) &&
            prescriptionRepository.GetLastName(prescription.IdDoctor, "Doctor") is not null &&
            prescriptionRepository.GetLastName(prescription.IdPatient, "Patient") is not null) {
            return prescriptionRepository.CreatePrescription(prescription);
        }
        throw new SyntaxErrorException();
    }
}