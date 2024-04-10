using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Furnish.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GivenName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryId", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Sofa", "~/images/sofa.png", "Navi", 649.99000000000001 },
                    { 2, "Bed", "~/images/WhiteBed.png", "Serta Peninsula", 899.99000000000001 },
                    { 3, "Table", "~/images/table.png", "Mobili Fiver", 945.99000000000001 },
                    { 4, "Chair", "~/images/product-1.png", "Ashley Cashton", 2299.9899999999998 },
                    { 5, "Sofa", "~/images/couch.png", "Llappuil", 649.99000000000001 },
                    { 6, "Bed", "~/images/yuhuashi.png", "Yuhuashi", 247.0 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "userId", "Address", "Email", "GivenName", "Password", "Role", "Surname", "Username" },
                values: new object[,]
                {
                    { 1, "123 Goose Arm Street", "sbarr@shaw.ca", "Spencer", "Abcd1234", "Administrator", "Barr", "sbarr" },
                    { 2, "21 Marina Court", "cbarr@shaw.ca", "Corrine", "Abcd1234", "Buyer", "Barr", "cbarr" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "UserLogin");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
