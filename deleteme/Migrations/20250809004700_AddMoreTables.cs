using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace deleteme.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Bicycles_BicycleId",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Clients_ClientId",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Employee_EmployeeId",
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetail_Invoice_InvoiceId",
                table: "InvoiceDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetail_ProductOrService_ProductOrServiceId",
                table: "InvoiceDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductOrService",
                table: "ProductOrService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceDetail",
                table: "InvoiceDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoice",
                table: "Invoice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee",
                table: "Employee");

            migrationBuilder.RenameTable(
                name: "ProductOrService",
                newName: "ProductsOrServices");

            migrationBuilder.RenameTable(
                name: "InvoiceDetail",
                newName: "InvoiceDetails");

            migrationBuilder.RenameTable(
                name: "Invoice",
                newName: "Invoices");

            migrationBuilder.RenameTable(
                name: "Employee",
                newName: "Employees");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceDetail_ProductOrServiceId",
                table: "InvoiceDetails",
                newName: "IX_InvoiceDetails_ProductOrServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceDetail_InvoiceId",
                table: "InvoiceDetails",
                newName: "IX_InvoiceDetails_InvoiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoice_EmployeeId",
                table: "Invoices",
                newName: "IX_Invoices_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoice_ClientId",
                table: "Invoices",
                newName: "IX_Invoices_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoice_BicycleId",
                table: "Invoices",
                newName: "IX_Invoices_BicycleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsOrServices",
                table: "ProductsOrServices",
                column: "ProductOrServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceDetails",
                table: "InvoiceDetails",
                column: "InvoiceDetailId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices",
                column: "InvoiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_Invoices_InvoiceId",
                table: "InvoiceDetails",
                column: "InvoiceId",
                principalTable: "Invoices",
                principalColumn: "InvoiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetails_ProductsOrServices_ProductOrServiceId",
                table: "InvoiceDetails",
                column: "ProductOrServiceId",
                principalTable: "ProductsOrServices",
                principalColumn: "ProductOrServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Bicycles_BicycleId",
                table: "Invoices",
                column: "BicycleId",
                principalTable: "Bicycles",
                principalColumn: "BicycleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Clients_ClientId",
                table: "Invoices",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Employees_EmployeeId",
                table: "Invoices",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_Invoices_InvoiceId",
                table: "InvoiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetails_ProductsOrServices_ProductOrServiceId",
                table: "InvoiceDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Bicycles_BicycleId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Clients_ClientId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Employees_EmployeeId",
                table: "Invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsOrServices",
                table: "ProductsOrServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Invoices",
                table: "Invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InvoiceDetails",
                table: "InvoiceDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.RenameTable(
                name: "ProductsOrServices",
                newName: "ProductOrService");

            migrationBuilder.RenameTable(
                name: "Invoices",
                newName: "Invoice");

            migrationBuilder.RenameTable(
                name: "InvoiceDetails",
                newName: "InvoiceDetail");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employee");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_EmployeeId",
                table: "Invoice",
                newName: "IX_Invoice_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_ClientId",
                table: "Invoice",
                newName: "IX_Invoice_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Invoices_BicycleId",
                table: "Invoice",
                newName: "IX_Invoice_BicycleId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceDetails_ProductOrServiceId",
                table: "InvoiceDetail",
                newName: "IX_InvoiceDetail_ProductOrServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceDetails_InvoiceId",
                table: "InvoiceDetail",
                newName: "IX_InvoiceDetail_InvoiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductOrService",
                table: "ProductOrService",
                column: "ProductOrServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Invoice",
                table: "Invoice",
                column: "InvoiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InvoiceDetail",
                table: "InvoiceDetail",
                column: "InvoiceDetailId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee",
                table: "Employee",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Bicycles_BicycleId",
                table: "Invoice",
                column: "BicycleId",
                principalTable: "Bicycles",
                principalColumn: "BicycleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Clients_ClientId",
                table: "Invoice",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Employee_EmployeeId",
                table: "Invoice",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetail_Invoice_InvoiceId",
                table: "InvoiceDetail",
                column: "InvoiceId",
                principalTable: "Invoice",
                principalColumn: "InvoiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetail_ProductOrService_ProductOrServiceId",
                table: "InvoiceDetail",
                column: "ProductOrServiceId",
                principalTable: "ProductOrService",
                principalColumn: "ProductOrServiceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
