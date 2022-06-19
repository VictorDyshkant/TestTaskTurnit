using FluentNHibernate.Mapping;
using Turnit.Abstraction.Entities;
namespace Turnit.Database.Entities;

public class CategoryMap : ClassMap<Category>
{
    public CategoryMap()
    {
        Schema("public");
        Table("category");

        Id(x => x.Id, "id");
        Map(x => x.Name, "name");
    }
}