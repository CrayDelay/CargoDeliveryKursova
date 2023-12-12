using Drivers.DAL.EF.Data;
using Drivers.DAL.EF.Entities;
using Drivers.DAL.EF.Repositories.Contracts;

namespace Drivers.DAL.EF.Repositories
{
    public class EFTruckRepository : EFGenericRepository<EFTruck>, IEFTruckRepository
    {
        public EFTruckRepository(DriversManagementContext databaseContext) : base(databaseContext)
        {
        }

        /// <summary>
        /// Отримання повної інформації з таблиці Truck по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task<EFTruck> GetCompleteEntityAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
