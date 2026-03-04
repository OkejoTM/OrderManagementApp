using MediatR;

namespace OrderManagement.Application.Features.Addresses.Commands.CreateAddress;

public record CreateAddressCommand(string Name) : IRequest<Guid>;