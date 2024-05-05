using System.Data.SqlClient;
using PrescriptionExample.Model;

namespace PrescriptionExample.Repository;

public class PrescriptionRepository(IConfiguration configuration) : IPrescriptionRepository {

    public IEnumerable<PrescriptionView> GetPrescriptions(string? name) {
        using var con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT IdPrescription, Date, DueDate, P.LastName, D.LastName FROM Prescription " +
                          "LEFT JOIN s27049.Patient P on P.IdPatient = Prescription.IdPatient " +
                          "LEFT JOIN s27049.Doctor D on D.IdDoctor = Prescription.IdDoctor " +
                          (string.IsNullOrEmpty(name) ? "" : "WHERE D.LastName = @Name ") +
                          "ORDER BY Date DESC ";
        if (!string.IsNullOrEmpty(name)) cmd.Parameters.AddWithValue("@Name", name);


        var dr = cmd.ExecuteReader();
        var prescriptions = new List<PrescriptionView>();
        while (dr.Read()) {
            var prescription = new PrescriptionView {
                Date = dr["Date"].ToString(),
                DueDate = dr["DueDate"].ToString(),
                PatientLastName = dr["P.LastName"].ToString(),
                DoctorLastName = dr["D.LastName"].ToString()
            };
            prescriptions.Add(prescription);
        }

        return prescriptions;
    }

    public int CreatePrescription(Prescription prescription) {
        using var con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        con.Open();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText =
            "INSERT INTO Student(Date, DueDate, IdPatient,IdDoctor) VALUES(@Date, @DueDate, @IdPatient, @IdDoctor)";
        cmd.Parameters.AddWithValue("@Date", prescription.Date);
        cmd.Parameters.AddWithValue("@DueDate", prescription.DueDate);
        cmd.Parameters.AddWithValue("@IdPatient", prescription.IdPatient);
        cmd.Parameters.AddWithValue("@IdDoctor", prescription.IdDoctor);

        var affectedCount = cmd.ExecuteNonQuery();
        return affectedCount;
    }
}