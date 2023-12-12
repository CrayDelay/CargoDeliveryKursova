using Drivers.DAL.EF.Data;
using Drivers.DAL.EF.Entities;
using Drivers.DAL.EF.Repositories.Contracts;

namespace Drivers.DAL.EF.Repositories
{
    public class EFPhotoRepository : EFGenericRepository<EFPhoto>, IEFPhotoRepository
    {
        public EFPhotoRepository(DriversManagementContext databaseContext) : base(databaseContext)
        {
        }

        /// <summary>
        /// Отримання повної інформації з таблиці Photo по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task<EFPhoto> GetCompleteEntityAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
