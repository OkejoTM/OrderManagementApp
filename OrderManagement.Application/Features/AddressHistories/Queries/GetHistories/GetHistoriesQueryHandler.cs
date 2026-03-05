using MediatR;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Mapping;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Domain.Specifications.HistorySpecifications;

namespace OrderManagement.Application.Features.AddressHistories.Queries.GetHistories;

public class GetHistoriesQueryHandler(IRepository<AddressHistory> repository)
    : IRequestHandler<GetHistoriesQuery, PagedResult<AddressHistoryDto>>
{
    public async Task<PagedResult<AddressHistoryDto>> Handle(GetHistoriesQuery request, CancellationToken ct)
    {
        var countSpec = new HistoryByAddressSpecification(
            request.AddressId, request.DateFrom, request.DateTo, request.OrderByDateDescending);
        var totalCount = await repository.CountAsync(countSpec, ct);

        var spec = new HistoryByAddressSpecification(
            request.AddressId, request.DateFrom, request.DateTo, request.OrderByDateDescending,
            request.PageNumber, request.PageSize);

        var histories = await repository.ListAsync(spec, ct);

        return new PagedResult<AddressHistoryDto>
        {
            Items = ManualMapper.ToDtoList(histories),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}