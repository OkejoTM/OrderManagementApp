using MediatR;

namespace OrderManagement.Application.Features.Areas.Commands.DeleteArea;

public record DeleteAreaCommand(Guid Id) : IRequest;