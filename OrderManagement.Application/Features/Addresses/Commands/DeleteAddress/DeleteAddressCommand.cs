using MediatR;

namespace OrderManagement.Application.Features.Addresses.Commands.DeleteAddress;

public record DeleteAddressCommand(Guid Id) : IRequest;