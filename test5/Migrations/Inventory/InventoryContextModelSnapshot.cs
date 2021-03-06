﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using test5.Models;

namespace test5.Migrations
{
    [DbContext(typeof(InventoryContext))]
    partial class InventoryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("test5.Models.Inventory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EntryDate");

                    b.Property<DateTime>("dateReceived");

                    b.Property<string>("description");

                    b.Property<string>("detailedDescription");

                    b.Property<int>("discountPrice");

                    b.Property<string>("image");

                    b.Property<int>("itemID");

                    b.Property<string>("itemName");

                    b.Property<string>("locationID");

                    b.Property<double>("price");

                    b.Property<int>("quantity");

                    b.Property<string>("sellerName");

                    b.HasKey("ID");

                    b.ToTable("Inventory");
                });
#pragma warning restore 612, 618
        }
    }
}
