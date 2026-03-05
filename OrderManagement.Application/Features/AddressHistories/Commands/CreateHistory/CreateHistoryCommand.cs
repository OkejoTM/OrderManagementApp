using MediatR;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Features.AddressHistories.Commands.CreateHistory;

public record CreateHistoryCommand(
    Guid AddressId,
    DateOnly PumpingDate,
    double CubeAmount,
    PaymentType PaymentType,
    double Price) : IRequest<Guid>;