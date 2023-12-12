namespace DriversManagement.Domain.Drivers.Features;

using DriversManagement.Domain.Drivers.Dtos;
using DriversManagement.Domain.Drivers.Services;
using DriversManagement.Exceptions;
using DriversManagement.Domain;
using HeimGuard;
using Mappings;
using MediatR;

public static class GetDriver
{
    public sealed record Query(Guid DriverId) : IRequest<DriverDto>;

    public sealed class Handler : IRequestHandler<Query, DriverDto>
    {
        private readonly IDriverRepository _driverRepository;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IDriverRepository driverRepository, IHeimGuardClient heimGuard)
        {
            _driverRepository = driverRepository;
            _heimGuard = heimGuard;
        }

        public async Task<DriverDto> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadDriver);

            var result = await _driverRepository.GetById(request.DriverId, cancellationToken: cancellationToken);
            return result.ToDriverDto();
        }
    }
}