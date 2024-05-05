using PrescriptionExample.Model;

namespace PrescriptionExample.Repository;

public interface IPrescriptionRepository {
    IEnumerable<PrescriptionView> GetPrescriptionsWithLastNames(string? name);
    int CreatePrescription(Prescription prescription);
    public string? GetLastName(int id, string table);
}