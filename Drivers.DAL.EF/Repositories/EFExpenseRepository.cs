using Drivers.DAL.EF.Data;
using Drivers.DAL.EF.Entities;
using Drivers.DAL.EF.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Drivers.DAL.EF.Repositories
{
    public class EFExpenseRepository : EFGenericRepository<EFExpense>, IEFExpenseRepository
    {
        public EFExpenseRepository(DriversManagementContext databaseContext) : base(databaseContext)
        {
        }

        /// <summary>
        /// Отримання повної інформації з таблиці Expense по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Task<EFExpense> GetCompleteEntityAsync(int id)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<EFExpense>> GetExpencesByDriver(int driver_id)
        {
            return await databaseContext.Expenses.Where(expense => expense.DriverId == driver_id).ToListAsync();

        }
    }
}
