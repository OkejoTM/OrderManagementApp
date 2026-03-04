using MediatR;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.Application.Features.AddressHistories.Commands.DeleteHistory;

public class DeleteHistoryCommandHandler(IRepository<AddressHistory> repository) : IRequestHandler<DeleteHistoryCommand>
{
    public async Task Handle(DeleteHistoryCommand request, CancellationToken ct)
    {
        var history = await repository.GetByIdAsync(request.Id, ct)
                      ?? throw new InvalidOperationException($"History with id {request.Id} not found.");

        repository.Delete(history);
        await repository.SaveChangesAsync(ct);
    }
}