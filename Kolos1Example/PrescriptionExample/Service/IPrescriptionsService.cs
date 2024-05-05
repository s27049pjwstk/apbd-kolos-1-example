using PrescriptionExample.Model;

namespace PrescriptionExample.Service;

public interface IPrescriptionsService {
    IEnumerable<PrescriptionView>? GetPrescriptions(string? name);
    int CreatePrescription(Prescription prescription);
}