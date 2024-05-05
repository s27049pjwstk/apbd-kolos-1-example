using Microsoft.AspNetCore.Mvc;
using PrescriptionExample.Model;
using PrescriptionExample.Service;

namespace PrescriptionExample.Controller;

[Route("api/[controller]")]
[ApiController]
public class PrescriptionsController(IPrescriptionsService prescriptionsService) : ControllerBase {
    [HttpGet]
    public IActionResult GetPrescriptions(string? name) {
        var result = prescriptionsService.GetPrescriptions(name);
        if (result is null) return NotFound("No Prescriptions Found");
        return Ok(result);
    }

    [HttpPost]
    public IActionResult CreatePrescription(Prescription prescription) {
        try {
            prescriptionsService.CreatePrescription(prescription);
        } catch (Exception e) {
            return StatusCode(StatusCodes.Status400BadRequest);
        }
        return StatusCode(StatusCodes.Status201Created);
    }
}