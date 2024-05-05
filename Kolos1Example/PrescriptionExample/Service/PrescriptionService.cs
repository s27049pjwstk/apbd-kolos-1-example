using System.Data;
using System.Runtime.InteropServices.JavaScript;
using PrescriptionExample.Model;
using PrescriptionExample.Repository;

namespace PrescriptionExample.Service;

public class PrescriptionService(IPrescriptionRepository prescriptionRepository) : IPrescriptionsService {
    public async Task<IEnumerable<PrescriptionView>> GetPrescriptionsAsync(string? name) {
        return await prescriptionRepository.GetPrescriptionsWithLastNamesAsync(name);
    }

    public async Task<int> CreatePrescriptionAsync(Prescription prescription) {
        if ((DateTime.Parse(prescription.Date) < DateTime.Parse(prescription.DueDate)) &&
            await prescriptionRepository.GetLastNameAsync(prescription.IdDoctor, "Doctor") is not null &&
            await prescriptionRepository.GetLastNameAsync(prescription.IdPatient, "Patient") is not null) {
            return await prescriptionRepository.CreatePrescriptionAsync(prescription);
        }
        throw new SyntaxErrorException();
    }
}