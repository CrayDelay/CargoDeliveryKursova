namespace Drivers.DAL.ADO.Entities;

public class Driver
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string MiddleName { get; set; } = default!;
    public string Gender { get; set; } = default!;
    public DateTime Birthdate { get; set; }
    public string PlaceOfBirth { get; set; } = default!;
    public string Nationality { get; set; } = default!;
    public string MaritalStatus { get; set; } = default!;
    public string IdentificationType { get; set; } = default!;
    public string IdentificationNumber { get; set; } = default!;
    public DateTime? IdentificationExpirationDate { get; set; }
    public string Address { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string DriverLicenseNumber { get; set; } = default!;
    public string DriverLicenseCategory { get; set; } = default!;
    public DateTime DriverLicenseIssuingDate { get; set; }
    public DateTime DriverLicenseExpirationDate { get; set; }
    public string DriverLicenseIssuingAuthority { get; set; } = default!;
    public string EmploymentStatus { get; set; } = default!;
    public DateTime? EmploymentStartDate { get; set; }
    public DateTime? EmploymentEndDate { get; set; }


    public int CompanyID { get; set; }
    public Company? Company { get; set; }
    public int PhotoID { get; set; }
    public Photo? Photo { get; set; }

    //public int? TruckID { get; set; }
    //public Truck? Truck { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
