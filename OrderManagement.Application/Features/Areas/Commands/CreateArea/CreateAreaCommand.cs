using MediatR;

namespace OrderManagement.Application.Features.Areas.Commands.CreateArea;

public record CreateAreaCommand(string Name) : IRequest<Guid>;