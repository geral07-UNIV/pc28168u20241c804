using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace PC28168.U20241C804.Openpay.Platform.Shared.Infrastructure.Persistence.EFC.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "disbursement_orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    order_tracking_code = table.Column<Guid>(type: "char(36)", nullable: false),
                    affiliate_store_id = table.Column<Guid>(type: "char(36)", nullable: false),
                    scheduled_from = table.Column<DateOnly>(type: "date", nullable: false),
                    scheduled_to = table.Column<DateOnly>(type: "date", nullable: false),
                    transfer_cap_amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    transfer_cap_currency_code = table.Column<string>(type: "longtext", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    user = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    remarks = table.Column<string>(type: "varchar(85)", maxLength: 85, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_disbursement_orders", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_disbursement_orders_order_tracking_code",
                table: "disbursement_orders",
                column: "order_tracking_code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "disbursement_orders");
        }
    }
}
