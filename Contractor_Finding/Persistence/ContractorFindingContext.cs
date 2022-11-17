using System;
using System.Collections.Generic;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public partial class ContractorFindingContext : DbContext
{
    public ContractorFindingContext()
    {
    }

    public ContractorFindingContext(DbContextOptions<ContractorFindingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ContractorDetail> ContractorDetails { get; set; }

    public virtual DbSet<Map> Maps { get; set; }

    public virtual DbSet<ServiceProviding> ServiceProvidings { get; set; }

    public virtual DbSet<TbCity> TbCities { get; set; }

    public virtual DbSet<TbGender> TbGenders { get; set; }

    public virtual DbSet<TbState> TbStates { get; set; }

    public virtual DbSet<TbUser> TbUsers { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=65.0.181.176;Database=Contractor_Finding;User Id=admin; Password=Asdf1234*;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContractorDetail>(entity =>
        {
            entity.HasKey(e => e.Pincode).HasName("PK__Contract__546084484ABEEC1B");

            entity.ToTable("Contractor_details");

            entity.HasIndex(e => e.License, "UQ__Contract__A4E54DE4FACBD4B1").IsUnique();

            entity.Property(e => e.Pincode).ValueGeneratedNever();
            entity.Property(e => e.CompanyName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.License)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("license");

            entity.HasOne(d => d.Contractor).WithMany(p => p.ContractorDetails)
                .HasForeignKey(d => d.ContractorId)
                .HasConstraintName("FK__Contracto__Contr__2645B050");

            entity.HasOne(d => d.GenderNavigation).WithMany(p => p.ContractorDetails)
                .HasForeignKey(d => d.Gender)
                .HasConstraintName("FK__Contracto__Gende__2739D489");

            entity.HasOne(d => d.PlaceNavigation).WithMany(p => p.ContractorDetails)
                .HasForeignKey(d => d.Place)
                .HasConstraintName("FK__Contracto__Place__29221CFB");

            entity.HasOne(d => d.ServicesNavigation).WithMany(p => p.ContractorDetails)
                .HasForeignKey(d => d.Services)
                .HasConstraintName("FK__Contracto__Servi__282DF8C2");
        });

        modelBuilder.Entity<Map>(entity =>
        {
            entity.HasKey(e => e.PlaceId).HasName("PK__Map__D5222B4E81E9E661");

            entity.ToTable("Map");

            entity.HasIndex(e => e.Latitude, "UQ__Map__678401E2555E2743").IsUnique();

            entity.HasIndex(e => e.Longitude, "UQ__Map__ED4CDA8C2206C3A7").IsUnique();

            entity.Property(e => e.PlaceId).HasColumnName("PlaceID");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
        });

        modelBuilder.Entity<ServiceProviding>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service___C51BB00AFB7201A5");

            entity.ToTable("Service_providing");

            entity.HasIndex(e => e.ServiceName, "UQ__Service___A42B5F993E1D1212").IsUnique();

            entity.Property(e => e.ServiceId).ValueGeneratedNever();
            entity.Property(e => e.ServiceName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbCity>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__Tb_City__DE9DE020BA42BDCD");

            entity.ToTable("Tb_City");

            entity.Property(e => e.CityId)
                .ValueGeneratedNever()
                .HasColumnName("City_ID");
            entity.Property(e => e.CityName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("City_Name");
            entity.Property(e => e.StateId).HasColumnName("State_ID");

            entity.HasOne(d => d.State).WithMany(p => p.TbCities)
                .HasForeignKey(d => d.StateId)
                .HasConstraintName("FK__Tb_City__State_I__0B91BA14");
        });

        modelBuilder.Entity<TbGender>(entity =>
        {
            entity.HasKey(e => e.GenderId).HasName("PK__Tb_Gende__4E24E9F7B9A92101");

            entity.ToTable("Tb_Gender");

            entity.Property(e => e.GenderId).ValueGeneratedNever();
            entity.Property(e => e.GenderType)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbState>(entity =>
        {
            entity.HasKey(e => e.StateId).HasName("PK__Tb_State__AF9338D7993F2069");

            entity.ToTable("Tb_State");

            entity.Property(e => e.StateId)
                .ValueGeneratedNever()
                .HasColumnName("State_ID");
            entity.Property(e => e.StateName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("State_Name");
        });

        modelBuilder.Entity<TbUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Tb_User__1788CC4C48963B6B");

            entity.ToTable("Tb_User");

            entity.HasIndex(e => e.EmailId, "UQ__Tb_User__7ED91ACEB3FB9349").IsUnique();

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EmailId)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.TypeUserNavigation).WithMany(p => p.TbUsers)
                .HasForeignKey(d => d.TypeUser)
                .HasConstraintName("FK__Tb_User__TypeUse__7F2BE32F");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__User_Typ__F04DF13AF728AE3C");

            entity.ToTable("User_Type");

            entity.Property(e => e.TypeId)
                .ValueGeneratedNever()
                .HasColumnName("typeId");
            entity.Property(e => e.Usertype1)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("usertype");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
