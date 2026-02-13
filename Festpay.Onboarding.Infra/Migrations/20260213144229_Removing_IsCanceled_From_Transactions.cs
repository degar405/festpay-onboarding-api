using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Festpay.Onboarding.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Removing_IsCanceled_From_Transactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Transactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Transactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
