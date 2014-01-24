namespace TransactionScopeTest.Core.Domain.Model
{
    public class Order : EntityBase<Order>
    {
        /// <summary>
        /// The product is on a different database
        /// </summary>
        public virtual long ProductId { get; set; }

        public virtual int Quantity { get; set; }
    }
}
