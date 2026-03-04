using MediatR;
using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Features.Addresses.Queries.GetAddresses;

public record GetAddressesQuery(
    string? NameFilter = null,
    bool OrderByNameDescending = false) : IRequest<IReadOnlyList<AddressDto>>;