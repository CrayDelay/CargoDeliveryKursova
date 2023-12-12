using Drivers.DAL.EF.Data;
using Drivers.DAL.EF.Entities;
using Drivers.DAL.EF.Repositories.Contracts;

namespace Drivers.DAL.EF.Repositories
{
    public class EFInspectionRepository : EFGenericRepository<EFInspection>, IEFInspectionRepository
    {
        public EFInspectionRepository(DriversManagementContext databaseContext) : base(databaseContext)
        {
        }

        /// <summary>
        /// Отримання повної інформації з таблиці Inspection по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task<EFInspection> GetCompleteEntityAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
