using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestProjectMTech.Migrations
{
    /// <inheritdoc />
    public partial class ResetIdentitySequences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                SELECT setval(pg_get_serial_sequence('categories', 'Id'), COALESCE((SELECT MAX("Id") FROM categories), 1), true);
                SELECT setval(pg_get_serial_sequence('products', 'Id'), COALESCE((SELECT MAX("Id") FROM products), 1), true);
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                SELECT setval(pg_get_serial_sequence('categories', 'Id'), 1, false);
                SELECT setval(pg_get_serial_sequence('products', 'Id'), 1, false);
                """);
        }
    }
}
