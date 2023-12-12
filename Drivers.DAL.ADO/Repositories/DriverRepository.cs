using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Drivers.DAL.ADO.Entities;
using Drivers.DAL.ADO.Repositories.Contracts;

namespace Drivers.DAL.ADO.Repositories;

public class DriverRepository : GenericRepository<Driver>, IDriverRepository
{
    public DriverRepository(SqlConnection sqlConnection, IDbTransaction dbtransaction) : base(sqlConnection, dbtransaction, "Drivers")
    {

    }

    public async Task<IEnumerable<Driver>> GetAllDrivers()
    {
        string sql = @"SELECT Name, Surname  FROM Driver";
        var results = await _sqlConnection.QueryAsync<Driver>(sql,
            transaction: _dbTransaction);
        return results;

    }

    public async Task<Driver> GetDriversInfo()
    {
        string sql = @"SELECT Name, Surname , Driver_Id, Country_Id, Rating_Id, DriverLicense_Id , Car_Id 
                FROM Driver, Car, Country, Rating, DriverLicense
                Where Driver.Car_Id=Car.Id, Driver.Country_Id=Country.Id, Driver.Rating_Id=Rating.Id, Driver.DriverLicense_Id=DriverLicense.Id";

        var results = await _sqlConnection.QueryAsync<Driver>(sql,
            transaction: _dbTransaction);
        return (Driver)results;

    }
    public async Task<IEnumerable<Driver>> GetTop5DriversByRating()
    {
        string sql = @"SELECT TOP 5 Rating 
                         From Drivers, Rating
                         Where Drivers.Rating_id=Rating.Id";
        var results = await _sqlConnection.QueryAsync<Driver>(sql,
           transaction: _dbTransaction);
        return results;
    }
}
