using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Specifications.Base;

namespace OrderManagement.Domain.Specifications.AddressSpecifications;

public class AddressWithLatestHistorySpecification : BaseSpecification<Address>
{
    public AddressWithLatestHistorySpecification(Guid areaId, string? nameFilter = null, bool orderByNameDescending = false)
    {
        AddInclude(a => a.Histories);

        if (!string.IsNullOrWhiteSpace(nameFilter))
        {
            SetCriteria(a => a.AreaId == areaId && a.Name.ToLower().Contains(nameFilter.ToLower()));
        }
        else
        {
            SetCriteria(a => a.AreaId == areaId);
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