using MediatR;
using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Features.Addresses.Queries.SearchAddresses;

public record SearchAddressesQuery(
    string NameFilter,
    int PageNumber = 1,
    int PageSize = 8) : IRequest<PagedResult<AddressSearchResultDto>>;