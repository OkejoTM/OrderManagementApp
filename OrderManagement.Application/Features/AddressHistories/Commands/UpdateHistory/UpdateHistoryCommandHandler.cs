using MediatR;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.Application.Features.AddressHistories.Commands.UpdateHistory;

public class UpdateHistoryCommandHandler(IRepository<AddressHistory> repository) : IRequestHandler<UpdateHistoryCommand>
{
    public async Task Handle(UpdateHistoryCommand request, CancellationToken ct)
    {
        var history = await repository.GetByIdAsync(request.Id, ct)
                      ?? throw new InvalidOperationException($"History with id {request.Id} not found.");

        history.Update(request.PumpingDate, request.CubeAmount, request.PaymentType, request.Price);
        repository.Update(history);
        await repository.SaveChangesAsync(ct);
    }
}