using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JWTRefreshToken.Migrations
{
    /// <inheritdoc />
    public partial class initial8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Department_departmentDeptId",
                table: "Employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_departmentDeptId",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "departmentDeptId",
                table: "Employee");

            migrationBuilder.RenameTable(
                name: "Employee",
                newName: "employee");

            migrationBuilder.AddPrimaryKey(
                name: "PK_employee",
                table: "employee",
                column: "EmpID");

            migrationBuilder.CreateIndex(
                name: "IX_employee_DeptId",
                table: "employee",
                column: "DeptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Department",
                table: "employee",
                column: "DeptId",
                principalTable: "Department",
                principalColumn: "DeptId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Department",
                table: "employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_employee",
                table: "employee");

            migrationBuilder.DropIndex(
                name: "IX_employee_DeptId",
                table: "employee");

            migrationBuilder.RenameTable(
                name: "employee",
                newName: "Employee");

            migrationBuilder.AddColumn<int>(
                name: "departmentDeptId",
                table: "Employee",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee",
                table: "Employee",
                column: "EmpID");

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
    }
}
