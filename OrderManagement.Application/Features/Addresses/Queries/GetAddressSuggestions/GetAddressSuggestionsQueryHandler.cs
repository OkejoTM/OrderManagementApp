using System.Text.RegularExpressions;
using MediatR;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Mapping;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Interfaces;
using OrderManagement.Domain.Specifications.AddressSpecifications;

namespace OrderManagement.Application.Features.Addresses.Queries.GetAddressSuggestions;

public class GetAddressSuggestionsQueryHandler(IRepository<Address> repository)
    : IRequestHandler<GetAddressSuggestionsQuery, IReadOnlyList<AddressDto>>
{
    private const int MaxSuggestions = 3;
    private const int MinInputLength = 2;
    private const int PrefixFallbackLength = 3;

    public async Task<IReadOnlyList<AddressDto>> Handle(
        GetAddressSuggestionsQuery request, CancellationToken ct)
    {
        var normalized = Address.Normalize(request.NameInput);
        if (normalized.Length < MinInputLength)
            return Array.Empty<AddressDto>();

        var results = new List<Address>(MaxSuggestions);
        var seen = new HashSet<Guid>();

        await AppendAsync(
            new AddressSuggestionSpecification(
                request.AreaId, normalized, AddressSuggestionMatchType.Exact, MaxSuggestions),
            results, seen, ct);

        if (results.Count < MaxSuggestions)
        {
            await AppendAsync(
                new AddressSuggestionSpecification(
                    request.AreaId, normalized, AddressSuggestionMatchType.Contains, MaxSuggestions),
                results, seen, ct);
        }

        if (results.Count < MaxSuggestions)
        {
            var numberMatch = Regex.Match(normalized, @"\d+");
            if (numberMatch.Success)
            {
                await AppendAsync(
                    new AddressSuggestionSpecification(
                        request.AreaId, numberMatch.Value, AddressSuggestionMatchType.Contains, MaxSuggestions),
                    results, seen, ct);
            }
            else
            {
                var prefix = normalized.Length >= PrefixFallbackLength
                    ? normalized[..PrefixFallbackLength]
                    : normalized;

                await AppendAsync(
                    new AddressSuggestionSpecification(
                        request.AreaId, prefix, AddressSuggestionMatchType.StartsWith, MaxSuggestions),
                    results, seen, ct);
            }
        }

        return ManualMapper.ToDtoList(results);
    }

    private async Task AppendAsync(
        AddressSuggestionSpecification spec,
        List<Address> results,
        HashSet<Guid> seen,
        CancellationToken ct)
    {
        if (results.Count >= MaxSuggestions) return;

        var page = await repository.ListAsync(spec, ct);
        foreach (var a in page)
        {
            if (results.Count >= MaxSuggestions) break;
            if (seen.Add(a.Id)) results.Add(a);
        }
    }
}