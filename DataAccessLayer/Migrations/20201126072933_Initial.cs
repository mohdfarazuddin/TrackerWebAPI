using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_DiseaseTypes",
                columns: table => new
                {
                    DiseaseTypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiseaseType = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_DiseaseTypes", x => x.DiseaseTypeID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_HospitalDetails",
                columns: table => new
                {
                    HospitalID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_HospitalDetails", x => x.HospitalID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_PatientDetails",
                columns: table => new
                {
                    UniqueID = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    Age = table.Column<byte>(type: "tinyint", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_PatientDetails", x => x.UniqueID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_StateNames",
                columns: table => new
                {
                    StateID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_StateNames", x => x.StateID);
                });

            migrationBuilder.CreateTable(
                name: "tbl_OccupationDetails",
                columns: table => new
                {
                    OccupationID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueID = table.Column<string>(nullable: false),
                    OccupationType = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_OccupationDetails", x => x.OccupationID);
                    table.ForeignKey(
                        name: "FK_tbl_OccupationDetails_tbl_PatientDetails_UniqueID",
                        column: x => x.UniqueID,
                        principalTable: "tbl_PatientDetails",
                        principalColumn: "UniqueID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tbl_TreatmentDetails",
                columns: table => new
                {
                    TreatmentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueID = table.Column<string>(nullable: false),
                    DiseaseTypeID = table.Column<int>(nullable: false),
                    DiseaseName = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    HospitalID = table.Column<int>(nullable: false),
                    AdmitDate = table.Column<DateTime>(type: "Date", nullable: false),
                    DischargeDate = table.Column<DateTime>(type: "Date", nullable: false),
                    Prescription = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CurrentStatus = table.Column<string>(type: "nvarchar(30)", nullable: false),
                    IsFatality = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_TreatmentDetails", x => x.TreatmentID);
                    table.ForeignKey(
                        name: "FK_tbl_TreatmentDetails_tbl_DiseaseTypes_DiseaseTypeID",
                        column: x => x.DiseaseTypeID,
                        principalTable: "tbl_DiseaseTypes",
                        principalColumn: "DiseaseTypeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_TreatmentDetails_tbl_HospitalDetails_HospitalID",
                        column: x => x.HospitalID,
                        principalTable: "tbl_HospitalDetails",
                        principalColumn: "HospitalID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_TreatmentDetails_tbl_PatientDetails_UniqueID",
                        column: x => x.UniqueID,
                        principalTable: "tbl_PatientDetails",
                        principalColumn: "UniqueID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Address",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueID = table.Column<string>(nullable: true),
                    OccupationID = table.Column<int>(nullable: true),
                    HospitalID = table.Column<int>(nullable: true),
                    AddressType = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Addressline = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    StateID = table.Column<int>(nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Address", x => x.ID);
                    table.ForeignKey(
                        name: "FK_tbl_Address_tbl_HospitalDetails_HospitalID",
                        column: x => x.HospitalID,
                        principalTable: "tbl_HospitalDetails",
                        principalColumn: "HospitalID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_Address_tbl_OccupationDetails_OccupationID",
                        column: x => x.OccupationID,
                        principalTable: "tbl_OccupationDetails",
                        principalColumn: "OccupationID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_Address_tbl_StateNames_StateID",
                        column: x => x.StateID,
                        principalTable: "tbl_StateNames",
                        principalColumn: "StateID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tbl_Address_tbl_PatientDetails_UniqueID",
                        column: x => x.UniqueID,
                        principalTable: "tbl_PatientDetails",
                        principalColumn: "UniqueID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Address_HospitalID",
                table: "tbl_Address",
                column: "HospitalID",
                unique: true,
                filter: "[HospitalID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Address_OccupationID",
                table: "tbl_Address",
                column: "OccupationID",
                unique: true,
                filter: "[OccupationID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Address_StateID",
                table: "tbl_Address",
                column: "StateID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Address_UniqueID",
                table: "tbl_Address",
                column: "UniqueID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_DiseaseTypes_DiseaseType",
                table: "tbl_DiseaseTypes",
                column: "DiseaseType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_OccupationDetails_UniqueID",
                table: "tbl_OccupationDetails",
                column: "UniqueID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_StateNames_State",
                table: "tbl_StateNames",
                column: "State",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tbl_TreatmentDetails_DiseaseTypeID",
                table: "tbl_TreatmentDetails",
                column: "DiseaseTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_TreatmentDetails_HospitalID",
                table: "tbl_TreatmentDetails",
                column: "HospitalID");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_TreatmentDetails_UniqueID",
                table: "tbl_TreatmentDetails",
                column: "UniqueID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Address");

            migrationBuilder.DropTable(
                name: "tbl_TreatmentDetails");

            migrationBuilder.DropTable(
                name: "tbl_OccupationDetails");

            migrationBuilder.DropTable(
                name: "tbl_StateNames");

            migrationBuilder.DropTable(
                name: "tbl_DiseaseTypes");

            migrationBuilder.DropTable(
                name: "tbl_HospitalDetails");

            migrationBuilder.DropTable(
                name: "tbl_PatientDetails");
        }
    }
}
