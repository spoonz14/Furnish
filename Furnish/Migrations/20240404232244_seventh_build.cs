using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Furnish.Migrations
{
    /// <inheritdoc />
    public partial class seventh_build : Migration
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
                    loginId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => x.loginId);
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
                    { 1, "Sofa", "C:\\Projects\\Furnish\\Furnish\\wwwroot\\images\\navi-sofa.jpg", "Navi", 649.99000000000001 },
                    { 2, "Bed", null, "Serta Peninsula", 899.99000000000001 },
                    { 3, "Table", null, "Mobili Fiver", 945.99000000000001 },
                    { 4, "Chair", null, "Ashley Cashton", 2299.9899999999998 },
                    { 5, "Sofa", null, "Llappuil", 649.99000000000001 },
                    { 6, "Bed", null, "Yuhuashi", 247.0 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "userId", "Email", "GivenName", "Password", "Role", "Surname", "Username" },
                values: new object[,]
                {
                    { 1, "sbarr@shaw.ca", "Spencer", "Abcd1234", "Administrator", "Barr", "sbarr" },
                    { 2, "cbarr@shaw.ca", "Corrine", "Abcd1234", "Buyer", "Barr", "cbarr" }
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
