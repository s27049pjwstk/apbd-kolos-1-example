using PrescriptionExample.Model;

namespace PrescriptionExample.Repository;

public interface IPrescriptionRepository {
    IEnumerable<PrescriptionView> GetPrescriptions(string? name);
    int CreatePrescription(Prescription prescription);
}