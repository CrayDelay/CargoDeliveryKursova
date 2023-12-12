using Drivers.DAL.ADO.Entities;
using Drivers.DAL.ADO.Repositories.Contracts;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Drivers.DAL.ADO.Repositories;

public class TruckRepository : GenericRepository<Truck>, ITruckRepository
{
    public TruckRepository(SqlConnection sqlConnection, IDbTransaction dbtransaction) : base(sqlConnection, dbtransaction, "Trucks")
    {

    }
}
