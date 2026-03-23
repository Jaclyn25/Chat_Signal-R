using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat_Website.Migrations
{
    /// <inheritdoc />
    public partial class AddChatRoomMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatRoomUserApplication");

            migrationBuilder.AddColumn<string>(
                name: "CreatedByUserId",
                table: "chatrooms",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                table: "chatrooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "chatRoomMembers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChatRoomId = table.Column<int>(type: "int", nullable: false),
                    UserApplicationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCreator = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chatRoomMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_chatRoomMembers_AspNetUsers_UserApplicationId",
                        column: x => x.UserApplicationId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_chatRoomMembers_chatrooms_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "chatrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_chatrooms_CreatedByUserId",
                table: "chatrooms",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_chatRoomMembers_ChatRoomId_UserApplicationId",
                table: "chatRoomMembers",
                columns: new[] { "ChatRoomId", "UserApplicationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_chatRoomMembers_UserApplicationId",
                table: "chatRoomMembers",
                column: "UserApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_chatrooms_AspNetUsers_CreatedByUserId",
                table: "chatrooms",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chatrooms_AspNetUsers_CreatedByUserId",
                table: "chatrooms");

            migrationBuilder.DropTable(
                name: "chatRoomMembers");

            migrationBuilder.DropIndex(
                name: "IX_chatrooms_CreatedByUserId",
                table: "chatrooms");

            migrationBuilder.DropColumn(
                name: "CreatedByUserId",
                table: "chatrooms");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                table: "chatrooms");

            migrationBuilder.CreateTable(
                name: "ChatRoomUserApplication",
                columns: table => new
                {
                    ChatRoomsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoomUserApplication", x => new { x.ChatRoomsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ChatRoomUserApplication_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatRoomUserApplication_chatrooms_ChatRoomsId",
                        column: x => x.ChatRoomsId,
                        principalTable: "chatrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomUserApplication_UsersId",
                table: "ChatRoomUserApplication",
                column: "UsersId");
        }
    }
}
