using MediatR;
using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Features.Areas.Queries.GetAreas;

public record GetAreasQuery : IRequest<IReadOnlyList<AreaDto>>;