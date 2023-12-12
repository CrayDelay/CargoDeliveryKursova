using Drivers.BLL.DTOs.Responses;

namespace Drivers.BLL.Managers.Contracts
{
    public interface ICompanyManager
    {
        Task<CompanyResponceDTO> GetCompanyById(int id);
    }
}
