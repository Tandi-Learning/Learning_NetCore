using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PublisherData.Migrations
{
    public partial class InitialSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AuthorFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.BookId);
                    table.ForeignKey(
                        name: "FK_Books_Authors_AuthorFK",
                        column: x => x.AuthorFK,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Covers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DesignIdeas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DigitalOnly = table.Column<bool>(type: "bit", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Covers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Covers_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "BookId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistCover",
                columns: table => new
                {
                    ArtistsId = table.Column<int>(type: "int", nullable: false),
                    CoversId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistCover", x => new { x.ArtistsId, x.CoversId });
                    table.ForeignKey(
                        name: "FK_ArtistCover_Artists_ArtistsId",
                        column: x => x.ArtistsId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArtistCover_Covers_CoversId",
                        column: x => x.CoversId,
                        principalTable: "Covers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Artists",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "Pablo", "Picasso" },
                    { 2, "Dee", "Bell" },
                    { 3, "Katharine", "Kuharic" }
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "Rhoda", "Lerman" },
                    { 2, "Ruth", "Ozeki" },
                    { 3, "Sofia", "Segovia" },
                    { 4, "Ursula K.", "LeGuin" },
                    { 5, "Hugh", "Howey" },
                    { 6, "Isabelle", "Allende" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "AuthorFK", "BasePrice", "PublishDate", "Title" },
                values: new object[] { 1, 1, 0m, new DateTime(1989, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "In God's Ear" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "AuthorFK", "BasePrice", "PublishDate", "Title" },
                values: new object[] { 2, 2, 0m, new DateTime(2013, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "A Tale For the Time Being" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "AuthorFK", "BasePrice", "PublishDate", "Title" },
                values: new object[] { 3, 3, 0m, new DateTime(1969, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "The Left Hand of Darkness" });

            migrationBuilder.InsertData(
                table: "Covers",
                columns: new[] { "Id", "BookId", "DesignIdeas", "DigitalOnly" },
                values: new object[] { 1, 3, "How about a left hand in the dark?", false });

            migrationBuilder.InsertData(
                table: "Covers",
                columns: new[] { "Id", "BookId", "DesignIdeas", "DigitalOnly" },
                values: new object[] { 2, 2, "Should we put a clock?", true });

            migrationBuilder.InsertData(
                table: "Covers",
                columns: new[] { "Id", "BookId", "DesignIdeas", "DigitalOnly" },
                values: new object[] { 3, 1, "A big ear in the clouds?", false });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistCover_CoversId",
                table: "ArtistCover",
                column: "CoversId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_AuthorFK",
                table: "Books",
                column: "AuthorFK");

            migrationBuilder.CreateIndex(
                name: "IX_Covers_BookId",
                table: "Covers",
                column: "BookId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistCover");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Covers");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
