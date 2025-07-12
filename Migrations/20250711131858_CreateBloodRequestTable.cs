using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodDonationBE.Migrations
{
    /// <inheritdoc />
    public partial class CreateBloodRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "blood_requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PatientName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BloodType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    FulfilledAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    HospitalId = table.Column<int>(type: "int", nullable: false),
                    RequestingUserId = table.Column<int>(type: "int", nullable: false),
                    VerifyingStaffId = table.Column<int>(type: "int", nullable: true),
                    ApprovingAdminId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blood_requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_blood_requests_hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_blood_requests_user_ApprovingAdminId",
                        column: x => x.ApprovingAdminId,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_blood_requests_user_RequestingUserId",
                        column: x => x.RequestingUserId,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_blood_requests_user_VerifyingStaffId",
                        column: x => x.VerifyingStaffId,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_blood_requests_ApprovingAdminId",
                table: "blood_requests",
                column: "ApprovingAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_blood_requests_HospitalId",
                table: "blood_requests",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_blood_requests_RequestingUserId",
                table: "blood_requests",
                column: "RequestingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_blood_requests_VerifyingStaffId",
                table: "blood_requests",
                column: "VerifyingStaffId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blood_requests");
        }
    }
}
