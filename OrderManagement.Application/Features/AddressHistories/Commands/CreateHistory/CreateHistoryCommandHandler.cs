using MediatR;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.Application.Features.AddressHistories.Commands.CreateHistory;

public class CreateHistoryCommandHandler(IRepository<AddressHistory> repository)
    : IRequestHandler<CreateHistoryCommand, Guid>
{
    public async Task<Guid> Handle(CreateHistoryCommand request, CancellationToken ct)
    {
        var history = new AddressHistory(
            request.AddressId,
            request.PumpingDate,
            request.CubeAmount,
            request.PaymentType,
            request.Price);

        await repository.AddAsync(history, ct);
        await repository.SaveChangesAsync(ct);
        return history.Id;
    }
}