﻿// <auto-generated />
using System;
using FirstProjectNET.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FirstProjectNET.Migrations
{
    [DbContext(typeof(HotelDbContext))]
    [Migration("20250306165359_AddnewColumnToAccount")]
    partial class AddnewColumnToAccount
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FirstProjectNET.Models.Account", b =>
                {
                    b.Property<int>("AccountID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountID"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("AccountID");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("FirstProjectNET.Models.Booking", b =>
                {
                    b.Property<string>("BookingID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CustomerID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateCome")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateGo")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Deposit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("NumberPeople")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("BookingID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Booking", (string)null);
                });

            modelBuilder.Entity("FirstProjectNET.Models.BookingDetail", b =>
                {
                    b.Property<string>("BookingID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("NumberRoom")
                        .HasColumnType("int");

                    b.HasKey("BookingID");

                    b.HasIndex("CategoryID");

                    b.ToTable("BookingDetail", (string)null);
                });

            modelBuilder.Entity("FirstProjectNET.Models.Category", b =>
                {
                    b.Property<string>("CategoryID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Capacity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryID");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("FirstProjectNET.Models.Customer", b =>
                {
                    b.Property<string>("CustomerID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("AccountID")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Membership")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("CustomerID");

                    b.HasIndex("AccountID");

                    b.ToTable("Customer", (string)null);
                });

            modelBuilder.Entity("FirstProjectNET.Models.Image", b =>
                {
                    b.Property<string>("ImageID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoomID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ImageID");

                    b.HasIndex("RoomID");

                    b.ToTable("Image", (string)null);
                });

            modelBuilder.Entity("FirstProjectNET.Models.Invoice", b =>
                {
                    b.Property<string>("InvoiceID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BookingID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StaffID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("InvoiceID");

                    b.HasIndex("BookingID");

                    b.HasIndex("StaffID");

                    b.ToTable("Invoice", (string)null);
                });

            modelBuilder.Entity("FirstProjectNET.Models.Payment", b =>
                {
                    b.Property<string>("PaymentID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DatePayment")
                        .HasColumnType("datetime2");

                    b.Property<string>("InvoiceID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PaymentMethod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentID");

                    b.HasIndex("InvoiceID")
                        .IsUnique()
                        .HasFilter("[InvoiceID] IS NOT NULL");

                    b.ToTable("Payment", (string)null);
                });

            modelBuilder.Entity("FirstProjectNET.Models.Rate", b =>
                {
                    b.Property<string>("RateID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Point")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RateID");

                    b.ToTable("Rate", (string)null);
                });

            modelBuilder.Entity("FirstProjectNET.Models.RentForm", b =>
                {
                    b.Property<string>("RentFormID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BookingID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CustomerID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateCheckIn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateCheckOut")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("datetime2");

                    b.Property<string>("RoomID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Sale")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("StaffID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("RentFormID");

                    b.HasIndex("BookingID")
                        .IsUnique()
                        .HasFilter("[BookingID] IS NOT NULL");

                    b.HasIndex("CustomerID");

                    b.HasIndex("RoomID");

                    b.HasIndex("StaffID");

                    b.ToTable("RentForm", (string)null);
                });

            modelBuilder.Entity("FirstProjectNET.Models.Room", b =>
                {
                    b.Property<string>("RoomID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoomID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("FirstProjectNET.Models.RoomService", b =>
                {
                    b.Property<int>("RoomServiceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoomServiceID"));

                    b.Property<string>("RoomID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ServiceID")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("RoomServiceID");

                    b.HasIndex("RoomID");

                    b.HasIndex("ServiceID");

                    b.ToTable("RoomService", (string)null);
                });

            modelBuilder.Entity("FirstProjectNET.Models.Service", b =>
                {
                    b.Property<string>("ServiceID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ServiceID");

                    b.ToTable("Service", (string)null);
                });

            modelBuilder.Entity("FirstProjectNET.Models.Staff", b =>
                {
                    b.Property<string>("StaffID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("AccountID")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("StaffID");

                    b.HasIndex("AccountID")
                        .IsUnique()
                        .HasFilter("[AccountID] IS NOT NULL");

                    b.ToTable("Staff", (string)null);
                });

            modelBuilder.Entity("FirstProjectNET.Models.Booking", b =>
                {
                    b.HasOne("FirstProjectNET.Models.Customer", "Customer")
                        .WithMany("Bookings")
                        .HasForeignKey("CustomerID");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("FirstProjectNET.Models.BookingDetail", b =>
                {
                    b.HasOne("FirstProjectNET.Models.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FirstProjectNET.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("FirstProjectNET.Models.Customer", b =>
                {
                    b.HasOne("FirstProjectNET.Models.Account", "Account")
                        .WithMany("Customers")
                        .HasForeignKey("AccountID");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("FirstProjectNET.Models.Image", b =>
                {
                    b.HasOne("FirstProjectNET.Models.Room", "Room")
                        .WithMany("Images")
                        .HasForeignKey("RoomID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("FirstProjectNET.Models.Invoice", b =>
                {
                    b.HasOne("FirstProjectNET.Models.Booking", "Booking")
                        .WithMany()
                        .HasForeignKey("BookingID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FirstProjectNET.Models.Staff", "Staff")
                        .WithMany("Invoices")
                        .HasForeignKey("StaffID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Staff");
                });

            modelBuilder.Entity("FirstProjectNET.Models.Payment", b =>
                {
                    b.HasOne("FirstProjectNET.Models.Invoice", "Invoice")
                        .WithOne("Payment")
                        .HasForeignKey("FirstProjectNET.Models.Payment", "InvoiceID");

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("FirstProjectNET.Models.RentForm", b =>
                {
                    b.HasOne("FirstProjectNET.Models.Booking", "Booking")
                        .WithOne("RentForm")
                        .HasForeignKey("FirstProjectNET.Models.RentForm", "BookingID");

                    b.HasOne("FirstProjectNET.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FirstProjectNET.Models.Room", "Room")
                        .WithMany("RentForms")
                        .HasForeignKey("RoomID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FirstProjectNET.Models.Staff", "Staff")
                        .WithMany("RentForms")
                        .HasForeignKey("StaffID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Booking");

                    b.Navigation("Customer");

                    b.Navigation("Room");

                    b.Navigation("Staff");
                });

            modelBuilder.Entity("FirstProjectNET.Models.Room", b =>
                {
                    b.HasOne("FirstProjectNET.Models.Category", "Category")
                        .WithMany("Rooms")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("FirstProjectNET.Models.RoomService", b =>
                {
                    b.HasOne("FirstProjectNET.Models.Room", "Room")
                        .WithMany("RoomServices")
                        .HasForeignKey("RoomID");

                    b.HasOne("FirstProjectNET.Models.Service", "Service")
                        .WithMany("RoomServices")
                        .HasForeignKey("ServiceID");

                    b.Navigation("Room");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("FirstProjectNET.Models.Staff", b =>
                {
                    b.HasOne("FirstProjectNET.Models.Account", "Account")
                        .WithOne("Staff")
                        .HasForeignKey("FirstProjectNET.Models.Staff", "AccountID");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("FirstProjectNET.Models.Account", b =>
                {
                    b.Navigation("Customers");

                    b.Navigation("Staff");
                });

            modelBuilder.Entity("FirstProjectNET.Models.Booking", b =>
                {
                    b.Navigation("RentForm");
                });

            modelBuilder.Entity("FirstProjectNET.Models.Category", b =>
                {
                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("FirstProjectNET.Models.Customer", b =>
                {
                    b.Navigation("Bookings");
                });

            modelBuilder.Entity("FirstProjectNET.Models.Invoice", b =>
                {
                    b.Navigation("Payment");
                });

            modelBuilder.Entity("FirstProjectNET.Models.Room", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("RentForms");

                    b.Navigation("RoomServices");
                });

            modelBuilder.Entity("FirstProjectNET.Models.Service", b =>
                {
                    b.Navigation("RoomServices");
                });

            modelBuilder.Entity("FirstProjectNET.Models.Staff", b =>
                {
                    b.Navigation("Invoices");

                    b.Navigation("RentForms");
                });
#pragma warning restore 612, 618
        }
    }
}
