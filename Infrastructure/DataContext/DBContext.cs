
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using File = DataAccess.Entities.File;

namespace DataAccess.DataContext
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; } = null!;
        public virtual DbSet<Clientdetail> Clientdetails { get; set; } = null!;
        public virtual DbSet<File> Files { get; set; } = null!;
        public virtual DbSet<State> States { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("client");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Clientdetail>(entity =>
            {
                entity.ToTable("clientdetails");

                entity.HasIndex(e => e.Email, "clientdetails_email_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClientId).HasColumnName("client_id");

                entity.Property(e => e.Dob).HasColumnName("dob");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.ExpEnd).HasColumnName("exp_end");

                entity.Property(e => e.ExpStart).HasColumnName("exp_start");

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .HasColumnName("gender");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.PayType)
                    .HasMaxLength(50)
                    .HasColumnName("pay_type");

                entity.Property(e => e.PayValue)
                    .HasPrecision(10, 2)
                    .HasColumnName("pay_value");

                entity.Property(e => e.StateId)
                    .HasMaxLength(255)
                    .HasColumnName("state_id");

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.Clientdetails)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("clientdetails_client_id_fkey");
            });

            modelBuilder.Entity<File>(entity =>
            {
                entity.ToTable("files");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClientDetailsId).HasColumnName("client_details_id");

                entity.Property(e => e.Filedata).HasColumnName("filedata");

                entity.Property(e => e.Filename)
                    .HasMaxLength(255)
                    .HasColumnName("filename");

                entity.HasOne(d => d.ClientDetails)
                    .WithMany(p => p.Files)
                    .HasForeignKey(d => d.ClientDetailsId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("files_client_details_id_fkey");
            });

            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("states");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
