namespace DriversManagement.Domain.Expences.Features;

using DriversManagement.Domain.Expences.Services;
using DriversManagement.Domain.Expences;
using DriversManagement.Domain.Expences.Dtos;
using DriversManagement.Domain.Expences.Models;
using DriversManagement.Services;
using DriversManagement.Exceptions;
using DriversManagement.Domain;
using HeimGuard;
using Mappings;
using MediatR;

public static class AddExpence
{
    public sealed record Command(ExpenceForCreationDto ExpenceToAdd) : IRequest<ExpenceDto>;

    public sealed class Handler : IRequestHandler<Command, ExpenceDto>
    {
        private readonly IExpenceRepository _expenceRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IExpenceRepository expenceRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _expenceRepository = expenceRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<ExpenceDto> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddExpence);

            var expenceToAdd = request.ExpenceToAdd.ToExpenceForCreation();
            var expence = Expence.Create(expenceToAdd);

            await _expenceRepository.Add(expence, cancellationToken);
            await _unitOfWork.CommitChanges(cancellationToken);

            return expence.ToExpenceDto();
        }
    }
}