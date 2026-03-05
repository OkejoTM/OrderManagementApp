using MediatR;

namespace OrderManagement.Application.Features.Areas.Commands.UpdateArea;

public record UpdateAreaCommand(Guid Id, string Name) : IRequest;