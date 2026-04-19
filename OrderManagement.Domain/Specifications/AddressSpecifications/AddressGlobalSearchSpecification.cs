using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Specifications.Base;

namespace OrderManagement.Domain.Specifications.AddressSpecifications;

public class AddressGlobalSearchSpecification : BaseSpecification<Address>
{
    public AddressGlobalSearchSpecification(
        string normalizedFilter,
        int? pageNumber = null,
        int? pageSize = null)
    {
        AddInclude(a => a.Area);
        AddInclude(a => a.Histories);

        SetCriteria(a => a.NormalizedName.Contains(normalizedFilter));

        ApplyOrderBy(a => a.Name);

        if (pageNumber.HasValue && pageSize.HasValue)
        {
            ApplyPaging((pageNumber.Value - 1) * pageSize.Value, pageSize.Value);
        }
    }
}