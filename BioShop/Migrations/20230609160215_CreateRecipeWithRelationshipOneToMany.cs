using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BioShop.Migrations
{
    /// <inheritdoc />
    public partial class CreateRecipeWithRelationshipOneToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Portions = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<double>(type: "float", nullable: false),
                    NecesseryProductsAndQuantity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DesciptionStepByStepHowToBeMade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeYouNeedToBeMade = table.Column<double>(type: "float", nullable: false),
                    CurrentProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_Products_CurrentProductId",
                        column: x => x.CurrentProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CurrentProductId",
                table: "Recipes",
                column: "CurrentProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
