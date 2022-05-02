using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mykennel.Data.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Breeds",
                columns: table => new
                {
                    BreedId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OriginalName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breeds", x => x.BreedId);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Kennels",
                columns: table => new
                {
                    KennelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KennelName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    URLName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kennels", x => x.KennelId);
                    table.ForeignKey(
                        name: "FK_Kennels_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Kennels_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Dogs",
                columns: table => new
                {
                    DogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Born = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TitlesGenetics = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DogImage = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BreedId = table.Column<int>(type: "int", nullable: false),
                    KennelId = table.Column<int>(type: "int", nullable: false),
                    FatherId = table.Column<int>(type: "int", nullable: true),
                    MotherId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dogs", x => x.DogId);
                    table.ForeignKey(
                        name: "FK_Dogs_Breeds_BreedId",
                        column: x => x.BreedId,
                        principalTable: "Breeds",
                        principalColumn: "BreedId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dogs_Dogs_FatherId",
                        column: x => x.FatherId,
                        principalTable: "Dogs",
                        principalColumn: "DogId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dogs_Dogs_MotherId",
                        column: x => x.MotherId,
                        principalTable: "Dogs",
                        principalColumn: "DogId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dogs_Kennels_KennelId",
                        column: x => x.KennelId,
                        principalTable: "Kennels",
                        principalColumn: "KennelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Litters",
                columns: table => new
                {
                    LitterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KennelId = table.Column<int>(type: "int", nullable: false),
                    FatherId = table.Column<int>(type: "int", nullable: true),
                    MotherId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Litters", x => x.LitterId);
                    table.ForeignKey(
                        name: "FK_Litters_Dogs_FatherId",
                        column: x => x.FatherId,
                        principalTable: "Dogs",
                        principalColumn: "DogId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Litters_Dogs_MotherId",
                        column: x => x.MotherId,
                        principalTable: "Dogs",
                        principalColumn: "DogId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Litters_Kennels_KennelId",
                        column: x => x.KennelId,
                        principalTable: "Kennels",
                        principalColumn: "KennelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Puppies",
                columns: table => new
                {
                    PuppyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Sex = table.Column<int>(type: "int", nullable: false),
                    Bookable = table.Column<bool>(type: "bit", nullable: false),
                    Aim = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DogId = table.Column<int>(type: "int", nullable: true),
                    LitterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puppies", x => x.PuppyId);
                    table.ForeignKey(
                        name: "FK_Puppies_Dogs_DogId",
                        column: x => x.DogId,
                        principalTable: "Dogs",
                        principalColumn: "DogId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Puppies_Litters_LitterId",
                        column: x => x.LitterId,
                        principalTable: "Litters",
                        principalColumn: "LitterId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dogs_BreedId",
                table: "Dogs",
                column: "BreedId");

            migrationBuilder.CreateIndex(
                name: "IX_Dogs_FatherId",
                table: "Dogs",
                column: "FatherId");

            migrationBuilder.CreateIndex(
                name: "IX_Dogs_KennelId",
                table: "Dogs",
                column: "KennelId");

            migrationBuilder.CreateIndex(
                name: "IX_Dogs_MotherId",
                table: "Dogs",
                column: "MotherId");

            migrationBuilder.CreateIndex(
                name: "IX_Kennels_ApplicationUserId",
                table: "Kennels",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Kennels_CountryId",
                table: "Kennels",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Kennels_URLName",
                table: "Kennels",
                column: "URLName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Litters_FatherId",
                table: "Litters",
                column: "FatherId");

            migrationBuilder.CreateIndex(
                name: "IX_Litters_KennelId",
                table: "Litters",
                column: "KennelId");

            migrationBuilder.CreateIndex(
                name: "IX_Litters_MotherId",
                table: "Litters",
                column: "MotherId");

            migrationBuilder.CreateIndex(
                name: "IX_Puppies_DogId",
                table: "Puppies",
                column: "DogId",
                unique: true,
                filter: "[DogId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Puppies_LitterId",
                table: "Puppies",
                column: "LitterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Puppies");

            migrationBuilder.DropTable(
                name: "Litters");

            migrationBuilder.DropTable(
                name: "Dogs");

            migrationBuilder.DropTable(
                name: "Breeds");

            migrationBuilder.DropTable(
                name: "Kennels");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
