using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SubscriptionAnalytics.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBaseEntityInheritanceAndStripeConnections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "UserTenants",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "UserTenants",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "UserTenants",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "UserTenants",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "UserTenants",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserTenants",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "UserTenants",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Tenants",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Tenants",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Tenants",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Tenants",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Tenants",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "Tenants",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SyncedCustomers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "SyncedCustomers",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "SyncedCustomers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "SyncedCustomers",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "SyncedCustomers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SyncedCustomers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "SyncedCustomers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "SyncedCustomers",
                type: "character varying(450)",
                maxLength: 450,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "SyncedCustomers");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SyncedCustomers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SyncedCustomers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SyncedCustomers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "SyncedCustomers");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "SyncedCustomers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "SyncedCustomers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SyncedCustomers");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "UserTenants");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "UserTenants");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "UserTenants");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserTenants");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "UserTenants");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "UserTenants");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UserTenants");
        }
    }
}
