using MediatR;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Mapping;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Domain.Specifications.HistorySpecifications;

namespace OrderManagement.Application.Features.AddressHistories.Queries.GetHistories;

public class GetHistoriesQueryHandler(IRepository<AddressHistory> repository)
    : IRequestHandler<GetHistoriesQuery, IReadOnlyList<AddressHistoryDto>>
{
    public async Task<IReadOnlyList<AddressHistoryDto>> Handle(GetHistoriesQuery request, CancellationToken ct)
    {
        var spec = new HistoryByAddressSpecification(
            request.AddressId, request.DateFrom, request.DateTo, request.OrderByDateDescending);

        var histories = await repository.ListAsync(spec, ct);
        return ManualMapper.ToDtoList(histories);
    }
}