using Drivers.DAL.ADO.Entities;
using Drivers.DAL.ADO.Repositories.Contracts;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Drivers.DAL.ADO.Repositories;

public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
{
    public CompanyRepository(SqlConnection sqlConnection, IDbTransaction dbtransaction) : base(sqlConnection, dbtransaction, "Companies")
    {

    }
}
