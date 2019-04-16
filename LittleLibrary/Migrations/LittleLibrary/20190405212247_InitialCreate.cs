using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LittleLibrary.Migrations.LittleLibrary
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    authorID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    firstname = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    lastname = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    email = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    streetAddress = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    city = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    province = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    country = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    postalCode = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    authorProfile = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.authorID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    authorizedUserId = table.Column<string>(nullable: true),
                    email = table.Column<string>(unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userID);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    bookID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    title = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    authorID = table.Column<int>(nullable: true),
                    genre = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    datePublished = table.Column<DateTime>(type: "date", nullable: true),
                    price = table.Column<decimal>(type: "money", nullable: true),
                    summary = table.Column<string>(type: "text", nullable: true),
                    bookCover = table.Column<byte[]>(nullable: true),
                    bookContent = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.bookID);
                    table.ForeignKey(
                        name: "FK__Books__authorID__6E01572D",
                        column: x => x.authorID,
                        principalTable: "Authors",
                        principalColumn: "authorID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersBooks",
                columns: table => new
                {
                    UserName = table.Column<string>(nullable: true),
                    userBookID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    userID = table.Column<int>(nullable: true),
                    bookID = table.Column<int>(nullable: true),
                    review = table.Column<string>(unicode: false, maxLength: 60, nullable: true),
                    reviewDate = table.Column<DateTime>(type: "date", nullable: true),
                    isPurchased = table.Column<bool>(nullable: true),
                    rating = table.Column<short>(nullable: true),
                    firstname = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    lastname = table.Column<string>(unicode: false, maxLength: 30, nullable: true),
                    paypalEmail = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    amount = table.Column<string>(nullable: true),
                    intent = table.Column<string>(nullable: true),
                    paymentMethod = table.Column<string>(nullable: true),
                    paymentState = table.Column<string>(nullable: true),
                    paymentId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersBooks", x => x.userBookID);
                    table.ForeignKey(
                        name: "FK__UsersBook__bookI__75A278F5",
                        column: x => x.bookID,
                        principalTable: "Books",
                        principalColumn: "bookID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__UsersBook__userI__74AE54BC",
                        column: x => x.userID,
                        principalTable: "Users",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_authorID",
                table: "Books",
                column: "authorID");

            migrationBuilder.CreateIndex(
                name: "IX_UsersBooks_bookID",
                table: "UsersBooks",
                column: "bookID");

            migrationBuilder.CreateIndex(
                name: "IX_UsersBooks_userID",
                table: "UsersBooks",
                column: "userID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersBooks");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
