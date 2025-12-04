using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebGestionAppro.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrixUnitaire = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockActuel = table.Column<int>(type: "int", nullable: false),
                    Unite = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateCreation = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Fournisseurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Adresse = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telephone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateCreation = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fournisseurs", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Approvisionnements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Reference = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateApprovisionnement = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FournisseurId = table.Column<int>(type: "int", nullable: false),
                    MontantTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    Observations = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateCreation = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Approvisionnements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Approvisionnements_Fournisseurs_FournisseurId",
                        column: x => x.FournisseurId,
                        principalTable: "Fournisseurs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ApprovisionnementArticles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ApprovisionnementId = table.Column<int>(type: "int", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    PrixUnitaire = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Montant = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovisionnementArticles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovisionnementArticles_Approvisionnements_Approvisionneme~",
                        column: x => x.ApprovisionnementId,
                        principalTable: "Approvisionnements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApprovisionnementArticles_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "DateCreation", "Description", "Nom", "PrixUnitaire", "StockActuel", "Unite" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tissu 100% coton blanc de qualité supérieure", "Tissu Coton Blanc", 2500m, 150, "mètre" },
                    { 2, new DateTime(2023, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Tissu wax traditionnel avec motifs", "Tissu Wax Multicolore", 4500m, 80, "mètre" },
                    { 3, new DateTime(2023, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bobine de fil noir 500m", "Fil à coudre Noir", 500m, 200, "bobine" },
                    { 4, new DateTime(2023, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lot de 100 boutons plastique blancs", "Boutons Plastique", 1000m, 50, "lot" },
                    { 5, new DateTime(2023, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fermeture éclair métal 20cm", "Fermeture Éclair 20cm", 300m, 120, "pièce" }
                });

            migrationBuilder.InsertData(
                table: "Fournisseurs",
                columns: new[] { "Id", "Adresse", "DateCreation", "Email", "Nom", "Telephone" },
                values: new object[,]
                {
                    { 1, "Zone Industrielle, Dakar", new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "contact@textilesdakar.sn", "Textiles Dakar SARL", "+221 33 123 4567" },
                    { 2, "Rue 10, Dakar", new DateTime(2023, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "info@merceriecentrale.sn", "Mercerie Centrale", "+221 33 234 5678" },
                    { 3, "Avenue Lamine Gueye, Dakar", new DateTime(2023, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "contact@tissuspremium.sn", "Tissus Premium", "+221 33 345 6789" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovisionnementArticles_ApprovisionnementId",
                table: "ApprovisionnementArticles",
                column: "ApprovisionnementId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovisionnementArticles_ArticleId",
                table: "ApprovisionnementArticles",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_Approvisionnements_FournisseurId",
                table: "Approvisionnements",
                column: "FournisseurId");

            migrationBuilder.CreateIndex(
                name: "IX_Approvisionnements_Reference",
                table: "Approvisionnements",
                column: "Reference",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovisionnementArticles");

            migrationBuilder.DropTable(
                name: "Approvisionnements");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Fournisseurs");
        }
    }
}
