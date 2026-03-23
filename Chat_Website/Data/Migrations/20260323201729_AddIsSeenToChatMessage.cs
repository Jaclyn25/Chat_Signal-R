using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat_Website.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsSeenToChatMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSeen",
                table: "messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSeen",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "AspNetUsers");
        }
    }
}
