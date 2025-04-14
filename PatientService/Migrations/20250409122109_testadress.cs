using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedilaboSolutionsBack1.Migrations
{
    /// <inheritdoc />
    public partial class testadress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adresse",
                table: "Patients");

            migrationBuilder.AddColumn<int>(
                name: "AdresseId",
                table: "Patients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Adresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libele = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adresses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_AdresseId",
                table: "Patients",
                column: "AdresseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Adresses_AdresseId",
                table: "Patients",
                column: "AdresseId",
                principalTable: "Adresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Adresses_AdresseId",
                table: "Patients");

            migrationBuilder.DropTable(
                name: "Adresses");

            migrationBuilder.DropIndex(
                name: "IX_Patients_AdresseId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "AdresseId",
                table: "Patients");

            migrationBuilder.AddColumn<string>(
                name: "Adresse",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
