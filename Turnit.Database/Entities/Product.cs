using FluentNHibernate.Mapping;
using Turnit.Abstraction.Entities;

namespace Turnit.Database.Entities;

public class ProductMap : ClassMap<Product>
{
    public ProductMap()
    {
        Schema("public");
        Table("product");

        Id(x => x.Id, "id");
        Map(x => x.Name, "name");
        Map(x => x.Description, "description");
    }
}