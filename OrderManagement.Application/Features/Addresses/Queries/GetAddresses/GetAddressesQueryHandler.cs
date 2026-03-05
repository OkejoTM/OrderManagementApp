using MediatR;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Mapping;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Domain.Specifications.AddressSpecifications;

namespace OrderManagement.Application.Features.Addresses.Queries.GetAddresses;

public class GetAddressesQueryHandler(IRepository<Address> repository)
    : IRequestHandler<GetAddressesQuery, PagedResult<AddressDto>>
{
    public async Task<PagedResult<AddressDto>> Handle(GetAddressesQuery request, CancellationToken ct)
    {
        var countSpec = new AddressWithLatestHistorySpecification(request.AreaId, request.NameFilter, request.OrderByNameDescending);
        var totalCount = await repository.CountAsync(countSpec, ct);

        var spec = new AddressWithLatestHistorySpecification(
            request.AreaId, request.NameFilter, request.OrderByNameDescending,
            request.PageNumber, request.PageSize);

        var addresses = await repository.ListAsync(spec, ct);

        return new PagedResult<AddressDto>
        {
            Items = ManualMapper.ToDtoList(addresses),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}