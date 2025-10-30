using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusShare.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articulos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Disponible = table.Column<bool>(type: "bit", nullable: false),
                    TipoArticulo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articulos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prestamos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FecInicio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FecFin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoPrestamo = table.Column<int>(type: "int", nullable: false),
                    ArticuloId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AlumnoId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestamos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prestamos_Articulos_ArticuloId",
                        column: x => x.ArticuloId,
                        principalTable: "Articulos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prestamos_Users_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reservas",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FecInicio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FecFin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstadoReserva = table.Column<int>(type: "int", nullable: false),
                    ArticuloId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AlumnoId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AlumnoId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservas_Articulos_ArticuloId",
                        column: x => x.ArticuloId,
                        principalTable: "Articulos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reservas_Users_AlumnoId",
                        column: x => x.AlumnoId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservas_Users_AlumnoId1",
                        column: x => x.AlumnoId1,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_AlumnoId",
                table: "Prestamos",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_ArticuloId",
                table: "Prestamos",
                column: "ArticuloId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_AlumnoId",
                table: "Reservas",
                column: "AlumnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_AlumnoId1",
                table: "Reservas",
                column: "AlumnoId1");

            migrationBuilder.CreateIndex(
                name: "IX_Reservas_ArticuloId",
                table: "Reservas",
                column: "ArticuloId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prestamos");

            migrationBuilder.DropTable(
                name: "Reservas");

            migrationBuilder.DropTable(
                name: "Articulos");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
