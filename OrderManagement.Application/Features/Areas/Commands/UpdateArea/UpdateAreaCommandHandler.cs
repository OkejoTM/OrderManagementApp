using MediatR;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.Application.Features.Areas.Commands.UpdateArea;

public class UpdateAreaCommandHandler(IRepository<Area> repository) : IRequestHandler<UpdateAreaCommand>
{
    public async Task Handle(UpdateAreaCommand request, CancellationToken ct)
    {
        var area = await repository.GetByIdAsync(request.Id, ct)
                   ?? throw new InvalidOperationException($"Area with id {request.Id} not found.");

        area.UpdateName(request.Name);
        repository.Update(area);
        await repository.SaveChangesAsync(ct);
    }
}