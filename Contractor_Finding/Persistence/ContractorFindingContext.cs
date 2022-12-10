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

    public virtual DbSet<ServiceProviding> ServiceProvidings { get; set; }

    public virtual DbSet<TbBuilding> TbBuildings { get; set; }

    public virtual DbSet<TbCustomer> TbCustomers { get; set; }

    public virtual DbSet<TbGender> TbGenders { get; set; }

    public virtual DbSet<TbUser> TbUsers { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=65.0.181.176;Database=Contractor_Finding;User Id=admin; Password=Asdf1234*;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContractorDetail>(entity =>
        {
            entity.HasKey(e => e.License).HasName("PK__Contract__A4E54DE5171ACA52");

            entity.ToTable("Contractor_details");

            entity.Property(e => e.License)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("license");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber).HasColumnName("phoneNumber");

            entity.HasOne(d => d.Contractor).WithMany(p => p.ContractorDetails)
                .HasForeignKey(d => d.ContractorId)
                .HasConstraintName("FK__Contracto__Contr__00DF2177");

            entity.HasOne(d => d.GenderNavigation).WithMany(p => p.ContractorDetails)
                .HasForeignKey(d => d.Gender)
                .HasConstraintName("FK__Contracto__Gende__01D345B0");

            entity.HasOne(d => d.ServicesNavigation).WithMany(p => p.ContractorDetails)
                .HasForeignKey(d => d.Services)
                .HasConstraintName("FK__Contracto__Servi__02C769E9");
        });

        modelBuilder.Entity<ServiceProviding>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Service___C51BB00A24DEE480");

            entity.ToTable("Service_providing");

            entity.HasIndex(e => e.ServiceName, "UQ__Service___A42B5F9967930F36").IsUnique();

            entity.Property(e => e.ServiceId).ValueGeneratedNever();
            entity.Property(e => e.ServiceName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbBuilding>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tb_Build__3214EC27A87D4A5F");

            entity.ToTable("Tb_Building");

            entity.HasIndex(e => e.Building, "UQ__Tb_Build__55366371B58A2F27").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Building)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbCustomer>(entity =>
        {
            entity.HasKey(e => e.RegistrationNo).HasName("PK__Tb_Custo__6EF5E04387FA6484");

            entity.ToTable("Tb_Customer");

            entity.Property(e => e.RegistrationNo)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.BuildingType).HasColumnName("Building_Type");
            entity.Property(e => e.LandSqft).HasColumnName("Land_sqft");

            entity.HasOne(d => d.BuildingTypeNavigation).WithMany(p => p.TbCustomers)
                .HasForeignKey(d => d.BuildingType)
                .HasConstraintName("FK__Tb_Custom__Build__0880433F");

            entity.HasOne(d => d.Customer).WithMany(p => p.TbCustomers)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK__Tb_Custom__Custo__0C50D423");
        });

        modelBuilder.Entity<TbGender>(entity =>
        {
            entity.HasKey(e => e.GenderId).HasName("PK__Tb_Gende__4E24E9F79C4093C8");

            entity.ToTable("Tb_Gender");

            entity.Property(e => e.GenderId).ValueGeneratedNever();
            entity.Property(e => e.GenderType)
                .HasMaxLength(30)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TbUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Tb_User__1788CC4C67328C76");

            entity.ToTable("Tb_User");

            entity.HasIndex(e => e.EmailId, "UQ__Tb_User__7ED91ACED3D6A03E").IsUnique();

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
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.TypeUserNavigation).WithMany(p => p.TbUsers)
                .HasForeignKey(d => d.TypeUser)
                .HasConstraintName("FK__Tb_User__TypeUse__690797E6");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("PK__User_Typ__F04DF13A2C2E4F42");

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
