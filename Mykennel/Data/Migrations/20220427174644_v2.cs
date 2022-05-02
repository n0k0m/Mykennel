using Microsoft.EntityFrameworkCore.Migrations;

namespace Mykennel.Data.Migrations
{
    public partial class v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dogs_Breeds_BreedId",
                table: "Dogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Dogs_Kennels_KennelId",
                table: "Dogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Kennels_Countries_CountryId",
                table: "Kennels");

            migrationBuilder.DropForeignKey(
                name: "FK_Litters_Kennels_KennelId",
                table: "Litters");

            migrationBuilder.DropForeignKey(
                name: "FK_Puppies_Litters_LitterId",
                table: "Puppies");

            migrationBuilder.AlterColumn<int>(
                name: "LitterId",
                table: "Puppies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "KennelId",
                table: "Litters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Kennels",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "KennelId",
                table: "Dogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BreedId",
                table: "Dogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(128)",
                oldMaxLength: 128);

            migrationBuilder.AddForeignKey(
                name: "FK_Dogs_Breeds_BreedId",
                table: "Dogs",
                column: "BreedId",
                principalTable: "Breeds",
                principalColumn: "BreedId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dogs_Kennels_KennelId",
                table: "Dogs",
                column: "KennelId",
                principalTable: "Kennels",
                principalColumn: "KennelId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Kennels_Countries_CountryId",
                table: "Kennels",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Litters_Kennels_KennelId",
                table: "Litters",
                column: "KennelId",
                principalTable: "Kennels",
                principalColumn: "KennelId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Puppies_Litters_LitterId",
                table: "Puppies",
                column: "LitterId",
                principalTable: "Litters",
                principalColumn: "LitterId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dogs_Breeds_BreedId",
                table: "Dogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Dogs_Kennels_KennelId",
                table: "Dogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Kennels_Countries_CountryId",
                table: "Kennels");

            migrationBuilder.DropForeignKey(
                name: "FK_Litters_Kennels_KennelId",
                table: "Litters");

            migrationBuilder.DropForeignKey(
                name: "FK_Puppies_Litters_LitterId",
                table: "Puppies");

            migrationBuilder.AlterColumn<int>(
                name: "LitterId",
                table: "Puppies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "KennelId",
                table: "Litters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CountryId",
                table: "Kennels",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "KennelId",
                table: "Dogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BreedId",
                table: "Dogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserTokens",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderKey",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "LoginProvider",
                table: "AspNetUserLogins",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Dogs_Breeds_BreedId",
                table: "Dogs",
                column: "BreedId",
                principalTable: "Breeds",
                principalColumn: "BreedId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Dogs_Kennels_KennelId",
                table: "Dogs",
                column: "KennelId",
                principalTable: "Kennels",
                principalColumn: "KennelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Kennels_Countries_CountryId",
                table: "Kennels",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Litters_Kennels_KennelId",
                table: "Litters",
                column: "KennelId",
                principalTable: "Kennels",
                principalColumn: "KennelId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Puppies_Litters_LitterId",
                table: "Puppies",
                column: "LitterId",
                principalTable: "Litters",
                principalColumn: "LitterId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
