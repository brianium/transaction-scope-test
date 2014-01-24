using FluentMigrator;

namespace TransactionScopeTest.Infrastructure.OrderMigrations
{
    [Migration(201401231919)]
    public class Migration_201401231919 : Migration
    {
        public override void Up()
        {
            Create.Table("Order")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("ProductId").AsInt64().NotNullable()
                .WithColumn("Quantity").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Order");
        }
    }
}
