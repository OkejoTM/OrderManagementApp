using MediatR;
using OrderManagement.Application.Common.Exceptions;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Domain.Specifications.AddressSpecifications;

namespace OrderManagement.Application.Features.Addresses.Commands.CreateAddress;

public class CreateAddressCommandHandler(IRepository<Address> repository) : IRequestHandler<CreateAddressCommand, Guid>
{
    public async Task<Guid> Handle(CreateAddressCommand request, CancellationToken ct)
    {
        var normalized = Address.Normalize(request.Name);

        var duplicateSpec = new AddressByNormalizedNameSpecification(request.AreaId, normalized);
        if (await repository.CountAsync(duplicateSpec, ct) > 0)
        {
            throw new DuplicateAddressException(request.Name);
        }

        var address = new Address(request.AreaId, request.Name);
        await repository.AddAsync(address, ct);
        await repository.SaveChangesAsync(ct);
        return address.Id;
    }
}