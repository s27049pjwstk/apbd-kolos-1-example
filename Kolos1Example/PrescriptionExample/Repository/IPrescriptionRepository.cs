using PrescriptionExample.Model;

namespace PrescriptionExample.Repository;

public interface IPrescriptionRepository {
    Task<IEnumerable<PrescriptionView>> GetPrescriptionsWithLastNamesAsync(string? name);
    Task<int> CreatePrescriptionAsync(Prescription prescription);
    public Task<string?> GetLastNameAsync(int id, string table);
}