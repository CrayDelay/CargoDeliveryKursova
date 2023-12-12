namespace Drivers.DAL.ADO.Entities;

public class Truck
{
    public int Id { get; set; }
    public string TruckNumber { get; set; } = default!;
    public string Model { get; set; } = default!;
    public int Year { get; set; }
    public int Capacity { get; set; }
    public string FuelType { get; set; } = default!;
    public decimal? FuelConsumption { get; set; }
    public string RegistrationNumber { get; set; } = default!;
    public string VIN { get; set; } = default!;
    public string EngineNumber { get; set; } = default!;
    public string ChassisNumber { get; set; } = default!;
    public string InsurancePolicyNumber { get; set; } = default!;
    public DateTime? InsuranceExpirationDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
