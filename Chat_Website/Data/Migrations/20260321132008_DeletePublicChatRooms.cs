using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat_Website.Migrations
{
    /// <inheritdoc />
    public partial class DeletePublicChatRooms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Delete all public chat rooms (those without "_" in title - private chats have user IDs separated by "_")
            // First delete associated chat messages
            migrationBuilder.Sql(@"
                DELETE FROM [messages] 
                WHERE ChatRoomId IN (
                    SELECT Id FROM [chatrooms] 
                    WHERE [Title] NOT LIKE '%_%'
                )
            ");
            
            // Then delete associated room members if the table exists
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[chatRoomMembers]') AND type in (N'U'))
                BEGIN
                    DELETE FROM [chatRoomMembers] 
                    WHERE ChatRoomId IN (
                        SELECT Id FROM [chatrooms] 
                        WHERE [Title] NOT LIKE '%_%'
                    )
                END
            ");
            
            // Finally delete the public chat rooms themselves
            migrationBuilder.Sql(@"
                DELETE FROM [chatrooms] 
                WHERE [Title] NOT LIKE '%_%'
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // This migration deletes data, so rollback is not possible without a database backup
        }
    }
}
