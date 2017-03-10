﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CC98.Software.Data;

namespace CC98.Software.Migrations
{
    [DbContext(typeof(SoftwareDbContext))]
    [Migration("20170310103544_V1")]
    partial class V1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CC98.Software.Data.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int?>("ParentId");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("CC98.Software.Data.Feedback", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Message");

                    b.Property<string>("RecieverName");

                    b.Property<string>("SenderName");

                    b.Property<DateTimeOffset>("Time");

                    b.HasKey("Id");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("CC98.Software.Data.Software", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ClassId");

                    b.Property<int>("DownloadNum");

                    b.Property<string>("FileLocation");

                    b.Property<string>("Introduction");

                    b.Property<bool>("IsFrequent");

                    b.Property<string>("Name");

                    b.Property<string>("PhotoLocation");

                    b.Property<int>("Platform");

                    b.Property<long>("Size");

                    b.Property<DateTimeOffset>("UpdateTime");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.ToTable("Softwares");
                });

            modelBuilder.Entity("CC98.Software.Data.Category", b =>
                {
                    b.HasOne("CC98.Software.Data.Category", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId");
                });

            modelBuilder.Entity("CC98.Software.Data.Software", b =>
                {
                    b.HasOne("CC98.Software.Data.Category", "Class")
                        .WithMany()
                        .HasForeignKey("ClassId");
                });
        }
    }
}
