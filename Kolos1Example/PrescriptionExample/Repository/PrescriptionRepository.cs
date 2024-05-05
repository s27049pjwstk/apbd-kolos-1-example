using System.Data.SqlClient;
using PrescriptionExample.Model;

namespace PrescriptionExample.Repository;

public class PrescriptionRepository(IConfiguration configuration) : IPrescriptionRepository {
    public async Task<IEnumerable<PrescriptionView>> GetPrescriptionsWithLastNamesAsync(string? name) {
        await using var con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT IdPrescription, Date, DueDate, P.LastName, D.LastName FROM Prescription " +
                          "LEFT JOIN s27049.Patient P on P.IdPatient = Prescription.IdPatient " +
                          "LEFT JOIN s27049.Doctor D on D.IdDoctor = Prescription.IdDoctor " +
                          (string.IsNullOrEmpty(name) ? "" : "WHERE D.LastName = @Name ") +
                          "ORDER BY Date DESC ";
        if (!string.IsNullOrEmpty(name)) cmd.Parameters.AddWithValue("@Name", name);


        var dr = await cmd.ExecuteReaderAsync();
        var prescriptions = new List<PrescriptionView>();
        while (await dr.ReadAsync()) {
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

    public async Task<int> CreatePrescriptionAsync(Prescription prescription) {
        await using var con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText =
            "INSERT INTO Prescription(Date, DueDate, IdPatient,IdDoctor) VALUES(@Date, @DueDate, @IdPatient, @IdDoctor)";
        cmd.Parameters.AddWithValue("@Date", $"\'{prescription.Date}\'");
        cmd.Parameters.AddWithValue("@DueDate", $"\'{prescription.DueDate}\'");
        cmd.Parameters.AddWithValue("@IdPatient", prescription.IdPatient);
        cmd.Parameters.AddWithValue("@IdDoctor", prescription.IdDoctor);

        var affectedCount = await cmd.ExecuteNonQueryAsync();
        return affectedCount;
    }

    private readonly string[] _tables = ["Doctor", "Patient"];

    public async Task<string?> GetLastNameAsync(int id, string table) {
        await using var con = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        if (!_tables.Contains(table)) return null;
        cmd.CommandText = $"select LastName from {table} where IdDoctor = @id";
        cmd.Parameters.AddWithValue("@id", id);

        var dr = await cmd.ExecuteReaderAsync();
        return !await dr.ReadAsync() ? null : dr["LastName"].ToString();
    }
}