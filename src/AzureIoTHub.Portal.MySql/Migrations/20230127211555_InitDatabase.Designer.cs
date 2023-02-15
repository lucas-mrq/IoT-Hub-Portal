﻿// <auto-generated />
using System;
using AzureIoTHub.Portal.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AzureIoTHub.Portal.MySql.Migrations
{
    [DbContext(typeof(PortalDbContext))]
    [Migration("20230127211555_InitDatabase")]
    partial class InitDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.Concentrator", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ClientThumbprint")
                        .HasColumnType("longtext");

                    b.Property<string>("DeviceType")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsConnected")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LoraRegion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Concentrators");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.Device", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("DeviceModelId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("IsConnected")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("StatusUpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DeviceModelId");

                    b.ToTable("Devices", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.DeviceModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<bool?>("ABPRelaxMode")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("AppEUI")
                        .HasColumnType("longtext");

                    b.Property<int>("ClassType")
                        .HasColumnType("int");

                    b.Property<int?>("Deduplication")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<bool?>("Downlink")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsBuiltin")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("KeepAliveTimeout")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("PreferredWindow")
                        .HasColumnType("int");

                    b.Property<int?>("RXDelay")
                        .HasColumnType("int");

                    b.Property<string>("SensorDecoder")
                        .HasColumnType("longtext");

                    b.Property<bool>("SupportLoRaFeatures")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("UseOTAA")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("DeviceModels");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.DeviceModelCommand", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("Confirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("DeviceModelId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Frame")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsBuiltin")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Port")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DeviceModelCommands");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.DeviceModelProperty", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsWritable")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("ModelId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<int>("PropertyType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DeviceModelProperties");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.DeviceTag", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("Required")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Searchable")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("DeviceTags");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.DeviceTagValue", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("DeviceId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("EdgeDeviceId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("EdgeDeviceId");

                    b.ToTable("DeviceTagValues");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.EdgeDevice", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ConnectionState")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("DeviceModelId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("NbDevices")
                        .HasColumnType("int");

                    b.Property<int>("NbModules")
                        .HasColumnType("int");

                    b.Property<string>("Scope")
                        .HasColumnType("longtext");

                    b.Property<int>("Version")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DeviceModelId");

                    b.ToTable("EdgeDevices");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.EdgeDeviceModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("EdgeDeviceModels");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.EdgeDeviceModelCommand", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("EdgeDeviceModelId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ModuleName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("EdgeDeviceModelCommands");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.Label", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("DeviceId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("DeviceModelId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("EdgeDeviceId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("EdgeDeviceModelId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("DeviceModelId");

                    b.HasIndex("EdgeDeviceId");

                    b.HasIndex("EdgeDeviceModelId");

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.LoRaDeviceTelemetry", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("EnqueuedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LorawanDeviceId")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Telemetry")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("LorawanDeviceId");

                    b.ToTable("LoRaDeviceTelemetry");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.LorawanDevice", b =>
                {
                    b.HasBaseType("AzureIoTHub.Portal.Domain.Entities.Device");

                    b.Property<bool?>("ABPRelaxMode")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("AlreadyLoggedInOnce")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("AppEUI")
                        .HasColumnType("longtext");

                    b.Property<string>("AppKey")
                        .HasColumnType("longtext");

                    b.Property<string>("AppSKey")
                        .HasColumnType("longtext");

                    b.Property<int>("ClassType")
                        .HasColumnType("int");

                    b.Property<string>("DataRate")
                        .HasColumnType("longtext");

                    b.Property<int>("Deduplication")
                        .HasColumnType("int");

                    b.Property<string>("DevAddr")
                        .HasColumnType("longtext");

                    b.Property<bool?>("Downlink")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("FCntDownStart")
                        .HasColumnType("int");

                    b.Property<int?>("FCntResetCounter")
                        .HasColumnType("int");

                    b.Property<int?>("FCntUpStart")
                        .HasColumnType("int");

                    b.Property<string>("GatewayID")
                        .HasColumnType("longtext");

                    b.Property<int?>("KeepAliveTimeout")
                        .HasColumnType("int");

                    b.Property<string>("NbRep")
                        .HasColumnType("longtext");

                    b.Property<string>("NwkSKey")
                        .HasColumnType("longtext");

                    b.Property<int>("PreferredWindow")
                        .HasColumnType("int");

                    b.Property<int?>("RX1DROffset")
                        .HasColumnType("int");

                    b.Property<int?>("RX2DataRate")
                        .HasColumnType("int");

                    b.Property<int?>("RXDelay")
                        .HasColumnType("int");

                    b.Property<string>("ReportedRX1DROffset")
                        .HasColumnType("longtext");

                    b.Property<string>("ReportedRX2DataRate")
                        .HasColumnType("longtext");

                    b.Property<string>("ReportedRXDelay")
                        .HasColumnType("longtext");

                    b.Property<string>("SensorDecoder")
                        .HasColumnType("longtext");

                    b.Property<bool?>("Supports32BitFCnt")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("TxPower")
                        .HasColumnType("longtext");

                    b.Property<bool>("UseOTAA")
                        .HasColumnType("tinyint(1)");

                    b.ToTable("LorawanDevices", (string)null);
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.Device", b =>
                {
                    b.HasOne("AzureIoTHub.Portal.Domain.Entities.DeviceModel", "DeviceModel")
                        .WithMany()
                        .HasForeignKey("DeviceModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeviceModel");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.DeviceTagValue", b =>
                {
                    b.HasOne("AzureIoTHub.Portal.Domain.Entities.Device", null)
                        .WithMany("Tags")
                        .HasForeignKey("DeviceId");

                    b.HasOne("AzureIoTHub.Portal.Domain.Entities.EdgeDevice", null)
                        .WithMany("Tags")
                        .HasForeignKey("EdgeDeviceId");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.EdgeDevice", b =>
                {
                    b.HasOne("AzureIoTHub.Portal.Domain.Entities.EdgeDeviceModel", "DeviceModel")
                        .WithMany()
                        .HasForeignKey("DeviceModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeviceModel");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.Label", b =>
                {
                    b.HasOne("AzureIoTHub.Portal.Domain.Entities.Device", null)
                        .WithMany("Labels")
                        .HasForeignKey("DeviceId");

                    b.HasOne("AzureIoTHub.Portal.Domain.Entities.DeviceModel", null)
                        .WithMany("Labels")
                        .HasForeignKey("DeviceModelId");

                    b.HasOne("AzureIoTHub.Portal.Domain.Entities.EdgeDevice", null)
                        .WithMany("Labels")
                        .HasForeignKey("EdgeDeviceId");

                    b.HasOne("AzureIoTHub.Portal.Domain.Entities.EdgeDeviceModel", null)
                        .WithMany("Labels")
                        .HasForeignKey("EdgeDeviceModelId");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.LoRaDeviceTelemetry", b =>
                {
                    b.HasOne("AzureIoTHub.Portal.Domain.Entities.LorawanDevice", null)
                        .WithMany("Telemetry")
                        .HasForeignKey("LorawanDeviceId");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.LorawanDevice", b =>
                {
                    b.HasOne("AzureIoTHub.Portal.Domain.Entities.Device", null)
                        .WithOne()
                        .HasForeignKey("AzureIoTHub.Portal.Domain.Entities.LorawanDevice", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.Device", b =>
                {
                    b.Navigation("Labels");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.DeviceModel", b =>
                {
                    b.Navigation("Labels");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.EdgeDevice", b =>
                {
                    b.Navigation("Labels");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.EdgeDeviceModel", b =>
                {
                    b.Navigation("Labels");
                });

            modelBuilder.Entity("AzureIoTHub.Portal.Domain.Entities.LorawanDevice", b =>
                {
                    b.Navigation("Telemetry");
                });
#pragma warning restore 612, 618
        }
    }
}
