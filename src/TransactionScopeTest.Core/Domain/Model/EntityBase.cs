namespace TransactionScopeTest.Core.Domain.Model
{
    public class EntityBase<T> where T : EntityBase<T>
    {
        public virtual long Id { get; set; }
    }
}
