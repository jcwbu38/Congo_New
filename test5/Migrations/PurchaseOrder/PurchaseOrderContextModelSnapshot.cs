﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using test5.Models;

namespace test5.Migrations.PurchaseOrder
{
    [DbContext(typeof(PurchaseOrderContext))]
    partial class PurchaseOrderContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("test5.Models.PurchaseOrder", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("address1");

                    b.Property<string>("address2");

                    b.Property<string>("city");

                    b.Property<DateTime>("datePurchased");

                    b.Property<string>("email");

                    b.Property<string>("firstName");

                    b.Property<string>("lastName");

                    b.Property<string>("productID");

                    b.Property<string>("productName");

                    b.Property<DateTime>("shipDate");

                    b.Property<string>("state");

                    b.Property<string>("stowLocation");

                    b.Property<int>("userID");

                    b.Property<int>("zip");

                    b.HasKey("ID");

                    b.ToTable("PurchaseOrder");
                });
#pragma warning restore 612, 618
        }
    }
}
