using Drivers.BLL.Managers.Contracts;
using Drivers.DAL.ADO.UOW.Contracts;
using Drivers.DAL.EF.UOW.Contracts;
using Microsoft.Extensions.Logging;

namespace Drivers.BLL.Managers
{
    public class ExpenseManager : IExpenseManager
    {
        private readonly ILogger<ExpenseManager> _logger;
        private IUnitOfWork _ADOuow;
        private IEFUnitOfWork _EFuow;

        public ExpenseManager(ILogger<ExpenseManager> logger,
            IUnitOfWork ado_unitofwork,
            IEFUnitOfWork eFUnitOfWork)
        {
            _logger = logger;
            _ADOuow = ado_unitofwork;
            _EFuow = eFUnitOfWork;
        }
    }
}
