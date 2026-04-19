using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Specifications.Base;

namespace OrderManagement.Domain.Specifications.AddressSpecifications;

public class AddressByNormalizedNameSpecification : BaseSpecification<Address>
{
    public AddressByNormalizedNameSpecification(Guid areaId, string normalizedName, Guid? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            SetCriteria(a => a.AreaId == areaId
                             && a.NormalizedName == normalizedName
                             && a.Id != excludeId.Value);
        }
        else
        {
            SetCriteria(a => a.AreaId == areaId && a.NormalizedName == normalizedName);
        }
    }
}