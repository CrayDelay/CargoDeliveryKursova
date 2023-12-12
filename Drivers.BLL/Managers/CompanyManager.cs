using AutoMapper;
using Drivers.BLL.DTOs.Responses;
using Drivers.BLL.Managers.Contracts;
using Drivers.DAL.ADO.UOW.Contracts;
using Drivers.DAL.EF.UOW.Contracts;
using Microsoft.Extensions.Logging;

namespace Drivers.BLL.Managers
{
    public class CompanyManager : ICompanyManager
    {
        private readonly ILogger<CompanyManager> _logger;
        private readonly IMapper _mapper;
        private IUnitOfWork _ADOuow;
        private IEFUnitOfWork _EFuow;

        public CompanyManager(ILogger<CompanyManager> logger,
            IMapper mapper,
            IUnitOfWork ado_unitofwork,
            IEFUnitOfWork eFUnitOfWork)
        {
            _mapper = mapper;
            _logger = logger;
            _ADOuow = ado_unitofwork;
            _EFuow = eFUnitOfWork;
        }

        public async Task<CompanyResponceDTO> GetCompanyById(int id)
        {
            var company = await _EFuow.EFCompanyRepository.GetByIdAsync(id);
            if (company == null) { return null; }

            return _mapper.Map<CompanyResponceDTO>(company);
        }
    }
}
