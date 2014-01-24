using FluentNHibernate.Mapping;
using TransactionScopeTest.Core.Domain.Model;

namespace TransactionScopeTest.SessionTest.ClassMaps
{
    public class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            Id(p => p.Id).GeneratedBy.Native();
            Map(p => p.Name).Not.Nullable();
            Map(p => p.Price).Not.Nullable();
        }
    }
}
