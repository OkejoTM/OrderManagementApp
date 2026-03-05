using MediatR;

namespace OrderManagement.Application.Features.Addresses.Commands.CreateAddress;

public record CreateAddressCommand(Guid AreaId, string Name) : IRequest<Guid>;