using MediatR;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.Application.Features.Areas.Commands.DeleteArea;

public class DeleteAreaCommandHandler(IRepository<Area> repository) : IRequestHandler<DeleteAreaCommand>
{
    public async Task Handle(DeleteAreaCommand request, CancellationToken ct)
    {
        var area = await repository.GetByIdAsync(request.Id, ct)
                   ?? throw new InvalidOperationException($"Area with id {request.Id} not found.");

        repository.Delete(area);
        await repository.SaveChangesAsync(ct);
    }
}