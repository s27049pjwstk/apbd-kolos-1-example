using System.Data;
using System.Runtime.InteropServices.JavaScript;
using PrescriptionExample.Model;
using PrescriptionExample.Repository;

namespace PrescriptionExample.Service;

public class PrescriptionService : IPrescriptionsService
{
    private IPrescriptionRepository _prescriptionRepository;

    public PrescriptionService(IPrescriptionRepository prescriptionRepository)
    {
        _prescriptionRepository = prescriptionRepository;
    }

    public async Task<IEnumerable<PrescriptionView>> GetPrescriptionsAsync(string? name) {
        return await _prescriptionRepository.GetPrescriptionsWithLastNamesAsync(name);
    }

    public async Task<int> CreatePrescriptionAsync(Prescription prescription) {
        // if ((DateTime.Parse(prescription.Date) < DateTime.Parse(prescription.DueDate)) &&
        //     await _prescriptionRepository.GetLastNameAsync(prescription.IdDoctor, "Doctor") is not null &&
        //     await _prescriptionRepository.GetLastNameAsync(prescription.IdPatient, "Patient") is not null) {
        //     return await _prescriptionRepository.CreatePrescriptionAsync(prescription);
        // }
        //fixme debug
        return await _prescriptionRepository.CreatePrescriptionAsync(prescription);

        throw new SyntaxErrorException();
    }
}