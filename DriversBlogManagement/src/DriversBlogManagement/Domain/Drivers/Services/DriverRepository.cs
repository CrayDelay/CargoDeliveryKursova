namespace DriversBlogManagement.Domain.Drivers.Services;

using DriversBlogManagement.Domain.Drivers;
using DriversBlogManagement.Databases;
using DriversBlogManagement.Services;

public interface IDriverRepository : IGenericRepository<Driver>
{
}

public sealed class DriverRepository : GenericRepository<Driver>, IDriverRepository
{
    private readonly BlogManagementContext _dbContext;

    public DriverRepository(BlogManagementContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
