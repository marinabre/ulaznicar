namespace Ulaznicar.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BazaUlaznice : DbContext
    {
        public BazaUlaznice()
            : base("name=BazaUlaznice1")
        {
        }

        public virtual DbSet<Burza> Burza { get; set; }
        public virtual DbSet<Dogadjaj> Dogadjaj { get; set; }
        public virtual DbSet<Grad> Grad { get; set; }
        public virtual DbSet<Izvodjac> Izvodjac { get; set; }
        public virtual DbSet<Karta> Karta { get; set; }
        public virtual DbSet<Koncert> Koncert { get; set; }
        public virtual DbSet<Korisnik> Korisnik { get; set; }
        public virtual DbSet<KupljeneKarte> KupljeneKarte { get; set; }
        public virtual DbSet<Lokacija> Lokacija { get; set; }
        public virtual DbSet<VrstaKarte> VrstaKarte { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dogadjaj>()
                .HasMany(e => e.Karta)
                .WithRequired(e => e.Dogadjaj)
                .HasForeignKey(e => e.IdDogadjaj)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Grad>()
                .HasMany(e => e.Lokacija)
                .WithRequired(e => e.Grad)
                .HasForeignKey(e => e.IdGrad)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Karta>()
                .HasMany(e => e.KupljeneKarte)
                .WithRequired(e => e.Karta)
                .HasForeignKey(e => e.IdKarta)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Korisnik>()
                .Property(e => e.OIB)
                .IsFixedLength();

            modelBuilder.Entity<Korisnik>()
                .HasMany(e => e.Burza)
                .WithOptional(e => e.Korisnik)
                .HasForeignKey(e => e.IdKupac);

            modelBuilder.Entity<Korisnik>()
                .HasMany(e => e.KupljeneKarte)
                .WithRequired(e => e.Korisnik)
                .HasForeignKey(e => e.IdKorisnik)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Lokacija>()
                .HasMany(e => e.Dogadjaj)
                .WithRequired(e => e.Lokacija)
                .HasForeignKey(e => e.IdLokacija)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<VrstaKarte>()
                .HasMany(e => e.Karta)
                .WithRequired(e => e.VrstaKarte)
                .HasForeignKey(e => e.IdVrstakarte)
                .WillCascadeOnDelete(false);
        }
    }
}
