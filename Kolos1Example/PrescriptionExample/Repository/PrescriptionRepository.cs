using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using PrescriptionExample.Model;

namespace PrescriptionExample.Repository;

public class PrescriptionRepository : IPrescriptionRepository
{
    private IConfiguration _configuration;

    private string[] _tablesDefult = { "Doctor", "Patient" };

    private readonly List<string> _tables;

    public PrescriptionRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _tables = new List<string>(_tablesDefult);
    }

    public async Task<IEnumerable<PrescriptionView>> GetPrescriptionsWithLastNamesAsync(string? name)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText =
            "SELECT IdPrescription, Date, DueDate, P.LastName as PatientLastName, D.LastName as DoctorLastName FROM Prescription " +
            "LEFT JOIN s27049.Patient P on P.IdPatient = Prescription.IdPatient " +
            "LEFT JOIN s27049.Doctor D on D.IdDoctor = Prescription.IdDoctor " +
            (string.IsNullOrEmpty(name) ? "" : "WHERE D.LastName = @Name ") +
            "ORDER BY Date DESC ";
        if (!string.IsNullOrEmpty(name)) cmd.Parameters.AddWithValue("@Name", name);


        var dr = await cmd.ExecuteReaderAsync();
        var prescriptions = new List<PrescriptionView>();
        while (await dr.ReadAsync())
        {
            var prescription = new PrescriptionView
            {
                Date = dr["Date"].ToString(),
                DueDate = dr["DueDate"].ToString(),
                PatientLastName = dr["PatientLastName"].ToString(),
                DoctorLastName = dr["DoctorLastName"].ToString()
            };
            prescriptions.Add(prescription);
        }

        return prescriptions;
    }

    public async Task<int> CreatePrescriptionAsync(Prescription prescription)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText =
            "INSERT INTO Prescription(Date, DueDate, IdPatient,IdDoctor) VALUES(@Date, @DueDate, @IdPatient, @IdDoctor)";
        cmd.Parameters.AddWithValue("@Date", prescription.Date);
        cmd.Parameters.AddWithValue("@DueDate", prescription.DueDate);
        cmd.Parameters.AddWithValue("@IdPatient", prescription.IdPatient);
        cmd.Parameters.AddWithValue("@IdDoctor", prescription.IdDoctor);

        var affectedCount = await cmd.ExecuteNonQueryAsync();
        return affectedCount;
    }


    public async Task<string?> GetLastNameAsync(int id, string table)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        if (!_tables.Contains(table)) return null;
        cmd.CommandText = $"select LastName from {table} where Id{table} = @id";
        cmd.Parameters.AddWithValue("@id", id);

        var dr = await cmd.ExecuteReaderAsync();
        return !await dr.ReadAsync() ? null : dr["LastName"].ToString();
    }
}