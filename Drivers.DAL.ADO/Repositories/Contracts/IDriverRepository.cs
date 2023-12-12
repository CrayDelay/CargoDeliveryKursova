using Drivers.DAL.ADO.Entities;

namespace Drivers.DAL.ADO.Repositories.Contracts
{
    public interface IDriverRepository : IGenericRepository<Driver>
    {
        Task<IEnumerable<Driver>> GetAllDrivers();
        Task<Driver> GetDriversInfo();
        Task<IEnumerable<Driver>> GetTop5DriversByRating();
    }
}
