using PrescriptionExample.Model;

namespace PrescriptionExample.Service;

public interface IPrescriptionsService {
    Task<IEnumerable<PrescriptionView>> GetPrescriptionsAsync(string? name);
    Task<int> CreatePrescriptionAsync(Prescription prescription);
}