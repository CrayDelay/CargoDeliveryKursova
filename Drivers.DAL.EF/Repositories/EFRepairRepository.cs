using Drivers.DAL.EF.Data;
using Drivers.DAL.EF.Entities;
using Drivers.DAL.EF.Repositories.Contracts;

namespace Drivers.DAL.EF.Repositories
{
    public class EFRepairRepository : EFGenericRepository<EFRepair>, IEFRepairRepository
    {
        public EFRepairRepository(DriversManagementContext databaseContext) : base(databaseContext)
        {
        }

        /// <summary>
        /// Отримання повної інформації з таблиці Repair по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task<EFRepair> GetCompleteEntityAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
