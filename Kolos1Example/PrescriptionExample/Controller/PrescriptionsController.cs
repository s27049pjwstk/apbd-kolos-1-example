using Microsoft.AspNetCore.Mvc;
using PrescriptionExample.Model;
using PrescriptionExample.Service;

namespace PrescriptionExample.Controller;

[Route("api/[controller]")]
[ApiController]
public class PrescriptionsController(IPrescriptionsService prescriptionsService) : ControllerBase {
    [HttpGet]
    public IActionResult GetPrescriptions(string? name) {
        var data = prescriptionsService.GetPrescriptions(name);
        if (data == null) return NotFound("No Prescriptions Found");
        return Ok(data);
    }

    [HttpPost]
    public IActionResult CreatePrescription(Prescription prescription) {
        prescriptionsService.CreatePrescription(prescription);
        return StatusCode(StatusCodes.Status201Created);
    }
}