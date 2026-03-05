using MediatR;
using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Features.Addresses.Queries.GetAddresses;

public record GetAddressesQuery(
    Guid AreaId,
    string? NameFilter = null,
    bool OrderByNameDescending = false,
    int PageNumber = 1,
    int PageSize = 15) : IRequest<PagedResult<AddressDto>>;