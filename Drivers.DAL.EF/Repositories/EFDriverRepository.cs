using Drivers.DAL.EF.Data;
using Drivers.DAL.EF.Entities;
using Drivers.DAL.EF.Entities.HelpModels;
using Drivers.DAL.EF.Helpers;
using Drivers.DAL.EF.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Drivers.DAL.EF.Repositories;

public class EFDriverRepository : EFGenericRepository<EFDriver>, IEFDriverRepository
{
    private ISortHelper<EFDriver> _sortHelper;
    public EFDriverRepository(DriversManagementContext databaseContext,
        ISortHelper<EFDriver> sortHelper)
        : base(databaseContext)
    {
        _sortHelper = sortHelper;
    }

    /// <summary>
    /// Отримання пагінованих даних водіїв
    /// </summary>
    /// <param name="driverParameters"></param>
    /// <returns></returns>
    public async Task<PagedList<EFDriver>> GetPaginatedDriversAsync(DriverParameters driverParameters)
    {
        var drivers = await FindByCondition(d => d.Birthdate.Year >= driverParameters.MinYearOfBirth &&
                            d.Birthdate.Year <= driverParameters.MaxYearOfBirth);

        SearchByName(ref drivers, driverParameters.LastName);

        var sortedOwners = _sortHelper.ApplySort(drivers, driverParameters.OrderBy);

        return PagedList<EFDriver>.ToPagedList(drivers,
                driverParameters.PageNumber,
                driverParameters.PageSize);
    }

    public async Task<EFDriver> GetByIdAsNoTrackingAsync(int id)
    {
        return await table.AsNoTracking().FirstOrDefaultAsync(drv => drv.Id == id);
    }

    /// <summary>
    /// Отримання повної інформації з таблиці водії по id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override async Task<EFDriver> GetCompleteEntityAsync(int id)
    {
        var result = await databaseContext.Drivers
            .Where(driver => driver.Id == id)
            .Include(driver => driver.Company)
            .FirstOrDefaultAsync();

        var photo = await databaseContext.Photos
            .Where(photo => photo.Id == result.PhotoId)
            .Select(photo => new EFPhoto
            {
                Id = photo.Id,
                PhotoData = photo.PhotoData,
                ContentType = photo.ContentType,
                FileName = photo.FileName,
                FileSize = photo.FileSize,
                CreatedAt = photo.CreatedAt,
                UpdatedAt = photo.UpdatedAt,
                DeletedAt = photo.DeletedAt
            })
            .FirstOrDefaultAsync();

        var expenses = await databaseContext.Expenses
            .Where(expense => expense.DriverId == id)
            .Select(expense => new EFExpense
            {
                Id = expense.Id,
                DriverId = expense.DriverId,
                TruckId = expense.TruckId,
                DriverPayment = expense.DriverPayment,
                FuelCost = expense.FuelCost,
                MaintenanceCost = expense.MaintenanceCost,
                Category = expense.Category,
                Date = expense.Date,
                Note = expense.Note,
                CreatedAt = expense.CreatedAt,
                UpdatedAt = expense.UpdatedAt,
                DeletedAt = expense.DeletedAt
            })
            .ToListAsync();

        result.Photo = photo;
        result.Expenses = expenses;
        return result;
    }

    /////////////////////
    // ДОПОМІЖНІ МЕТОДИ 
    /////////////////////
    private void SearchByName(ref IQueryable<EFDriver> drivers, string LastName)
    {
        if (!drivers.Any() || string.IsNullOrWhiteSpace(LastName))
            return;
        drivers = drivers.Where(d => d.LastName.ToLower().Contains(LastName.Trim().ToLower()));
    }
}
