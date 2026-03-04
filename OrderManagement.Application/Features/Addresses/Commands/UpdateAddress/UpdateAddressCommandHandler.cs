using MediatR;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.Application.Features.Addresses.Commands.UpdateAddress;

public class UpdateAddressCommandHandler(IRepository<Address> repository) : IRequestHandler<UpdateAddressCommand>
{
    public async Task Handle(UpdateAddressCommand request, CancellationToken ct)
    {
        var address = await repository.GetByIdAsync(request.Id, ct)
                      ?? throw new InvalidOperationException($"Address with id {request.Id} not found.");

        address.UpdateName(request.Name);
        repository.Update(address);
        await repository.SaveChangesAsync(ct);
    }
}