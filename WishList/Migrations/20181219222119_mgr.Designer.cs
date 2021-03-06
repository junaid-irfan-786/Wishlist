﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WishList.Data;

namespace WishList.Migrations
{
    [DbContext(typeof(ShopifyContext))]
    [Migration("20181219222119_mgr")]
    partial class mgr
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("WishList.Models.Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("StoreName");

                    b.Property<string>("customer_email");

                    b.Property<string>("customer_id");

                    b.Property<string>("customer_name");

                    b.Property<string>("product_id");

                    b.Property<string>("product_title");

                    b.Property<string>("variant_barcode");

                    b.Property<string>("variant_id");

                    b.Property<string>("variant_image");

                    b.Property<string>("variant_sku");

                    b.HasKey("CustomerID");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("WishList.Models.Shopify", b =>
                {
                    b.Property<int>("ShopifyID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("StoreName");

                    b.Property<string>("Token");

                    b.HasKey("ShopifyID");

                    b.ToTable("Shopify");
                });
#pragma warning restore 612, 618
        }
    }
}
