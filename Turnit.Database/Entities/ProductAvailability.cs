using FluentNHibernate.Mapping;
using Turnit.Abstraction.Entities;

namespace Turnit.Database.Entities;

public class ProductAvailabilityMap : ClassMap<ProductAvailability>
{
    public ProductAvailabilityMap()
    {
        Schema("public");
        Table("product_availability");

        Id(x => x.Id, "id");
        Map(x => x.Availability, "availability");
        References(x => x.Store, "store_id");
        References(x => x.Product, "product_id");
    }
}