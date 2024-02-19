﻿// <auto-generated />
using Furnish.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Furnish.Migrations
{
    [DbContext(typeof(StoreContext))]
    partial class StoreContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Furnish.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("ProductId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            CategoryId = "Sofa",
                            Name = "Navi",
                            Price = 649.99000000000001
                        },
                        new
                        {
                            ProductId = 2,
                            CategoryId = "Bed",
                            Name = "Serta Peninsula",
                            Price = 899.99000000000001
                        },
                        new
                        {
                            ProductId = 3,
                            CategoryId = "Table",
                            Name = "Mobili Fiver",
                            Price = 945.99000000000001
                        },
                        new
                        {
                            ProductId = 4,
                            CategoryId = "Chair",
                            Name = "Ashley Cashton",
                            Price = 2299.9899999999998
                        },
                        new
                        {
                            ProductId = 5,
                            CategoryId = "Sofa",
                            Name = "Llappuil",
                            Price = 649.99000000000001
                        },
                        new
                        {
                            ProductId = 6,
                            CategoryId = "Bed",
                            Name = "Yuhuashi",
                            Price = 247.0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
