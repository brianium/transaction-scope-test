using FluentNHibernate.Mapping;
using TransactionScopeTest.Core.Domain.Model;

namespace TransactionScopeTest.SessionTest.ClassMaps
{
    public class OrderMap : ClassMap<Order>
    {
        public OrderMap()
        {
            Id(o => o.Id).GeneratedBy.Native();
            Map(o => o.ProductId).Not.Nullable();
            Map(o => o.Quantity).Not.Nullable();
        }
    }
}
