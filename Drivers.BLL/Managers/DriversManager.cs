using AutoMapper;
using Drivers.BLL.DTOs.Requests;
using Drivers.BLL.DTOs.Responses;
using Drivers.BLL.Exceptions;
using Drivers.BLL.Managers.Contracts;
using Drivers.DAL.ADO.UOW.Contracts;
using Drivers.DAL.EF.Entities;
using Drivers.DAL.EF.Entities.HelpModels;
using Drivers.DAL.EF.Helpers;
using Drivers.DAL.EF.UOW.Contracts;
using Microsoft.Extensions.Logging;

namespace Drivers.BLL.Managers
{
    public class DriversManager : IDriversManager
    {
        private readonly ILogger<DriversManager> _logger;
        private readonly IMapper _mapper;
        private IUnitOfWork _ADOuow;
        private IEFUnitOfWork _EFuow;
        
        public DriversManager(ILogger<DriversManager> logger,
            IMapper mapper,
            IUnitOfWork ado_unitofwork,
            IEFUnitOfWork eFUnitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _ADOuow = ado_unitofwork;
            _EFuow = eFUnitOfWork;
        }

        /// <summary>
        /// ПОВНА ІНФОРМАЦІЯ ПРО ВОДІЯ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FullDriverResponceDTO> GetFullInfoAboutDriver(int id, CancellationToken cancellationToken)
        {
            var result = new FullDriverResponceDTO();
            var driver = await _EFuow.EFDriverRepository.GetByIdAsNoTrackingAsync(id);
            if (driver == null)
            {
                throw new NotFoundException($"Driver with {id} cant be found in database!");
            }
            var driverEntity = await _EFuow.EFDriverRepository.GetCompleteEntityAsync(id);
            return _mapper.Map<FullDriverResponceDTO>(driverEntity);
        }

        /// <summary>
        /// ОТРИМАННЯ ПАГІНОВАНИХ ДАНИХ ПРО ВОДІЯ
        /// </summary>
        /// <param name="driverParameters"></param>
        /// <returns></returns>
        public async Task<PagedList<EFDriver>> GetPaginatedDrivers(DriverParameters driverParameters, CancellationToken cancellationToken)
        {
            return await _EFuow.EFDriverRepository.GetPaginatedDriversAsync(driverParameters);
        }

        /// <summary>
        /// ОТРИМАННЯ CКОРОЧЕНОї ІНФОРМАЦІЇ ПРО ВСІХ ВСІХ ВОДІЇВ
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<ShortDriverResponceDTO>> GetListOfAllDrivers(CancellationToken cancellationToken)
        {
            var alldrv = await _EFuow.EFDriverRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ShortDriverResponceDTO>>(alldrv);
        }

        /// <summary>
        /// ДОДАВАННЯ ВОДІЯ
        /// </summary>
        /// <param name="driverDTO"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<EFDriver> AddDriverToSystemAsync(MiniDriverReqDTO driverDTO, CancellationToken cancellationToken)
        {
            try
            {
                await _EFuow.BeginTransactionAsync(cancellationToken);
                    var result = await _EFuow.EFDriverRepository.AddAsync(_mapper.Map<EFDriver>(driverDTO));
                    await _EFuow.CompleteAsync(cancellationToken);
                await _EFuow.CommitTransactionAsync(cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                await _EFuow.RollbackTransactionAsync(cancellationToken);
                throw new ApplicationException("Error in transaction while adding information about driver", ex);
            }
        }

        /// <summary>
        /// ОНОВЛЕННЯ ІНФОРМАЦІЇ ПРО ВОДІЯ
        /// </summary>
        /// <param name="driverDTO">Скорочена інфа про водія</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<EFDriver> UpdateDriverInSystemAsync(int id, MiniDriverReqDTO driverDTO, CancellationToken cancellationToken)
        {
            var driverEntity = await _EFuow.EFDriverRepository.GetByIdAsNoTrackingAsync(id);
            if (driverEntity == null)
            {
                throw new NotFoundException($"Driver with {id} cant be found and updated in database!");
            }
            try
            {
                await _EFuow.BeginTransactionAsync(cancellationToken);
                await _EFuow.EFDriverRepository.UpdateAsync(_mapper.Map<EFDriver>(driverDTO));
                await _EFuow.CompleteAsync(cancellationToken);

                await _EFuow.CommitTransactionAsync(cancellationToken);
                return await _EFuow.EFDriverRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                await _EFuow.RollbackTransactionAsync(cancellationToken);
                throw new ApplicationException("Error in transaction while updating information about driver", ex);
            }
        }

        /// <summary>
        /// Видалення запису водія
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public async Task DeleteDriverFromSystemAsync(int id, CancellationToken cancellationToken)
        {
            var driverEntity = await _EFuow.EFDriverRepository.GetByIdAsNoTrackingAsync(id);
            if (driverEntity == null)
            {
                throw new NotFoundException($"Driver with {id} cant be found and deleted from database!");
            }
            try
            {
                await _EFuow.BeginTransactionAsync(cancellationToken);
                await _EFuow.EFDriverRepository.DeleteByIdAsync(id);
                await _EFuow.CompleteAsync(cancellationToken);
                await _EFuow.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await _EFuow.RollbackTransactionAsync(cancellationToken);
                throw new ApplicationException("Error in transaction while updating information about driver", ex);
            }
        }
    }
}
