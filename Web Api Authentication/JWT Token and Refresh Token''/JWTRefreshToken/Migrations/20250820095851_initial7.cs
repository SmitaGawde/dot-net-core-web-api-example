using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JWTRefreshToken.Migrations
{
    /// <inheritdoc />
    public partial class initial7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Department_departmentDeptId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_DeptId",
                table: "Employee");

            migrationBuilder.AddColumn<int>(
                name: "departmentDeptId",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Employee_departmentDeptId",
                table: "Employee",
                column: "departmentDeptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Department_departmentDeptId",
                table: "Employee",
                column: "departmentDeptId",
                principalTable: "Department",
                principalColumn: "DeptId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Department_departmentDeptId",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_departmentDeptId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "departmentDeptId",
                table: "Employee");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DeptId",
                table: "Employee",
                column: "DeptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Department_DeptId",
                table: "Employee",
                column: "DeptId",
                principalTable: "Department",
                principalColumn: "DeptId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
