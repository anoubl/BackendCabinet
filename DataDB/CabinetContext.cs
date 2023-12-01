using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BackendCabinet.DataDB;

public partial class CabinetContext : DbContext
{
    public CabinetContext()
    {
    }

    public CabinetContext(DbContextOptions<CabinetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=ahmed;Initial Catalog=Cabinet;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3214EC07E7A22126");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__A9D1053495140FD7").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Nom).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Prenom).HasMaxLength(255);
            entity.Property(e => e.Rôle)
                .HasDefaultValueSql("((0))")
                .HasColumnName("rôle");
            entity.Property(e => e.Telephone).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
