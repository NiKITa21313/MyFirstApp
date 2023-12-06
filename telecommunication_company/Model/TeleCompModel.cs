using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace telecommunication_company.Model
{
    public partial class TeleCompModel : DbContext
    {
        private static TeleCompModel _instance;

        public TeleCompModel()
            : base("name=TeleCompModel")
        {
        }

        public static TeleCompModel GetContext()
        {
            if (_instance == null)
            {
                _instance = new TeleCompModel();
            }
            return _instance;
        }

        public virtual DbSet<Doljnost> Doljnost { get; set; }
        public virtual DbSet<Klienty> Klienty { get; set; }
        public virtual DbSet<Oborudovanie> Oborudovanie { get; set; }
        public virtual DbSet<Pokupka_oborudovaniya> Pokupka_oborudovaniya { get; set; }
        public virtual DbSet<Sotrudniki> Sotrudniki { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Uslugi> Uslugi { get; set; }
        public virtual DbSet<Vipolnenie_zayavki> Vipolnenie_zayavki { get; set; }
        public virtual DbSet<Zayavka> Zayavka { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doljnost>()
                .HasMany(e => e.Sotrudniki)
                .WithRequired(e => e.Doljnost1)
                .HasForeignKey(e => e.Doljnost)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Klienty>()
                .Property(e => e.Nomer_telefona)
                .IsFixedLength();

            modelBuilder.Entity<Klienty>()
                .HasMany(e => e.Pokupka_oborudovaniya)
                .WithRequired(e => e.Klienty)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Klienty>()
                .HasMany(e => e.Zayavka)
                .WithRequired(e => e.Klienty)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Oborudovanie>()
                .Property(e => e.Zena)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Oborudovanie>()
                .HasMany(e => e.Zayavka)
                .WithMany(e => e.Oborudovanie)
                .Map(m => m.ToTable("Spisok_oborudovaniya").MapLeftKey("ID_oborudovaniya").MapRightKey("ID_zayavki"));

            modelBuilder.Entity<Oborudovanie>()
                .HasMany(e => e.Pokupka_oborudovaniya)
                .WithMany(e => e.Oborudovanie)
                .Map(m => m.ToTable("Spisok_pokupki_oborodovaniya").MapLeftKey("ID_oborudovaniya").MapRightKey("ID_pokupki"));

            modelBuilder.Entity<Pokupka_oborudovaniya>()
                .Property(e => e.Summa)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Sotrudniki>()
                .Property(e => e.Nomer_telefona)
                .IsFixedLength();

            modelBuilder.Entity<Sotrudniki>()
                .HasMany(e => e.Vipolnenie_zayavki)
                .WithRequired(e => e.Sotrudniki)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Uslugi>()
                .Property(e => e.Stoimost)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Uslugi>()
                .HasMany(e => e.Zayavka)
                .WithRequired(e => e.Uslugi)
                .HasForeignKey(e => e.Usluga)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Vipolnenie_zayavki>()
                .Property(e => e.Stoimost)
                .HasPrecision(19, 4);
        }
    }
}
