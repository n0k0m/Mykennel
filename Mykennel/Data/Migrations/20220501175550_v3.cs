using Microsoft.EntityFrameworkCore.Migrations;

namespace Mykennel.Data.Migrations
{
    public partial class v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Litters_Dogs_FatherId",
                table: "Litters");

            migrationBuilder.DropForeignKey(
                name: "FK_Litters_Dogs_MotherId",
                table: "Litters");

            migrationBuilder.AlterColumn<int>(
                name: "MotherId",
                table: "Litters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FatherId",
                table: "Litters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Litters_Dogs_FatherId",
                table: "Litters",
                column: "FatherId",
                principalTable: "Dogs",
                principalColumn: "DogId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Litters_Dogs_MotherId",
                table: "Litters",
                column: "MotherId",
                principalTable: "Dogs",
                principalColumn: "DogId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Litters_Dogs_FatherId",
                table: "Litters");

            migrationBuilder.DropForeignKey(
                name: "FK_Litters_Dogs_MotherId",
                table: "Litters");

            migrationBuilder.AlterColumn<int>(
                name: "MotherId",
                table: "Litters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "FatherId",
                table: "Litters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Litters_Dogs_FatherId",
                table: "Litters",
                column: "FatherId",
                principalTable: "Dogs",
                principalColumn: "DogId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Litters_Dogs_MotherId",
                table: "Litters",
                column: "MotherId",
                principalTable: "Dogs",
                principalColumn: "DogId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
