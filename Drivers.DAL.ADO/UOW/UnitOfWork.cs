using Drivers.DAL.ADO.Repositories.Contracts;
using Drivers.DAL.ADO.UOW.Contracts;
using System.Data;

namespace Drivers.DAL.ADO.UOW;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    public IDriverRepository _driverRepository { get; }
    public ICompanyRepository _companyRepository { get; }
    public IExpensesRepository _expensesRepository { get; }
    public IPhotoRepository _photoRepository { get; }
    public IInspectionRepository _inspectionRepository { get; }
    public IRepairRepository _repairRepository { get; }
    public ITruckRepository _truckRepository { get; }

    readonly IDbTransaction _dbTransaction;

    public UnitOfWork(
        IDriverRepository driverRepository,
        ICompanyRepository companyRepository,
        IExpensesRepository expensesRepository,
        IInspectionRepository inspectionRepository,
        IPhotoRepository photoRepository,
        IRepairRepository repairRepository,
        ITruckRepository truckRepository,
        IDbTransaction dbTransaction)
    {
        _driverRepository = driverRepository;
        _companyRepository = companyRepository;
        _expensesRepository = expensesRepository;
        _photoRepository = photoRepository;
        _inspectionRepository = inspectionRepository;
        _repairRepository = repairRepository;
        _truckRepository = truckRepository;
        _dbTransaction = dbTransaction;

    }

    public void Commit()
    {
        try
        {
            _dbTransaction.Commit();
            // By adding this we can have muliple transactions as part of a single request
            //_dbTransaction.Connection.BeginTransaction();
        }
        catch (Exception ex)
        {
            _dbTransaction.Rollback();
            Console.WriteLine(ex.Message);
        }
    }

    public void Dispose()
    {
        //Close the SQL Connection and dispose the objects
        _dbTransaction.Connection?.Close();
        _dbTransaction.Connection?.Dispose();
        _dbTransaction.Dispose();
    }
}
