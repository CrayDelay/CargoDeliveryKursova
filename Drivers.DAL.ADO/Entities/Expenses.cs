namespace Drivers.DAL.ADO.Entities;

public class Expenses
{
    public int Id { get; set; }
    public decimal DriverPayment { get; set; }
    public decimal FuelCost { get; set; }
    public decimal MaintenanceCost { get; set; }
    public string Category { get; set; }
    public DateTime Date { get; set; }
    public string Note { get; set; }
    public int DriverID { get; set; }
    public int TruckID { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
