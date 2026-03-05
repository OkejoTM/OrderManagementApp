using MediatR;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.Application.Features.Addresses.Commands.CreateAddress;

public class CreateAddressCommandHandler(IRepository<Address> repository) : IRequestHandler<CreateAddressCommand, Guid>
{
    public async Task<Guid> Handle(CreateAddressCommand request, CancellationToken ct)
    {
        var address = new Address(request.AreaId, request.Name);
        await repository.AddAsync(address, ct);
        await repository.SaveChangesAsync(ct);
        return address.Id;
    }
}