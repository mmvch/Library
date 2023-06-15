using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Library.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("2949d456-901a-4f7b-a7a3-7fd1e8c2f65d"), "admin" },
                    { new Guid("e5e7f84a-685f-429b-ae38-9b44e067d76a"), "author" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "PasswordHash", "PasswordSalt" },
                values: new object[] { new Guid("630313d4-d313-4fa8-bb0e-bfce2dcb26b9"), "admin", new byte[] { 61, 142, 96, 149, 45, 243, 53, 28, 245, 194, 163, 124, 21, 204, 62, 183, 57, 193, 123, 209, 68, 5, 240, 149, 55, 127, 10, 197, 64, 49, 218, 79, 86, 14, 145, 231, 198, 243, 132, 173, 199, 152, 162, 92, 163, 157, 108, 185, 204, 207, 186, 231, 216, 74, 247, 3, 167, 106, 126, 68, 96, 128, 111, 12 }, new byte[] { 130, 17, 255, 118, 250, 76, 61, 237, 130, 77, 45, 221, 254, 85, 95, 154, 231, 66, 2, 202, 102, 152, 71, 52, 68, 153, 248, 209, 174, 162, 62, 71, 179, 248, 39, 88, 116, 11, 55, 195, 5, 21, 184, 190, 165, 224, 41, 84, 37, 183, 161, 184, 218, 194, 13, 161, 215, 132, 255, 44, 152, 185, 169, 101, 245, 198, 190, 151, 51, 78, 223, 163, 224, 17, 19, 248, 237, 156, 89, 135, 210, 170, 225, 58, 106, 227, 93, 79, 119, 33, 109, 39, 137, 143, 177, 192, 193, 160, 223, 182, 227, 122, 29, 240, 132, 112, 70, 168, 164, 71, 113, 78, 239, 251, 153, 99, 223, 1, 242, 27, 68, 78, 142, 79, 79, 97, 184, 57 } });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("2949d456-901a-4f7b-a7a3-7fd1e8c2f65d"), new Guid("630313d4-d313-4fa8-bb0e-bfce2dcb26b9") });

            migrationBuilder.CreateIndex(
                name: "IX_Books_UserId",
                table: "Books",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login",
                table: "Users",
                column: "Login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
