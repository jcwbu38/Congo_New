﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using test5.Models;

namespace test5.Migrations
{
    [DbContext(typeof(InventoryContext))]
    [Migration("20171111185140_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("test5.Models.Inventory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("dateReceived");

                    b.Property<int>("itemID");

                    b.Property<string>("itemName");

                    b.Property<string>("locationID");

                    b.Property<string>("sellerName");

                    b.HasKey("ID");

                    b.ToTable("Inventory");
                });
#pragma warning restore 612, 618
        }
    }
}
