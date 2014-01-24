using FluentMigrator;

namespace TransactionScopeTest.Infrastructure.ProductMigrations
{
    [Migration(201401231913)]
    public class Migration_201401231913 : Migration
    {
        public override void Up()
        {
            Create.Table("Product")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey().Identity()
                .WithColumn("Name").AsString(255).NotNullable().Unique()
                .WithColumn("Price").AsDecimal().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Product");
        }
    }
}
