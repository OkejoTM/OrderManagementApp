using MediatR;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.Features.AddressHistories.Commands.UpdateHistory;

public record UpdateHistoryCommand(
    Guid Id,
    DateOnly PumpingDate,
    int CubeAmount,
    PaymentType PaymentType,
    double Price) : IRequest;