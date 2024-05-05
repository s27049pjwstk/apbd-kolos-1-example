using PrescriptionExample.Model;
using PrescriptionExample.Repository;

namespace PrescriptionExample.Service;

public class PrescriptionService(IPrescriptionRepository prescriptionRepository) : IPrescriptionsService {
    public IEnumerable<PrescriptionView> GetPrescriptions(string? name) => prescriptionRepository.GetPrescriptions(name);

    public int CreatePrescription(Prescription prescription) => prescriptionRepository.CreatePrescription(prescription);
}