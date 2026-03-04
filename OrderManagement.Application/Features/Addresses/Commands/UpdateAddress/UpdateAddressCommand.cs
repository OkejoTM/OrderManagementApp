using MediatR;

namespace OrderManagement.Application.Features.Addresses.Commands.UpdateAddress;

public record UpdateAddressCommand(Guid Id, string Name) : IRequest;