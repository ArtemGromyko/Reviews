using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Reviews.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductActor");

            migrationBuilder.DropTable(
                name: "ProductDirector");

            migrationBuilder.AlterColumn<string>(
                name: "Slogan",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ActorsProducts",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActorsProducts", x => new { x.PersonId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ActorsProducts_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActorsProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DirectorsProducts",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectorsProducts", x => new { x.PersonId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_DirectorsProducts_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DirectorsProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonId", "BirthDate", "BirthPlace", "Height", "Name" },
                values: new object[,]
                {
                    { new Guid("53a1237a-3ed3-4462-b9f0-5a7bb1056a33"), new DateTime(1967, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Richmond, Virginia, USA", 1.8300000000000001, "Vince Gilligan" },
                    { new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"), new DateTime(1965, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "San Fernando, California, USA", 1.79, "Bryan Cranston" },
                    { new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"), new DateTime(1979, 8, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "Emmett, Idaho, USA", 1.73, "Aaron Paul" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "Category", "Country", "Genre", "Name", "ReleaseDate", "Slogan" },
                values: new object[] { new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), "series", "USA", "thriller, crime, dramas", "Breaking Bad", new DateTime(2008, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "In the no-holds-barred world of Walt White, the end justifies the extreme" });

            migrationBuilder.InsertData(
                table: "ActorsProducts",
                columns: new[] { "PersonId", "ProductId" },
                values: new object[,]
                {
                    { new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"), new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870") },
                    { new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"), new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870") }
                });

            migrationBuilder.InsertData(
                table: "DirectorsProducts",
                columns: new[] { "PersonId", "ProductId" },
                values: new object[] { new Guid("53a1237a-3ed3-4462-b9f0-5a7bb1056a33"), new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870") });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "ReviewId", "Heading", "ProductId", "Raiting", "Text" },
                values: new object[,]
                {
                    { new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"), "Chemistry and life", new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), 9, "review text1" },
                    { new Guid("80abbca8-664d-4b20-b5de-024705497d4a"), "Walt still has time", new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"), 8, "review text2" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActorsProducts_ProductId",
                table: "ActorsProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DirectorsProducts_ProductId",
                table: "DirectorsProducts",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActorsProducts");

            migrationBuilder.DropTable(
                name: "DirectorsProducts");

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"));

            migrationBuilder.DeleteData(
                table: "Reviews",
                keyColumn: "ReviewId",
                keyValue: new Guid("80abbca8-664d-4b20-b5de-024705497d4a"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("021ca3c1-0deb-4afd-ae94-2159a8479811"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("53a1237a-3ed3-4462-b9f0-5a7bb1056a33"));

            migrationBuilder.DeleteData(
                table: "Persons",
                keyColumn: "PersonId",
                keyValue: new Guid("86dba8c0-d178-41e7-938c-ed49778fb52a"));

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: new Guid("c9d4c053-49b6-410c-bc78-2d54a9991870"));

            migrationBuilder.AlterColumn<string>(
                name: "Slogan",
                table: "Products",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProductActor",
                columns: table => new
                {
                    ActorsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductsActorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductActor", x => new { x.ActorsId, x.ProductsActorId });
                    table.ForeignKey(
                        name: "FK_ProductActor_Persons_ActorsId",
                        column: x => x.ActorsId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductActor_Products_ProductsActorId",
                        column: x => x.ProductsActorId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDirector",
                columns: table => new
                {
                    DirectorsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductsDirectorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDirector", x => new { x.DirectorsId, x.ProductsDirectorId });
                    table.ForeignKey(
                        name: "FK_ProductDirector_Persons_DirectorsId",
                        column: x => x.DirectorsId,
                        principalTable: "Persons",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductDirector_Products_ProductsDirectorId",
                        column: x => x.ProductsDirectorId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductActor_ProductsActorId",
                table: "ProductActor",
                column: "ProductsActorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDirector_ProductsDirectorId",
                table: "ProductDirector",
                column: "ProductsDirectorId");
        }
    }
}
