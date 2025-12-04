using Microsoft.EntityFrameworkCore;
using WebGestionAppro.Models;

namespace WebGestionAppro.Data
{
    public class GesApproDbContext : DbContext
    {
        public GesApproDbContext(DbContextOptions<GesApproDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("Server=localhost;Port=3306;Database=GesApproDB;User=root;Password=;", 
                    new MySqlServerVersion(new Version(8, 0, 0)));
            }
        }

        public DbSet<Fournisseur> Fournisseurs { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Approvisionnement> Approvisionnements { get; set; }
        public DbSet<ApprovisionnementArticle> ApprovisionnementArticles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration de Fournisseur
            modelBuilder.Entity<Fournisseur>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nom).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Adresse).HasMaxLength(500);
                entity.Property(e => e.Telephone).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(100);
            });

            // Configuration de Article
            modelBuilder.Entity<Article>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nom).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.PrixUnitaire).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Unite).HasMaxLength(50);
            });

            // Configuration de Approvisionnement
            modelBuilder.Entity<Approvisionnement>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Reference).IsRequired().HasMaxLength(50);
                entity.Property(e => e.MontantTotal).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Observations).HasMaxLength(2000);

                // Relation avec Fournisseur
                entity.HasOne(e => e.Fournisseur)
                    .WithMany(f => f.Approvisionnements)
                    .HasForeignKey(e => e.FournisseurId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Index sur la référence
                entity.HasIndex(e => e.Reference).IsUnique();
            });

            // Configuration de ApprovisionnementArticle
            modelBuilder.Entity<ApprovisionnementArticle>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PrixUnitaire).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Montant).HasColumnType("decimal(18,2)");

                // Relation avec Approvisionnement
                entity.HasOne(e => e.Approvisionnement)
                    .WithMany(a => a.ApprovisionnementArticles)
                    .HasForeignKey(e => e.ApprovisionnementId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relation avec Article
                entity.HasOne(e => e.Article)
                    .WithMany(a => a.ApprovisionnementArticles)
                    .HasForeignKey(e => e.ArticleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Données de test (seed)
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Fournisseurs
            modelBuilder.Entity<Fournisseur>().HasData(
                new Fournisseur { Id = 1, Nom = "Textiles Dakar SARL", Adresse = "Zone Industrielle, Dakar", Telephone = "+221 33 123 4567", Email = "contact@textilesdakar.sn", DateCreation = new DateTime(2023, 1, 15) },
                new Fournisseur { Id = 2, Nom = "Mercerie Centrale", Adresse = "Rue 10, Dakar", Telephone = "+221 33 234 5678", Email = "info@merceriecentrale.sn", DateCreation = new DateTime(2023, 2, 10) },
                new Fournisseur { Id = 3, Nom = "Tissus Premium", Adresse = "Avenue Lamine Gueye, Dakar", Telephone = "+221 33 345 6789", Email = "contact@tissuspremium.sn", DateCreation = new DateTime(2023, 3, 5) }
            );

            // Articles
            modelBuilder.Entity<Article>().HasData(
                new Article { Id = 1, Nom = "Tissu Coton Blanc", Description = "Tissu 100% coton blanc de qualité supérieure", PrixUnitaire = 2500, StockActuel = 150, Unite = "mètre", DateCreation = new DateTime(2023, 1, 20) },
                new Article { Id = 2, Nom = "Tissu Wax Multicolore", Description = "Tissu wax traditionnel avec motifs", PrixUnitaire = 4500, StockActuel = 80, Unite = "mètre", DateCreation = new DateTime(2023, 1, 22) },
                new Article { Id = 3, Nom = "Fil à coudre Noir", Description = "Bobine de fil noir 500m", PrixUnitaire = 500, StockActuel = 200, Unite = "bobine", DateCreation = new DateTime(2023, 1, 25) },
                new Article { Id = 4, Nom = "Boutons Plastique", Description = "Lot de 100 boutons plastique blancs", PrixUnitaire = 1000, StockActuel = 50, Unite = "lot", DateCreation = new DateTime(2023, 2, 1) },
                new Article { Id = 5, Nom = "Fermeture Éclair 20cm", Description = "Fermeture éclair métal 20cm", PrixUnitaire = 300, StockActuel = 120, Unite = "pièce", DateCreation = new DateTime(2023, 2, 5) }
            );
        }
    }
}
