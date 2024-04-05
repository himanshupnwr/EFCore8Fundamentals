﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PublisherData;

#nullable disable

namespace PublisherData.Migrations
{
    [DbContext(typeof(PubContext))]
    [Migration("20240330084209_authoridchange")]
    partial class authoridchange
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PublisherDomain.Author", b =>
                {
                    b.Property<int>("AuthorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AuthorId"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AuthorId");

                    b.ToTable("Authors");

                    b.HasData(
                        new
                        {
                            AuthorId = 1,
                            FirstName = "Rhoda",
                            LastName = "Lerman"
                        },
                        new
                        {
                            AuthorId = 2,
                            FirstName = "Ruth",
                            LastName = "Ozeki"
                        },
                        new
                        {
                            AuthorId = 3,
                            FirstName = "Sofia",
                            LastName = "Segovia"
                        },
                        new
                        {
                            AuthorId = 4,
                            FirstName = "Ursula K.",
                            LastName = "LeGuin"
                        },
                        new
                        {
                            AuthorId = 5,
                            FirstName = "Hugh",
                            LastName = "Howey"
                        },
                        new
                        {
                            AuthorId = 6,
                            FirstName = "Isabelle",
                            LastName = "Allende"
                        });
                });

            modelBuilder.Entity("PublisherDomain.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookId"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<decimal>("BasePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateOnly>("PublishDate")
                        .HasColumnType("date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BookId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Books");

                    b.HasData(
                        new
                        {
                            BookId = 1,
                            AuthorId = 1,
                            BasePrice = 0m,
                            PublishDate = new DateOnly(1989, 3, 1),
                            Title = "In God's Ear"
                        },
                        new
                        {
                            BookId = 2,
                            AuthorId = 2,
                            BasePrice = 0m,
                            PublishDate = new DateOnly(2013, 12, 31),
                            Title = "A Tale For the Time Being"
                        },
                        new
                        {
                            BookId = 3,
                            AuthorId = 3,
                            BasePrice = 0m,
                            PublishDate = new DateOnly(1969, 3, 1),
                            Title = "The Left Hand of Darkness"
                        });
                });

            modelBuilder.Entity("PublisherDomain.Book", b =>
                {
                    b.HasOne("PublisherDomain.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("PublisherDomain.Author", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
