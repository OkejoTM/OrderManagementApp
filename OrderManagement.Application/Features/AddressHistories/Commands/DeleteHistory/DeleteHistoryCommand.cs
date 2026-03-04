using MediatR;

namespace OrderManagement.Application.Features.AddressHistories.Commands.DeleteHistory;

public record DeleteHistoryCommand(Guid Id) : IRequest;