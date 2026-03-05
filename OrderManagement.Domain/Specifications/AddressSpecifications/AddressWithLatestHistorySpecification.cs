using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Specifications.Base;

namespace OrderManagement.Domain.Specifications.AddressSpecifications;

public class AddressWithLatestHistorySpecification : BaseSpecification<Address>
{
    public AddressWithLatestHistorySpecification(
        Guid areaId,
        string? nameFilter = null,
        bool orderByNameDescending = false,
        int? pageNumber = null,
        int? pageSize = null)
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

        if (pageNumber.HasValue && pageSize.HasValue)
        {
            ApplyPaging((pageNumber.Value - 1) * pageSize.Value, pageSize.Value);
        }
    }
}