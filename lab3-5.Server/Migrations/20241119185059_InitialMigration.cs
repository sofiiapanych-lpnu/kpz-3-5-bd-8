using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace lab3_5.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "delivery_address",
                columns: table => new
                {
                    delivery_address_id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('address_address_id_seq'::regclass)"),
                    street = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    building_number = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    apartment_number = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    floor = table.Column<int>(type: "integer", nullable: true),
                    city = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("address_pkey", x => x.delivery_address_id);
                });

            migrationBuilder.CreateTable(
                name: "person",
                columns: table => new
                {
                    person_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("person_pkey", x => x.person_id);
                });

            migrationBuilder.CreateTable(
                name: "transport",
                columns: table => new
                {
                    licence_plate = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    transport_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    model = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("transport_pkey", x => x.licence_plate);
                });

            migrationBuilder.CreateTable(
                name: "warehouse",
                columns: table => new
                {
                    warehouse_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    contact_number = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("warehouse_pkey", x => x.warehouse_id);
                });

            migrationBuilder.CreateTable(
                name: "client",
                columns: table => new
                {
                    person_id = table.Column<int>(type: "integer", nullable: false),
                    address_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("client_pkey", x => x.person_id);
                    table.ForeignKey(
                        name: "client_address_id_fkey",
                        column: x => x.address_id,
                        principalTable: "delivery_address",
                        principalColumn: "delivery_address_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "client_person_id_fkey",
                        column: x => x.person_id,
                        principalTable: "person",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "courier",
                columns: table => new
                {
                    person_id = table.Column<int>(type: "integer", nullable: false),
                    licence_plate = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true),
                    total_deliveries = table.Column<int>(type: "integer", nullable: true, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("courier_pkey", x => x.person_id);
                    table.ForeignKey(
                        name: "courier_licence_plate_fkey",
                        column: x => x.licence_plate,
                        principalTable: "transport",
                        principalColumn: "licence_plate",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "courier_person_id_fkey",
                        column: x => x.person_id,
                        principalTable: "person",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    order_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cost = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Order_pkey", x => x.order_id);
                    table.ForeignKey(
                        name: "fk_client_id",
                        column: x => x.client_id,
                        principalTable: "client",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "delivery",
                columns: table => new
                {
                    delivery_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<int>(type: "integer", nullable: true),
                    courier_id = table.Column<int>(type: "integer", nullable: true),
                    start_time = table.Column<DateTime>(type: "timestamp(0) without time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    desired_duration = table.Column<TimeSpan>(type: "interval", nullable: true),
                    actual_duration = table.Column<TimeSpan>(type: "interval", nullable: true),
                    warehouse_id = table.Column<int>(type: "integer", nullable: true),
                    address_id = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true, defaultValueSql: "'pending'::character varying")
                },
                constraints: table =>
                {
                    table.PrimaryKey("delivery_pkey", x => x.delivery_id);
                    table.ForeignKey(
                        name: "delivery_address_id_fkey",
                        column: x => x.address_id,
                        principalTable: "delivery_address",
                        principalColumn: "delivery_address_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "delivery_courier_id_fkey",
                        column: x => x.courier_id,
                        principalTable: "courier",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "delivery_order_id_fkey",
                        column: x => x.order_id,
                        principalTable: "Order",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "delivery_warehouse_id_fkey",
                        column: x => x.warehouse_id,
                        principalTable: "warehouse",
                        principalColumn: "warehouse_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_client_address_id",
                table: "client",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "unique_licence_plate",
                table: "courier",
                column: "licence_plate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_delivery_address_id",
                table: "delivery",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "IX_delivery_courier_id",
                table: "delivery",
                column: "courier_id");

            migrationBuilder.CreateIndex(
                name: "IX_delivery_order_id",
                table: "delivery",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_delivery_warehouse_id",
                table: "delivery",
                column: "warehouse_id");

            migrationBuilder.CreateIndex(
                name: "IX_Order_client_id",
                table: "Order",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "unique_phone_number",
                table: "person",
                column: "phone_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "delivery");

            migrationBuilder.DropTable(
                name: "courier");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "warehouse");

            migrationBuilder.DropTable(
                name: "transport");

            migrationBuilder.DropTable(
                name: "client");

            migrationBuilder.DropTable(
                name: "delivery_address");

            migrationBuilder.DropTable(
                name: "person");
        }
    }
}
