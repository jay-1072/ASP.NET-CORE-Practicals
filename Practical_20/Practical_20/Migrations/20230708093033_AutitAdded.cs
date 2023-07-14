using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Practical_20.Migrations
{
	/// <inheritdoc />
	public partial class AutitAdded : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "AuditEntry",
				columns: table => new
				{
					Id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					EntityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
					ActionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
					TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
					EntityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Changes = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AuditEntry", x => x.Id);
				});
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "AuditEntry");
		}
	}
}
