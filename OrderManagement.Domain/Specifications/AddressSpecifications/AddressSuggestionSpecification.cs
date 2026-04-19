using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Specifications.Base;

namespace OrderManagement.Domain.Specifications.AddressSpecifications;

public class AddressSuggestionSpecification : BaseSpecification<Address>
{
    public AddressSuggestionSpecification(
        Guid areaId,
        string pattern,
        AddressSuggestionMatchType matchType,
        int take)
    {
        AddInclude(a => a.Histories);

        switch (matchType)
        {
            case AddressSuggestionMatchType.Exact:
                SetCriteria(a => a.AreaId == areaId && a.NormalizedName == pattern);
                break;
            case AddressSuggestionMatchType.Contains:
                SetCriteria(a => a.AreaId == areaId && a.NormalizedName.Contains(pattern));
                break;
            case AddressSuggestionMatchType.StartsWith:
                SetCriteria(a => a.AreaId == areaId && a.NormalizedName.StartsWith(pattern));
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(matchType), matchType, null);
        }

        ApplyOrderBy(a => a.Name);
        ApplyPaging(0, take);
    }
}