using MediatR;
using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Features.AddressHistories.Queries.GetHistories;

public record GetHistoriesQuery(
    Guid AddressId,
    DateOnly? DateFrom = null,
    DateOnly? DateTo = null,
    bool OrderByDateDescending = true) : IRequest<IReadOnlyList<AddressHistoryDto>>;