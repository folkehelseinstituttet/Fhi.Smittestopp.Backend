using DIGNDB.App.SmitteStop.DAL.Seeders;
using DIGNDB.App.SmitteStop.Domain.Db;
using Microsoft.EntityFrameworkCore;

namespace DIGNDB.App.SmitteStop.DAL.Context
{
    public partial class DigNDB_SmittestopContext : DbContext
    {
        private readonly string _connectionString;

        public DigNDB_SmittestopContext()
        {
        }

        public DigNDB_SmittestopContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DigNDB_SmittestopContext(DbContextOptions<DigNDB_SmittestopContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TemporaryExposureKey> TemporaryExposureKey { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Setting> Setting { get; set; }
        public virtual DbSet<Translation> Translation { get; set; }
        public virtual DbSet<TemporaryExposureKeyCountry> TemporaryExposureKeyCountry { get; set; }
        public virtual DbSet<JwtToken> JwtToken { get; set; }

        public virtual DbSet<SSIStatistics> SSIStatistics { get; set; }
        public virtual DbSet<ApplicationStatistics> ApplicationStatistics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Setting>(entity =>
            {
                entity.Property(e => e.Key).IsRequired();
            });
            modelBuilder.Entity<TemporaryExposureKey>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.KeyData)
                    .IsRequired()
                    .HasMaxLength(255);
                entity.Property(e => e.ReportType);

            });
            modelBuilder.Entity<TemporaryExposureKey>()
                .HasOne(t => t.Origin)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TemporaryExposureKey>()
                .HasIndex(x => x.KeyData)
                .IsUnique();

            modelBuilder.Entity<TemporaryExposureKeyCountry>()
                .HasKey(tc => new { tc.TemporaryExposureKeyId, tc.CountryId });

            modelBuilder.Entity<TemporaryExposureKeyCountry>()
                .HasOne(tc => tc.Country)
                .WithMany(c => c.TemporaryExposureKeyCountries)
                .HasForeignKey(tc => tc.CountryId);

            modelBuilder.Entity<TemporaryExposureKeyCountry>()
                .HasOne(tc => tc.TemporaryExposureKey)
                .WithMany(c => c.VisitedCountries)
                .HasForeignKey(tc => tc.TemporaryExposureKeyId);

            modelBuilder.Entity<Country>()
                .HasData(new CountrySeeder().GetSeedData());

            modelBuilder.Entity<Translation>()
                .HasData(new TranslationSeeder().GetSeedData());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
