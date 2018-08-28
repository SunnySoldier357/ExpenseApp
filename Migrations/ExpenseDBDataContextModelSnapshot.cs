﻿// <auto-generated />
using System;
using ExpenseApp.Models.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ExpenseApp.Migrations
{
    [DbContext(typeof(ExpenseDBDataContext))]
    partial class ExpenseDBDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ExpenseApp.Models.DB.Account", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Name");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("ExpenseApp.Models.DB.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("NEWID()");

                    b.Property<Guid?>("ApproverId");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("LocationName");

                    b.HasKey("Id");

                    b.HasIndex("ApproverId");

                    b.HasIndex("LocationName");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("ExpenseApp.Models.DB.ExpenseEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("AccountName");

                    b.Property<string>("Cost");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description");

                    b.Property<string>("ExpenseFormStatementNumber");

                    b.Property<Guid?>("ReceiptId");

                    b.HasKey("Id");

                    b.HasIndex("AccountName");

                    b.HasIndex("ExpenseFormStatementNumber");

                    b.HasIndex("ReceiptId");

                    b.ToTable("ExpenseEntries");
                });

            modelBuilder.Entity("ExpenseApp.Models.DB.ExpenseForm", b =>
                {
                    b.Property<string>("StatementNumber")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<Guid?>("EmployeeId");

                    b.Property<DateTime>("From");

                    b.Property<string>("Project")
                        .IsRequired();

                    b.Property<string>("Purpose")
                        .IsRequired();

                    b.Property<string>("RejectionComment");

                    b.Property<int>("Status");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<DateTime>("To");

                    b.HasKey("StatementNumber");

                    b.HasIndex("EmployeeId");

                    b.ToTable("ExpenseForms");
                });

            modelBuilder.Entity("ExpenseApp.Models.DB.Location", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.HasKey("Name");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("ExpenseApp.Models.DB.Receipt", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("NEWID()");

                    b.Property<string>("FileName");

                    b.Property<byte[]>("ReceiptImage");

                    b.HasKey("Id");

                    b.ToTable("Receipts");
                });

            modelBuilder.Entity("ExpenseApp.Models.DB.Employee", b =>
                {
                    b.HasOne("ExpenseApp.Models.DB.Employee", "Approver")
                        .WithMany()
                        .HasForeignKey("ApproverId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ExpenseApp.Models.DB.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationName");
                });

            modelBuilder.Entity("ExpenseApp.Models.DB.ExpenseEntry", b =>
                {
                    b.HasOne("ExpenseApp.Models.DB.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountName");

                    b.HasOne("ExpenseApp.Models.DB.ExpenseForm")
                        .WithMany("Entries")
                        .HasForeignKey("ExpenseFormStatementNumber");

                    b.HasOne("ExpenseApp.Models.DB.Receipt", "Receipt")
                        .WithMany()
                        .HasForeignKey("ReceiptId");
                });

            modelBuilder.Entity("ExpenseApp.Models.DB.ExpenseForm", b =>
                {
                    b.HasOne("ExpenseApp.Models.DB.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId");
                });
#pragma warning restore 612, 618
        }
    }
}
