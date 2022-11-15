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

    public virtual DbSet<CityName> CityNames { get; set; }

    public virtual DbSet<ContractorDetail> ContractorDetails { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Map> Maps { get; set; }

    public virtual DbSet<ServiceProviding> ServiceProvidings { get; set; }

    public virtual DbSet<StateName> StateNames { get; set; }

    public virtual DbSet<TypeUser> TypeUsers { get; set; }

    public virtual DbSet<UserDetail> UserDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=65.0.181.176;Database=Contractor_Finding;User Id=admin; Password=Asdf1234*;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CityName>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__city_nam__B4BEB95ED59CB7ED");

            entity.ToTable("city_name");

            entity.Property(e => e.CityId)
                .ValueGeneratedNever()
                .HasColumnName("cityId");
            entity.Property(e => e.CityName1)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("cityName");
        });

        modelBuilder.Entity<ContractorDetail>(entity =>
        {
            entity.HasKey(e => e.License).HasName("PK__Contract__A4E54DE5DFBF8511");

            entity.ToTable("Contractor_details");

            entity.Property(e => e.License)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("license");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(30)
                .IsUnicode(false);

            entity.HasOne(d => d.Contractor).WithMany(p => p.ContractorDetails)
                .HasForeignKey(d => d.ContractorId)
                .HasConstraintName("FK__Contracto__Contr__6A30C649");

            entity.HasOne(d => d.LocationNavigation).WithMany(p => p.ContractorDetails)
                .HasForeignKey(d => d.Location)
                .HasConstraintName("FK__Contracto__Locat__6C190EBB");

            entity.HasOne(d => d.ServicesNavigation).WithMany(p => p.ContractorDetails)
                .HasForeignKey(d => d.Services)
                .HasConstraintName("FK__Contracto__Servi__6B24EA82");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.GenderId).HasName("PK__Gender__306E2220B8999231");

            entity.ToTable("Gender");

            entity.HasIndex(e => e.GenderName, "UQ__Gender__14B63E736E5C2369").IsUnique();

            entity.Property(e => e.GenderId)
                .ValueGeneratedNever()
                .HasColumnName("genderID");
            entity.Property(e => e.GenderName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("genderName");
        });

        modelBuilder.Entity<Map>(entity =>
        {
            entity.HasKey(e => e.PlaceId).HasName("PK__Map__D5222B4E1197A651");

            entity.ToTable("Map");

            entity.HasIndex(e => e.Latitude, "UQ__Map__678401E2D7232E4D").IsUnique();

            entity.HasIndex(e => e.Longitude, "UQ__Map__ED4CDA8C69800BD1").IsUnique();

            entity.Property(e => e.PlaceId).HasColumnName("PlaceID");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
        });

        modelBuilder.Entity<ServiceProviding>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service___455070DFEC20AA86");

            entity.ToTable("Service_providing");

            entity.HasIndex(e => e.ServiceName, "UQ__Service___A42B5F9982220EDB").IsUnique();

            entity.Property(e => e.ServiceId)
                .ValueGeneratedNever()
                .HasColumnName("serviceId");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StateName>(entity =>
        {
            entity.HasKey(e => e.StateId).HasName("PK__state_na__A667B9E1A2B43E04");

            entity.ToTable("state_name");

            entity.HasIndex(e => e.StateName1, "UQ__state_na__8B97A96089669566").IsUnique();

            entity.Property(e => e.StateId)
                .ValueGeneratedNever()
                .HasColumnName("stateId");
            entity.Property(e => e.StateName1)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("stateName");
        });

        modelBuilder.Entity<TypeUser>(entity =>
        {
            entity.HasKey(e => e.UserType).HasName("PK__Type_Use__1FDA748A9476A190");

            entity.ToTable("Type_User");

            entity.HasIndex(e => e.Id, "UQ__Type_Use__3213E83E9EE98A4A").IsUnique();

            entity.Property(e => e.UserType)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("user_type");
            entity.Property(e => e.Duration)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("duration");
            entity.Property(e => e.Id)
                .IsRequired()
                .HasColumnName("id");
        });

        modelBuilder.Entity<UserDetail>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User_Det__CB9A1CFF8F69A4EF");

            entity.ToTable("User_Details");

            entity.HasIndex(e => e.PhoneNumber, "UQ__User_Det__4849DA0195AC9B60").IsUnique();

            entity.HasIndex(e => e.MailId, "UQ__User_Det__F5CD78A97A9F4DCE").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.FirstName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("firstName");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.LastName)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("lastName");
            entity.Property(e => e.MailId)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("mailId");
            entity.Property(e => e.Password)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber).HasColumnName("phoneNumber");
            entity.Property(e => e.StateName).HasColumnName("stateName");
            entity.Property(e => e.TypeUser).HasColumnName("typeUser");

            entity.HasOne(d => d.CityNameNavigation).WithMany(p => p.UserDetails)
                .HasForeignKey(d => d.CityName)
                .HasConstraintName("FK__User_Deta__CityN__5FB337D6");

            entity.HasOne(d => d.GenderNavigation).WithMany(p => p.UserDetails)
                .HasForeignKey(d => d.Gender)
                .HasConstraintName("FK__User_Deta__gende__5DCAEF64");

            entity.HasOne(d => d.StateNameNavigation).WithMany(p => p.UserDetails)
                .HasForeignKey(d => d.StateName)
                .HasConstraintName("FK__User_Deta__state__5EBF139D");

            entity.HasOne(d => d.TypeUserNavigation).WithMany(p => p.UserDetails)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(d => d.TypeUser)
                .HasConstraintName("FK__User_Deta__typeU__5CD6CB2B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
