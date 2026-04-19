using MediatR;
using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Features.Addresses.Queries.GetAddressSuggestions;

public record GetAddressSuggestionsQuery(Guid AreaId, string NameInput)
    : IRequest<IReadOnlyList<AddressDto>>;