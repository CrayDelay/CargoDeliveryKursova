using Drivers.BLL.DTOs.Requests;
using Drivers.BLL.DTOs.Responses;
using Drivers.DAL.EF.Entities;
using Drivers.DAL.EF.Entities.HelpModels;
using Drivers.DAL.EF.Helpers;

namespace Drivers.BLL.Managers.Contracts
{
    public interface IDriversManager
    {
        Task<IEnumerable<ShortDriverResponceDTO>> GetListOfAllDrivers(CancellationToken cancellationToken);
        Task<FullDriverResponceDTO> GetFullInfoAboutDriver(int id, CancellationToken cancellationToken);
        Task<PagedList<EFDriver>> GetPaginatedDrivers(DriverParameters driverParameters, CancellationToken cancellationToken);
        Task<EFDriver> AddDriverToSystemAsync(MiniDriverReqDTO driverDTO, CancellationToken cancellationToken);
        Task<EFDriver> UpdateDriverInSystemAsync(int id, MiniDriverReqDTO driverDTO, CancellationToken cancellationToken);
        Task DeleteDriverFromSystemAsync(int id, CancellationToken cancellationToken);
    }
}
