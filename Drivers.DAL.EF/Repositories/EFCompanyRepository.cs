using Drivers.DAL.EF.Data;
using Drivers.DAL.EF.Entities;
using Drivers.DAL.EF.Repositories.Contracts;

namespace Drivers.DAL.EF.Repositories
{
    public class EFCompanyRepository : EFGenericRepository<EFCompany>, IEFCompanyRepository
    {
        public EFCompanyRepository(DriversManagementContext databaseContext) : base(databaseContext)
        {
        }

        /// <summary>
        /// Отримання повної інформації з таблиці компанії по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task<EFCompany> GetCompleteEntityAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
