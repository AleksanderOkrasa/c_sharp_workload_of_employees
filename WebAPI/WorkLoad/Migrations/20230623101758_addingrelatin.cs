using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkLoad.Migrations
{
    /// <inheritdoc />
    public partial class addingrelatin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Tasks_employeeID",
                table: "Tasks",
                column: "employeeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Employees_employeeID",
                table: "Tasks",
                column: "employeeID",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Employees_employeeID",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_employeeID",
                table: "Tasks");
        }
    }
}
