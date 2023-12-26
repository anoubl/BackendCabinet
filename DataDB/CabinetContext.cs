﻿using System;
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

    public virtual DbSet<RendezVou> RendezVous { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userprofille> Userprofilles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=SQL8006.site4now.net;Initial Catalog=db_aa26b5_cabinetmedical;User Id=db_aa26b5_cabinetmedical_admin;Password=AHMEd26@;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RendezVou>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RendezVo__3213E83F30F83BB4");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Daterendezvous)
                .HasColumnType("date")
                .HasColumnName("daterendezvous");
            entity.Property(e => e.Description)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("description");
     
            entity.Property(e => e.Patientemail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("patientemail");
            entity.Property(e => e.Plage)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("plage");

        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__users__3214EC0752702939");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "UQ__users__AB6E6164D01FFCC2").IsUnique();

            entity.Property(e => e.Adresse)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DateNaissance).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Nom)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nom");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Prenom)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("prenom");
            entity.Property(e => e.Rôle)
                .HasDefaultValueSql("((0))")
                .HasColumnName("rôle");
            entity.Property(e => e.Telephone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("telephone");
        });

        modelBuilder.Entity<Userprofille>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__userprof__3213E83FAA08D9F4");

            entity.ToTable("userprofille");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Imag)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("imag");
            entity.Property(e => e.Userid).HasColumnName("userid");

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
