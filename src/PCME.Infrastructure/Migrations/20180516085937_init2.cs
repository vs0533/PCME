using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PCME.Infrastructure.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TicketCtr",
                table: "Students",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SignUp",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ExamSubjectId = table.Column<int>(nullable: false),
                    SignUpForUnitId = table.Column<int>(nullable: true),
                    StudentId = table.Column<int>(nullable: false),
                    TicketIsCreate = table.Column<bool>(nullable: false),
                    TrainingCenterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignUp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SignUpForUnit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    IsLock = table.Column<bool>(nullable: false),
                    IsPay = table.Column<bool>(nullable: false),
                    TrainingCenterId = table.Column<int>(nullable: false),
                    WorkUnitId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignUpForUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignUpForUnit_TrainingCenter_TrainingCenterId",
                        column: x => x.TrainingCenterId,
                        principalTable: "TrainingCenter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SignUpForUnit_WorkUnit_WorkUnitId",
                        column: x => x.WorkUnitId,
                        principalTable: "WorkUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SignUpCollections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExamSubjectId = table.Column<int>(nullable: false),
                    SignUpForUnitId = table.Column<int>(nullable: true),
                    StudentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignUpCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignUpCollections_SignUpForUnit_SignUpForUnitId",
                        column: x => x.SignUpForUnitId,
                        principalTable: "SignUpForUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SignUpCollections_SignUpForUnitId",
                table: "SignUpCollections",
                column: "SignUpForUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_SignUpForUnit_TrainingCenterId",
                table: "SignUpForUnit",
                column: "TrainingCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_SignUpForUnit_WorkUnitId",
                table: "SignUpForUnit",
                column: "WorkUnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SignUp");

            migrationBuilder.DropTable(
                name: "SignUpCollections");

            migrationBuilder.DropTable(
                name: "SignUpForUnit");

            migrationBuilder.DropColumn(
                name: "TicketCtr",
                table: "Students");
        }
    }
}
