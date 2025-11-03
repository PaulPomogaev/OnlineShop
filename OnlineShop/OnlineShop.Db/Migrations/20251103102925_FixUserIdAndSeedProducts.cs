using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OnlineShop.Db.Migrations
{
    /// <inheritdoc />
    public partial class FixUserIdAndSeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserId1",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_UserId1",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Cost", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 8500m, "Сывороточный протеин самого высокого класса", "Протеин-супер Optimum Nutrition 990 g" },
                    { 2, 5500m, "Инсулиноподобный фактор роста пролонгированного действия индуцирует рост мышц и регенерацию", "IGF-1 LR3 1 mg" },
                    { 3, 3500m, "BCAA обеспечивают энергию и защищают мышцы во время тренировки", "BCAA 500 g" },
                    { 4, 4500m, "Аминокислоты ускоряют восстановление мышечной ткани после тренировки", "Комплексные аминокислоты 500 g" },
                    { 5, 5000m, "Заменяет собой пищу, обеспечивая энергией и белком после тренировки и при дефиците пищи", "Gainer Mutant 5,5 kg" },
                    { 6, 4900m, "Креатин цитрат обеспечивает силовую выносливость, лучшую работу мозга и сердца и не вызывает отёков", "Creatine citrate 500 g" },
                    { 7, 2999m, "Предтрен стимулирует нервную систему и улучшает силовую выносливость, что напрямую влияет на физическую производительность в период тренировки.", "Psychotic предтрен" },
                    { 8, 3990m, "Препарат активирует фермент теломеразу, индуцирует удлинение теломер, предовращает появление онкологических заболеваний, продляет жизнь и улучшает качество жизни......", "Epithalon 10 mg / 2 vials" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId1",
                table: "Orders",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserId1",
                table: "Orders",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
