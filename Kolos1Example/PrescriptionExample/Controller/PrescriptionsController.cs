using Microsoft.AspNetCore.Mvc;
using PrescriptionExample.Model;
using PrescriptionExample.Service;

namespace PrescriptionExample.Controller;

[Route("api/[controller]")]
[ApiController]
public class PrescriptionsController(IPrescriptionsService prescriptionsService) : ControllerBase {
    [HttpGet]
    public async Task<IActionResult> GetPrescriptionsAsync(string? name) {
        var result = await prescriptionsService.GetPrescriptionsAsync(name);
        if (result is null) return NotFound("No Prescriptions Found");
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePrescriptionAsync(Prescription prescription) {
        try {
            await prescriptionsService.CreatePrescriptionAsync(prescription);
        } catch (Exception e) {
            return StatusCode(StatusCodes.Status400BadRequest);
        }

        return StatusCode(StatusCodes.Status201Created);
    }
}