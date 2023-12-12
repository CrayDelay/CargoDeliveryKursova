using Drivers.DAL.EF.Entities;
using Drivers.DAL.EF.Entities.HelpModels;
using Drivers.DAL.EF.Helpers;

namespace Drivers.DAL.EF.Repositories.Contracts;

public interface IEFDriverRepository : IEFGenericRepository<EFDriver>
{
    Task<PagedList<EFDriver>> GetPaginatedDriversAsync(DriverParameters driverParameters);
    Task<EFDriver> GetByIdAsNoTrackingAsync(int id);
}