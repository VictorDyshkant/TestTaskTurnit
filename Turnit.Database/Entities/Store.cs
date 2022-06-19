using FluentNHibernate.Mapping;
using Turnit.Abstraction.Entities;

namespace Turnit.Database.Entities;

public class StoreMap : ClassMap<Store>
{
    public StoreMap()
    {
        Schema("public");
        Table("store");

        Id(x => x.Id, "id");
        Map(x => x.Name, "name");
    }
}