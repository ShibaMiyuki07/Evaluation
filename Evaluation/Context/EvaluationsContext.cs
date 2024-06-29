﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using EvaluationClasse;

namespace Evaluation.Context;

public partial class EvaluationsContext : DbContext
{
    public EvaluationsContext()
    {
    }

    public EvaluationsContext(DbContextOptions<EvaluationsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Bien> Biens { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Typebien> Typebiens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=ConnectionStrings:Psql");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Idadmin).HasName("admin_pkey");

            entity.ToTable("admin");

            entity.Property(e => e.Idadmin)
                .HasMaxLength(10)
                .HasDefaultValueSql("concat('AD00', nextval('idadmin'::regclass))")
                .IsFixedLength()
                .HasColumnName("idadmin");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .IsFixedLength()
                .HasColumnName("login");
            entity.Property(e => e.Mdp)
                .HasMaxLength(20)
                .IsFixedLength()
                .HasColumnName("mdp");
        });

        modelBuilder.Entity<Bien>(entity =>
        {
            entity.HasKey(e => e.Idbien).HasName("biens_pkey");

            entity.ToTable("biens");

            entity.Property(e => e.Idbien)
                .HasMaxLength(10)
                .HasDefaultValueSql("concat('B00', nextval('idbien'::regclass))")
                .IsFixedLength()
                .HasColumnName("idbien");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Idproprietaire)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("idproprietaire");
            entity.Property(e => e.Idtypebien)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("idtypebien");
            entity.Property(e => e.Loyer)
                .HasPrecision(10, 2)
                .HasColumnName("loyer");
            entity.Property(e => e.Nombien)
                .HasMaxLength(255)
                .IsFixedLength()
                .HasColumnName("nombien");
            entity.Property(e => e.Photos).HasColumnName("photos");
            entity.Property(e => e.Region)
                .HasMaxLength(200)
                .IsFixedLength()
                .HasColumnName("region");

            entity.HasOne(d => d.IdproprietaireNavigation).WithMany(p => p.Biens)
                .HasForeignKey(d => d.Idproprietaire)
                .HasConstraintName("biens_idproprietaire_fkey");

            entity.HasOne(d => d.IdtypebienNavigation).WithMany(p => p.Biens)
                .HasForeignKey(d => d.Idtypebien)
                .HasConstraintName("biens_idtypebien_fkey");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Idclient).HasName("client_pkey");

            entity.ToTable("client");

            entity.Property(e => e.Idclient)
                .HasMaxLength(10)
                .HasDefaultValueSql("concat('C00', nextval('idclient'::regclass))")
                .IsFixedLength()
                .HasColumnName("idclient");
            entity.Property(e => e.Emailclient)
                .HasMaxLength(255)
                .IsFixedLength()
                .HasColumnName("emailclient");
            entity.Property(e => e.Numeroclient)
                .HasMaxLength(11)
                .IsFixedLength()
                .HasColumnName("numeroclient");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Idlocation).HasName("location_pkey");

            entity.ToTable("location");

            entity.Property(e => e.Idlocation)
                .HasMaxLength(10)
                .HasDefaultValueSql("concat('L00', nextval('idlocation'::regclass))")
                .IsFixedLength()
                .HasColumnName("idlocation");
            entity.Property(e => e.Datedebut).HasColumnName("datedebut");
            entity.Property(e => e.Duree).HasColumnName("duree");
            entity.Property(e => e.Idbien)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("idbien");
            entity.Property(e => e.Idclient)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("idclient");

            entity.HasOne(d => d.IdbienNavigation).WithMany(p => p.Locations)
                .HasForeignKey(d => d.Idbien)
                .HasConstraintName("location_idbien_fkey");

            entity.HasOne(d => d.IdclientNavigation).WithMany(p => p.Locations)
                .HasForeignKey(d => d.Idclient)
                .HasConstraintName("location_idclient_fkey");
        });

        modelBuilder.Entity<Typebien>(entity =>
        {
            entity.HasKey(e => e.Idtypebien).HasName("typebien_pkey");

            entity.ToTable("typebien");

            entity.Property(e => e.Idtypebien)
                .HasMaxLength(10)
                .HasDefaultValueSql("concat('T00', nextval('idtypebien'::regclass))")
                .IsFixedLength()
                .HasColumnName("idtypebien");
            entity.Property(e => e.Commission)
                .HasPrecision(3, 1)
                .HasDefaultValueSql("0.0")
                .HasColumnName("commission");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("type");
        });
        modelBuilder.HasSequence("idadmin");
        modelBuilder.HasSequence("idbien");
        modelBuilder.HasSequence("idclient");
        modelBuilder.HasSequence("idlocation");
        modelBuilder.HasSequence("idtypebien");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}