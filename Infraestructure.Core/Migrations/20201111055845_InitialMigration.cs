using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infraestructure.Core.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "General");

            migrationBuilder.EnsureSchema(
                name: "InputsOutputs");

            migrationBuilder.EnsureSchema(
                name: "Vehicle");

            migrationBuilder.CreateTable(
                name: "Amount",
                schema: "General",
                columns: table => new
                {
                    AmountId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    CreationUser = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ModificationUser = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    VehicleType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Amount", x => x.AmountId);
                });

            migrationBuilder.CreateTable(
                name: "ControlInputsOutputs",
                schema: "InputsOutputs",
                columns: table => new
                {
                    ControlInputsOutputsId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    CreationUser = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ModificationUser = table.Column<string>(nullable: true),
                    VehiclePlate = table.Column<string>(type: "varchar(6)", nullable: true),
                    VehicleType = table.Column<int>(type: "int", nullable: false),
                    EntryTime = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "datetime2(7)", nullable: true),
                    TotalMinutesOfStay = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlInputsOutputs", x => x.ControlInputsOutputsId);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                schema: "Vehicle",
                columns: table => new
                {
                    VehicleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    CreationUser = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ModificationUser = table.Column<string>(nullable: true),
                    VehiclePlate = table.Column<string>(type: "varchar(6)", nullable: true),
                    VehicleType = table.Column<int>(type: "int", nullable: false),
                    CutoffDate = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    TotalMinutesOfStay = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.VehicleId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentVehicle",
                schema: "Vehicle",
                columns: table => new
                {
                    PaymentVehicleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationDate = table.Column<DateTime>(nullable: true),
                    CreationUser = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ModificationUser = table.Column<string>(nullable: true),
                    CutoffDate = table.Column<DateTime>(type: "datetime2(7)", nullable: false),
                    TotalMinutesOfStay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentVehicle", x => x.PaymentVehicleId);
                    table.ForeignKey(
                        name: "FK_PaymentVehicle_Vehicle_VehicleId",
                        column: x => x.VehicleId,
                        principalSchema: "Vehicle",
                        principalTable: "Vehicle",
                        principalColumn: "VehicleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVehicle_VehicleId",
                schema: "Vehicle",
                table: "PaymentVehicle",
                column: "VehicleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Amount",
                schema: "General");

            migrationBuilder.DropTable(
                name: "ControlInputsOutputs",
                schema: "InputsOutputs");

            migrationBuilder.DropTable(
                name: "PaymentVehicle",
                schema: "Vehicle");

            migrationBuilder.DropTable(
                name: "Vehicle",
                schema: "Vehicle");
        }
    }
}
