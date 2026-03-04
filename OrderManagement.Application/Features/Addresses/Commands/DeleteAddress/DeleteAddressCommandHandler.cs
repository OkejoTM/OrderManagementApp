using MediatR;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;

namespace OrderManagement.Application.Features.Addresses.Commands.DeleteAddress;

public class DeleteAddressCommandHandler(IRepository<Address> repository) : IRequestHandler<DeleteAddressCommand>
{
    public async Task Handle(DeleteAddressCommand request, CancellationToken ct)
    {
        var address = await repository.GetByIdAsync(request.Id, ct)
                      ?? throw new InvalidOperationException($"Address with id {request.Id} not found.");

        repository.Delete(address);
        await repository.SaveChangesAsync(ct);
    }
}