using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Uid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLoginId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    IP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CilentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserUid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logins_Users_UserUid",
                        column: x => x.UserUid,
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NoteProfile",
                columns: table => new
                {
                    NoteSharesId = table.Column<int>(type: "int", nullable: false),
                    ShareRecipientsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteProfile", x => new { x.NoteSharesId, x.ShareRecipientsId });
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<int>(type: "int", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoteId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BioId = table.Column<int>(type: "int", nullable: true),
                    ProfilePictureId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Profiles_Photos_ProfilePictureId",
                        column: x => x.ProfilePictureId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Profiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<int>(type: "int", nullable: true),
                    Encrypted = table.Column<bool>(type: "bit", nullable: false),
                    SharedPublically = table.Column<bool>(type: "bit", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Profiles_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhotoProfile",
                columns: table => new
                {
                    PhotoSharesId = table.Column<int>(type: "int", nullable: false),
                    ShareRecipientsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoProfile", x => new { x.PhotoSharesId, x.ShareRecipientsId });
                    table.ForeignKey(
                        name: "FK_PhotoProfile_Photos_PhotoSharesId",
                        column: x => x.PhotoSharesId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhotoProfile_Profiles_ShareRecipientsId",
                        column: x => x.ShareRecipientsId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logins_UserUid",
                table: "Logins",
                column: "UserUid");

            migrationBuilder.CreateIndex(
                name: "IX_NoteProfile_ShareRecipientsId",
                table: "NoteProfile",
                column: "ShareRecipientsId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_OwnerId",
                table: "Notes",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoProfile_ShareRecipientsId",
                table: "PhotoProfile",
                column: "ShareRecipientsId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_NoteId",
                table: "Photos",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_OwnerId",
                table: "Photos",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_BioId",
                table: "Profiles",
                column: "BioId",
                unique: true,
                filter: "[BioId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_ProfilePictureId",
                table: "Profiles",
                column: "ProfilePictureId",
                unique: true,
                filter: "[ProfilePictureId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                table: "Profiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_LastLoginId",
                table: "Users",
                column: "LastLoginId",
                unique: true,
                filter: "[LastLoginId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Logins_LastLoginId",
                table: "Users",
                column: "LastLoginId",
                principalTable: "Logins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteProfile_Notes_NoteSharesId",
                table: "NoteProfile",
                column: "NoteSharesId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteProfile_Profiles_ShareRecipientsId",
                table: "NoteProfile",
                column: "ShareRecipientsId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Notes_NoteId",
                table: "Photos",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Profiles_OwnerId",
                table: "Photos",
                column: "OwnerId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Notes_BioId",
                table: "Profiles",
                column: "BioId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logins_Users_UserUid",
                table: "Logins");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Users_UserId",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Notes_NoteId",
                table: "Photos");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Notes_BioId",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Profiles_OwnerId",
                table: "Photos");

            migrationBuilder.DropTable(
                name: "NoteProfile");

            migrationBuilder.DropTable(
                name: "PhotoProfile");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Photos");
        }
    }
}
