using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class Addedremovedhubmessageandaddedroom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HubMessagesDb_AspNetUsers_SenderId",
                table: "HubMessagesDb");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HubMessagesDb",
                table: "HubMessagesDb");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "HubMessagesDb");

            migrationBuilder.RenameTable(
                name: "HubMessagesDb",
                newName: "ChatMessagesDb");

            migrationBuilder.RenameIndex(
                name: "IX_HubMessagesDb_SenderId",
                table: "ChatMessagesDb",
                newName: "IX_ChatMessagesDb_SenderId");

            migrationBuilder.AddColumn<string>(
                name: "RoomName",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatMessagesDb",
                table: "ChatMessagesDb",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "RoomsDb",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    MaxAmountOfUser = table.Column<long>(type: "bigint", nullable: false),
                    OwnerId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsDb", x => x.Name);
                    table.ForeignKey(
                        name: "FK_RoomsDb_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoomName",
                table: "AspNetUsers",
                column: "RoomName");

            migrationBuilder.CreateIndex(
                name: "IX_RoomsDb_OwnerId",
                table: "RoomsDb",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_RoomsDb_RoomName",
                table: "AspNetUsers",
                column: "RoomName",
                principalTable: "RoomsDb",
                principalColumn: "Name");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessagesDb_AspNetUsers_SenderId",
                table: "ChatMessagesDb",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_RoomsDb_RoomName",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessagesDb_AspNetUsers_SenderId",
                table: "ChatMessagesDb");

            migrationBuilder.DropTable(
                name: "RoomsDb");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoomName",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatMessagesDb",
                table: "ChatMessagesDb");

            migrationBuilder.DropColumn(
                name: "RoomName",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "ChatMessagesDb",
                newName: "HubMessagesDb");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessagesDb_SenderId",
                table: "HubMessagesDb",
                newName: "IX_HubMessagesDb_SenderId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "HubMessagesDb",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HubMessagesDb",
                table: "HubMessagesDb",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HubMessagesDb_AspNetUsers_SenderId",
                table: "HubMessagesDb",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
