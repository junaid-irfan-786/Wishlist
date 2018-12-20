using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WishList.Migrations
{
    public partial class mgr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StoreName = table.Column<string>(nullable: true),
                    customer_id = table.Column<string>(nullable: true),
                    customer_email = table.Column<string>(nullable: true),
                    customer_name = table.Column<string>(nullable: true),
                    product_id = table.Column<string>(nullable: true),
                    product_title = table.Column<string>(nullable: true),
                    variant_id = table.Column<string>(nullable: true),
                    variant_image = table.Column<string>(nullable: true),
                    variant_sku = table.Column<string>(nullable: true),
                    variant_barcode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerID);
                });

            migrationBuilder.CreateTable(
                name: "Shopify",
                columns: table => new
                {
                    ShopifyID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StoreName = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shopify", x => x.ShopifyID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Shopify");
        }
    }
}
