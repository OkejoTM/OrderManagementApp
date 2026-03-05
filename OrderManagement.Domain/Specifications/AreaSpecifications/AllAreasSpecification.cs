using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Specifications.Base;

namespace OrderManagement.Domain.Specifications.AreaSpecifications;

public class AllAreasSpecification : BaseSpecification<Area>
{
    public AllAreasSpecification()
    {
        ApplyOrderBy(a => a.Name);
    }
}