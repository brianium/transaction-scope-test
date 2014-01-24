namespace TransactionScopeTest.Core.Domain.Model
{
    public class Product : EntityBase<Product>
    {
        public virtual string Name { get; set; }
        public virtual decimal Price { get; set; }
    }
}
