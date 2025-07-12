using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodDonationBE.Migrations
{
    /// <inheritdoc />
    public partial class AddCampaignAndRelatedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hospitals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    contact_info = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hospitals", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "blood_donation_campaign",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ActiveTime = table.Column<DateTime>(type: "date", nullable: false),
                    DonateTime = table.Column<string>(type: "json", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Max = table.Column<int>(type: "int", nullable: false),
                    lat = table.Column<decimal>(type: "decimal(10,8)", nullable: true),
                    lng = table.Column<decimal>(type: "decimal(11,8)", nullable: true),
                    HospitalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blood_donation_campaign", x => x.Id);
                    table.ForeignKey(
                        name: "FK_blood_donation_campaign_hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "campaign_registration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Volume = table.Column<int>(type: "int", nullable: true),
                    RegisteredAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CampaignId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_campaign_registration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_campaign_registration_blood_donation_campaign_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "blood_donation_campaign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_campaign_registration_user_UserId",
                        column: x => x.UserId,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "blood_units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    blood_type = table.Column<int>(type: "int", nullable: false),
                    Volume = table.Column<int>(type: "int", nullable: false),
                    DonationDate = table.Column<DateTime>(type: "date", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "date", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    product_type = table.Column<int>(type: "int", nullable: false),
                    VerificationDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IssueDate = table.Column<DateTime>(type: "date", nullable: true),
                    RegistrationId = table.Column<int>(type: "int", nullable: true),
                    HospitalId = table.Column<int>(type: "int", nullable: false),
                    DonorId = table.Column<int>(type: "int", nullable: true),
                    VerifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_blood_units", x => x.Id);
                    table.ForeignKey(
                        name: "FK_blood_units_campaign_registration_RegistrationId",
                        column: x => x.RegistrationId,
                        principalTable: "campaign_registration",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_blood_units_hospitals_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "hospitals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_blood_units_user_DonorId",
                        column: x => x.DonorId,
                        principalTable: "user",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK_blood_units_user_VerifiedByUserId",
                        column: x => x.VerifiedByUserId,
                        principalTable: "user",
                        principalColumn: "user_id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_blood_donation_campaign_HospitalId",
                table: "blood_donation_campaign",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_blood_units_DonorId",
                table: "blood_units",
                column: "DonorId");

            migrationBuilder.CreateIndex(
                name: "IX_blood_units_HospitalId",
                table: "blood_units",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_blood_units_RegistrationId",
                table: "blood_units",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_blood_units_VerifiedByUserId",
                table: "blood_units",
                column: "VerifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_campaign_registration_CampaignId",
                table: "campaign_registration",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_campaign_registration_UserId",
                table: "campaign_registration",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_hospitals_Name",
                table: "hospitals",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blood_units");

            migrationBuilder.DropTable(
                name: "campaign_registration");

            migrationBuilder.DropTable(
                name: "blood_donation_campaign");

            migrationBuilder.DropTable(
                name: "hospitals");
        }
    }
}
