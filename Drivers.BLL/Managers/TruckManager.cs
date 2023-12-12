using Drivers.BLL.DTOs.Responses;
using Drivers.BLL.Managers.Contracts;
using Drivers.DAL.ADO.UOW.Contracts;
using Drivers.DAL.EF.UOW.Contracts;
using Microsoft.Extensions.Logging;

namespace Drivers.BLL.Managers
{
    public class TruckManager : ITruckManager
    {
        private readonly ILogger<TruckManager> _logger;
        private IUnitOfWork _ADOuow;
        private IEFUnitOfWork _EFuow;

        public TruckManager(ILogger<TruckManager> logger,
            IUnitOfWork ado_unitofwork,
            IEFUnitOfWork eFUnitOfWork)
        {
            _logger = logger;
            _ADOuow = ado_unitofwork;
            _EFuow = eFUnitOfWork;
        }

        public async Task<IEnumerable<TruckResponceDTO>> GetAllTrucks()
        {
            var list_of_trucks = await _EFuow.EFPTruckRepository.GetAllAsync();
            var list_of_trucks_dto = new List<TruckResponceDTO>();
            foreach(var truck in list_of_trucks)
            {
                var truck_dto = new TruckResponceDTO();
                truck_dto.Id = truck.Id;
                truck_dto.TruckNumber = truck.TruckNumber;
                truck_dto.RegistrationNumber = truck.RegistrationNumber;
                truck_dto.InsurancePolicyNumber = truck.InsurancePolicyNumber;
                truck_dto.Year = truck.Year;
                truck_dto.Capacity = truck.Capacity;
                truck_dto.FuelConsumption = truck.FuelConsumption;
                list_of_trucks_dto.Add(truck_dto);
            }
            return list_of_trucks_dto;
        }
    }
}
