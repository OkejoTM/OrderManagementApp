using MediatR;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Mapping;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Domain.Specifications.AreaSpecifications;

namespace OrderManagement.Application.Features.Areas.Queries.GetAreas;

public class GetAreasQueryHandler(IRepository<Area> repository)
    : IRequestHandler<GetAreasQuery, IReadOnlyList<AreaDto>>
{
    public async Task<IReadOnlyList<AreaDto>> Handle(GetAreasQuery request, CancellationToken ct)
    {
        var spec = new AllAreasSpecification();
        var areas = await repository.ListAsync(spec, ct);
        return ManualMapper.ToDtoList(areas);
    }
}