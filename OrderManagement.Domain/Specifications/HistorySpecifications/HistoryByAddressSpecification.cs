using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Specifications.Base;

namespace OrderManagement.Domain.Specifications.HistorySpecifications;

public class HistoryByAddressSpecification : BaseSpecification<AddressHistory>
{
    public HistoryByAddressSpecification(
        Guid addressId,
        DateOnly? dateFrom = null,
        DateOnly? dateTo = null,
        bool orderByDateDescending = true,
        int? pageNumber = null,
        int? pageSize = null)
    {
        if (dateFrom.HasValue && dateTo.HasValue)
        {
            SetCriteria(h => h.AddressId == addressId
                             && h.PumpingDate >= dateFrom.Value
                             && h.PumpingDate <= dateTo.Value);
        }
        else if (dateFrom.HasValue)
        {
            SetCriteria(h => h.AddressId == addressId
                             && h.PumpingDate >= dateFrom.Value);
        }
        else if (dateTo.HasValue)
        {
            SetCriteria(h => h.AddressId == addressId
                             && h.PumpingDate <= dateTo.Value);
        }
        else
        {
            SetCriteria(h => h.AddressId == addressId);
        }

        if (orderByDateDescending)
        {
            ApplyOrderByDescending(h => h.PumpingDate);
        }
        else
        {
            ApplyOrderBy(h => h.PumpingDate);
        }

        if (pageNumber.HasValue && pageSize.HasValue)
        {
            ApplyPaging((pageNumber.Value - 1) * pageSize.Value, pageSize.Value);
        }
    }
}