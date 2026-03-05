using MediatR;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Mapping;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Domain.Specifications.AddressSpecifications;

namespace OrderManagement.Application.Features.Addresses.Queries.GetAddresses;

public class GetAddressesQueryHandler(IRepository<Address> repository)
    : IRequestHandler<GetAddressesQuery, IReadOnlyList<AddressDto>>
{
    public async Task<IReadOnlyList<AddressDto>> Handle(GetAddressesQuery request, CancellationToken ct)
    {
        var spec = new AddressWithLatestHistorySpecification(request.AreaId, request.NameFilter, request.OrderByNameDescending);
        var addresses = await repository.ListAsync(spec, ct);
        return ManualMapper.ToDtoList(addresses);
    }
}