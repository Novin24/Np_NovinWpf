﻿// <auto-generated />
using System;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations.NovinDb
{
    [DbContext(typeof(NovinDbContext))]
    [Migration("20240203151705_convertAllPersianMonthToByte")]
    partial class convertAllPersianMonthToByte
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.NovinEntity.Bank.Bank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Account_num")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Bank_Branch")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Bank_ID")
                        .HasColumnType("int");

                    b.Property<string>("Bank_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifireId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Bank");
                });

            modelBuilder.Entity("Domain.NovinEntity.Customers.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Buyer")
                        .HasColumnType("bit");

                    b.Property<long>("CashCredit")
                        .HasColumnType("bigint");

                    b.Property<long>("ChequeCredit")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("HaveCashCredit")
                        .HasColumnType("bit");

                    b.Property<bool>("HaveChequeGuarantee")
                        .HasColumnType("bit");

                    b.Property<bool>("HavePromissoryNote")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NationalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("PromissoryNote")
                        .HasColumnType("bigint");

                    b.Property<bool>("Seller")
                        .HasColumnType("bit");

                    b.Property<long>("TotalCredit")
                        .HasColumnType("bigint");

                    b.Property<byte>("Type")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("Domain.NovinEntity.Documents.Document", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWSEQUENTIALID()");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<bool>("ReceivedOrPaid")
                        .HasColumnType("bit");

                    b.Property<long>("Serial")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Serial"));

                    b.Property<DateTime>("SubmitDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("Serial");

                    b.ToTable("Document");
                });

            modelBuilder.Entity("Domain.NovinEntity.Materials.Material", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<double>("Entity")
                        .HasColumnType("float");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsManufacturedGoods")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("LastPrice")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhysicalAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Serial")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UnitId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("UnitId");

                    b.ToTable("Material");
                });

            modelBuilder.Entity("Domain.NovinEntity.Materials.Unit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Descrip")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Unit");
                });

            modelBuilder.Entity("Domain.NovinEntity.Workers.FinancialAid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<long>("AmountOf")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("PersianMonth")
                        .HasColumnType("tinyint");

                    b.Property<int>("PersianYear")
                        .HasColumnType("int");

                    b.Property<int>("WorkerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersianMonth");

                    b.HasIndex("PersianYear");

                    b.HasIndex("WorkerId");

                    b.ToTable("FinancialAid");
                });

            modelBuilder.Entity("Domain.NovinEntity.Workers.Function", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte>("AmountOf")
                        .HasColumnType("tinyint");

                    b.Property<byte>("AmountOfOverTime")
                        .HasColumnType("tinyint");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("PersianMonth")
                        .HasColumnType("tinyint");

                    b.Property<int>("PersianYear")
                        .HasColumnType("int");

                    b.Property<int>("WorkerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersianMonth");

                    b.HasIndex("PersianYear");

                    b.HasIndex("WorkerId");

                    b.ToTable("Function");
                });

            modelBuilder.Entity("Domain.NovinEntity.Workers.Salary", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<long>("AmountOf")
                        .HasColumnType("bigint");

                    b.Property<long>("ChildAllowance")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("FinancialAid")
                        .HasColumnType("bigint");

                    b.Property<long>("Insurance")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("LeftOver")
                        .HasColumnType("bigint");

                    b.Property<long>("LoanInstallment")
                        .HasColumnType("bigint");

                    b.Property<long>("OtherAdditions")
                        .HasColumnType("bigint");

                    b.Property<long>("OtherDeductions")
                        .HasColumnType("bigint");

                    b.Property<long>("OverTime")
                        .HasColumnType("bigint");

                    b.Property<byte>("PersianMonth")
                        .HasColumnType("tinyint");

                    b.Property<int>("PersianYear")
                        .HasColumnType("int");

                    b.Property<long>("RightHousingAndFood")
                        .HasColumnType("bigint");

                    b.Property<long>("Tax")
                        .HasColumnType("bigint");

                    b.Property<int>("WorkerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersianMonth");

                    b.HasIndex("PersianYear");

                    b.HasIndex("WorkerId");

                    b.ToTable("Salary");
                });

            modelBuilder.Entity("Domain.NovinEntity.Workers.Worker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("DayInMonth")
                        .HasColumnType("tinyint");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("InsurancePremium")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("JobTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("LastModifireId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NationalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("OverTimeSalary")
                        .HasColumnType("bigint");

                    b.Property<int>("PersonnelId")
                        .HasColumnType("int");

                    b.Property<long>("Salary")
                        .HasColumnType("bigint");

                    b.Property<long>("ShiftOverTimeSalary")
                        .HasColumnType("bigint");

                    b.Property<long>("ShiftSalary")
                        .HasColumnType("bigint");

                    b.Property<byte>("ShiftStatus")
                        .HasColumnType("tinyint");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<byte>("Status")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("PersonnelId");

                    b.ToTable("Worker");
                });

            modelBuilder.Entity("Domain.NovinEntity.Materials.Material", b =>
                {
                    b.HasOne("Domain.NovinEntity.Materials.Unit", "Unit")
                        .WithMany("Materials")
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("Domain.NovinEntity.Workers.FinancialAid", b =>
                {
                    b.HasOne("Domain.NovinEntity.Workers.Worker", "Worker")
                        .WithMany("Aids")
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Worker");
                });

            modelBuilder.Entity("Domain.NovinEntity.Workers.Function", b =>
                {
                    b.HasOne("Domain.NovinEntity.Workers.Worker", "Worker")
                        .WithMany("Functions")
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Worker");
                });

            modelBuilder.Entity("Domain.NovinEntity.Workers.Salary", b =>
                {
                    b.HasOne("Domain.NovinEntity.Workers.Worker", "Worker")
                        .WithMany("Salaries")
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Worker");
                });

            modelBuilder.Entity("Domain.NovinEntity.Materials.Unit", b =>
                {
                    b.Navigation("Materials");
                });

            modelBuilder.Entity("Domain.NovinEntity.Workers.Worker", b =>
                {
                    b.Navigation("Aids");

                    b.Navigation("Functions");

                    b.Navigation("Salaries");
                });
#pragma warning restore 612, 618
        }
    }
}
