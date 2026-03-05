using MediatR;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.Application.Features.Areas.Commands.CreateArea;

public class CreateAreaCommandHandler(IRepository<Area> repository) : IRequestHandler<CreateAreaCommand, Guid>
{
    public async Task<Guid> Handle(CreateAreaCommand request, CancellationToken ct)
    {
        var area = new Area(request.Name);
        await repository.AddAsync(area, ct);
        await repository.SaveChangesAsync(ct);
        return area.Id;
    }
}