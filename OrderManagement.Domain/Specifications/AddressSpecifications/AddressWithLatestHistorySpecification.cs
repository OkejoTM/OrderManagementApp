using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Specifications.Base;

namespace OrderManagement.Domain.Specifications.AddressSpecifications;

public class AddressWithLatestHistorySpecification : BaseSpecification<Address>
{
    public AddressWithLatestHistorySpecification(string? nameFilter = null, bool orderByNameDescending = false)
    {
        AddInclude(a => a.Histories);

        if (!string.IsNullOrWhiteSpace(nameFilter))
        {
            SetCriteria(a => a.Name.ToLower().Contains(nameFilter.ToLower()));
        }

        if (orderByNameDescending)
        {
            ApplyOrderByDescending(a => a.Name);
        }
        else
        {
            ApplyOrderBy(a => a.Name);
        }
    }
}