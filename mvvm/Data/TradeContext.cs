using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace mvvm.Data;

public partial class TradeContext : DbContext
{
    public TradeContext()
    {
    }

    public TradeContext(DbContextOptions<TradeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Deliever> Delievers { get; set; }

    public virtual DbSet<Kategory> Kategories { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Orderproduct> Orderproducts { get; set; }

    public virtual DbSet<Orderuser> Orderusers { get; set; }

    public virtual DbSet<Pickuppoint> Pickuppoints { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("server=localhost;user=root;password=Qwerty123;database=trade", ServerVersion.Parse("8.0.25-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Deliever>(entity =>
        {
            entity.HasKey(e => e.Iddeliever).HasName("PRIMARY");

            entity.ToTable("deliever");

            entity.Property(e => e.Iddeliever)
                .ValueGeneratedNever()
                .HasColumnName("iddeliever");
            entity.Property(e => e.ProductDeliever)
                .HasMaxLength(45)
                .HasColumnName("productDeliever");
        });

        modelBuilder.Entity<Kategory>(entity =>
        {
            entity.HasKey(e => e.Idkategory).HasName("PRIMARY");

            entity.ToTable("kategory");

            entity.Property(e => e.Idkategory)
                .ValueGeneratedNever()
                .HasColumnName("idkategory");
            entity.Property(e => e.ProductKategory)
                .HasMaxLength(45)
                .HasColumnName("productKategory");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.IdManufacturer).HasName("PRIMARY");

            entity.ToTable("manufacturer");

            entity.Property(e => e.IdManufacturer)
                .ValueGeneratedNever()
                .HasColumnName("idManufacturer");
            entity.Property(e => e.ProductManufacture)
                .HasMaxLength(45)
                .HasColumnName("productManufacture");
        });

        modelBuilder.Entity<Orderproduct>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductArticleNumber })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("orderproduct");

            entity.HasIndex(e => e.ProductArticleNumber, "fk_article_idx");

            entity.HasIndex(e => e.OrderId, "fk_orderid_idx");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductArticleNumber)
                .HasMaxLength(100)
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.ProductCount).HasColumnName("productCount");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderproducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_id");
        });

        modelBuilder.Entity<Orderuser>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("orderuser");

            entity.HasIndex(e => e.OrderPickupPoint, "fk_order_pickup_idx");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.OrderCode).HasMaxLength(45);
            entity.Property(e => e.OrderFIO)
                .HasMaxLength(100)
                .HasColumnName("OrderFIO");
            entity.Property(e => e.OrderStatus).HasColumnType("text");
            entity.Property(e => e.OrderCost).HasPrecision(19, 4);
            entity.Property(e => e.OrderDiscountAmmount).HasPrecision(19, 4);
            entity.HasOne(d => d.OrderPickupPointNavigation).WithMany(p => p.Orderusers)
                .HasForeignKey(d => d.OrderPickupPoint)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_pickup");
        });

        modelBuilder.Entity<Pickuppoint>(entity =>
        {
            entity.HasKey(e => e.IdPickupPoint).HasName("PRIMARY");

            entity.ToTable("pickuppoint");

            entity.Property(e => e.IdPickupPoint)
                .ValueGeneratedNever()
                .HasColumnName("idPickupPoint");
            entity.Property(e => e.Country)
                .HasMaxLength(45)
                .HasColumnName("country");
            entity.Property(e => e.House).HasColumnName("house");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.Street)
                .HasMaxLength(45)
                .HasColumnName("street");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductArticleNumber).HasName("PRIMARY");

            entity.ToTable("product");

            entity.HasIndex(e => e.ProductCategory, "fk_category_idx");

            entity.HasIndex(e => e.ProductManufacturer, "fk_manufacture_idx");

            entity.Property(e => e.ProductArticleNumber)
                .HasMaxLength(100)
                .UseCollation("utf8_general_ci")
                .HasCharSet("utf8");
            entity.Property(e => e.ProductCost).HasPrecision(19, 4);
            entity.Property(e => e.ProductDescription).HasColumnType("text");
            entity.Property(e => e.ProductName).HasColumnType("text");
            entity.Property(e => e.ProductPhoto).HasColumnType("text");
            entity.Property(e => e.ProductStatus).HasMaxLength(20);

            entity.HasOne(d => d.ProductCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_category");

            entity.HasOne(d => d.ProductManufacturerNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductManufacturer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_manufacture");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.UserRole, "UserRole");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserLogin).HasColumnType("text");
            entity.Property(e => e.UserName).HasMaxLength(100);
            entity.Property(e => e.UserPassword).HasColumnType("text");
            entity.Property(e => e.UserPatronymic).HasMaxLength(100);
            entity.Property(e => e.UserSurname).HasMaxLength(100);

            entity.HasOne(d => d.UserRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
