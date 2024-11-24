using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace lab3_5.Server.Models;

public partial class DeliverycourierserviceContext : DbContext
{
    public DeliverycourierserviceContext()
    {
    }

    public DeliverycourierserviceContext(DbContextOptions<DeliverycourierserviceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Courier> Couriers { get; set; }

    public virtual DbSet<CourierPerformance> CourierPerformances { get; set; }

    public virtual DbSet<Delivery> Deliveries { get; set; }

    public virtual DbSet<DeliveryAddress> DeliveryAddresses { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Transport> Transports { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=12345678;Database=deliverycourierservice;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("client_pkey");

            entity.ToTable("client");

            entity.Property(e => e.PersonId)
                .ValueGeneratedNever()
                .HasColumnName("person_id");
            entity.Property(e => e.AddressId).HasColumnName("address_id");

            entity.HasOne(d => d.Address).WithMany(p => p.Clients)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("client_address_id_fkey");

            entity.HasOne(d => d.Person).WithOne(p => p.Client)
                .HasForeignKey<Client>(d => d.PersonId)
                .HasConstraintName("client_person_id_fkey");
        });

        modelBuilder.Entity<Courier>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("courier_pkey");

            entity.ToTable("courier");

            entity.HasIndex(e => e.LicencePlate, "unique_licence_plate").IsUnique();

            entity.Property(e => e.PersonId)
                .ValueGeneratedNever()
                .HasColumnName("person_id");
            entity.Property(e => e.LicencePlate)
                .HasMaxLength(15)
                .HasColumnName("licence_plate");


            entity.HasOne(d => d.LicencePlateNavigation).WithOne(p => p.Courier)
                .HasForeignKey<Courier>(d => d.LicencePlate)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("courier_licence_plate_fkey");

            entity.HasOne(d => d.Person).WithOne(p => p.Courier)
                .HasForeignKey<Courier>(d => d.PersonId)
                .HasConstraintName("courier_person_id_fkey");
        });

        modelBuilder.Entity<CourierPerformance>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("courier_performance");

            entity.Property(e => e.AvgDeliveryTime).HasColumnName("avg_delivery_time");
            entity.Property(e => e.CourierName).HasColumnName("courier_name");
            entity.Property(e => e.DeliveryRank).HasColumnName("delivery_rank");
            entity.Property(e => e.PersonId).HasColumnName("person_id");
            entity.Property(e => e.TotalDeliveries).HasColumnName("total_deliveries");
        });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.HasKey(e => e.DeliveryId).HasName("delivery_pkey");

            entity.ToTable("delivery");

            entity.Property(e => e.DeliveryId).HasColumnName("delivery_id");
            entity.Property(e => e.ActualDuration).HasColumnName("actual_duration");
            entity.Property(e => e.AddressId).HasColumnName("address_id");
            entity.Property(e => e.CourierId).HasColumnName("courier_id");
            entity.Property(e => e.DesiredDuration).HasColumnName("desired_duration");
            entity.Property(e => e.EndTime)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("end_time");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.StartTime)
                .HasColumnType("timestamp(0) without time zone")
                .HasColumnName("start_time");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValueSql("'pending'::character varying")
                .HasColumnName("status");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.Address).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("delivery_address_id_fkey");

            entity.HasOne(d => d.Courier).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.CourierId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("delivery_courier_id_fkey");

            entity.HasOne(d => d.Order).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("delivery_order_id_fkey");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Deliveries)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("delivery_warehouse_id_fkey");
        });

        modelBuilder.Entity<DeliveryAddress>(entity =>
        {
            entity.HasKey(e => e.DeliveryAddressId).HasName("address_pkey");

            entity.ToTable("delivery_address");

            entity.Property(e => e.DeliveryAddressId)
                .HasDefaultValueSql("nextval('address_address_id_seq'::regclass)")
                .HasColumnName("delivery_address_id");
            entity.Property(e => e.ApartmentNumber)
                .HasMaxLength(10)
                .HasColumnName("apartment_number");
            entity.Property(e => e.BuildingNumber)
                .HasMaxLength(10)
                .HasColumnName("building_number");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .HasColumnName("country");
            entity.Property(e => e.Floor).HasColumnName("floor");
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .HasColumnName("street");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("Order_pkey");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Cost)
                .HasPrecision(10, 2)
                .HasColumnName("cost");
            entity.Property(e => e.Description).HasColumnName("description");

            entity.HasOne(d => d.Client).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_client_id");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("person_pkey");

            entity.ToTable("person");

            entity.HasIndex(e => e.PhoneNumber, "unique_phone_number").IsUnique();

            entity.Property(e => e.PersonId).HasColumnName("person_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .HasColumnName("phone_number");
        });

        modelBuilder.Entity<Transport>(entity =>
        {
            entity.HasKey(e => e.LicencePlate).HasName("transport_pkey");

            entity.ToTable("transport");

            entity.Property(e => e.LicencePlate)
                .HasMaxLength(15)
                .HasColumnName("licence_plate");
            entity.Property(e => e.Model)
                .HasMaxLength(50)
                .HasColumnName("model");
            entity.Property(e => e.TransportType)
                .HasMaxLength(20)
                .HasColumnName("transport_type");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.WarehouseId).HasName("warehouse_pkey");

            entity.ToTable("warehouse");

            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");
            entity.Property(e => e.ContactNumber)
                .HasMaxLength(15)
                .HasColumnName("contact_number");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
