using MediatR;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Mapping;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Domain.Specifications.AddressSpecifications;

namespace OrderManagement.Application.Features.Addresses.Queries.SearchAddresses;

public class SearchAddressesQueryHandler(IRepository<Address> repository)
    : IRequestHandler<SearchAddressesQuery, PagedResult<AddressSearchResultDto>>
{
    public async Task<PagedResult<AddressSearchResultDto>> Handle(
        SearchAddressesQuery request, CancellationToken ct)
    {
        var normalized = Address.Normalize(request.NameFilter);

        if (string.IsNullOrEmpty(normalized))
        {
            return new PagedResult<AddressSearchResultDto>
            {
                Items = Array.Empty<AddressSearchResultDto>(),
                TotalCount = 0,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        var countSpec = new AddressGlobalSearchSpecification(normalized);
        var totalCount = await repository.CountAsync(countSpec, ct);

        var spec = new AddressGlobalSearchSpecification(
            normalized, request.PageNumber, request.PageSize);
        var addresses = await repository.ListAsync(spec, ct);

        return new PagedResult<AddressSearchResultDto>
        {
            Items = ManualMapper.ToSearchDtoList(addresses),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize
        };
    }
}