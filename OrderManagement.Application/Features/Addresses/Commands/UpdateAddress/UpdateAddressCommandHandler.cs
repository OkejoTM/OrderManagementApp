using MediatR;
using OrderManagement.Application.Common.Exceptions;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Domain.Specifications.AddressSpecifications;

namespace OrderManagement.Application.Features.Addresses.Commands.UpdateAddress;

public class UpdateAddressCommandHandler(IRepository<Address> repository) : IRequestHandler<UpdateAddressCommand>
{
    public async Task Handle(UpdateAddressCommand request, CancellationToken ct)
    {
        var address = await repository.GetByIdAsync(request.Id, ct)
                      ?? throw new InvalidOperationException($"Address with id {request.Id} not found.");

        var normalized = Address.Normalize(request.Name);

        var duplicateSpec = new AddressByNormalizedNameSpecification(address.AreaId, normalized, request.Id);
        if (await repository.CountAsync(duplicateSpec, ct) > 0)
        {
            throw new DuplicateAddressException(request.Name);
        }

        address.UpdateName(request.Name);
        repository.Update(address);
        await repository.SaveChangesAsync(ct);
    }
}