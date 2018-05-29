using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PCME.Infrastructure.Migrations
{
    public partial class roomplan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExaminationRoomPlans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuditStatusId = table.Column<int>(nullable: false),
                    ExamStartTime = table.Column<DateTime>(nullable: false),
                    ExamSubjectID = table.Column<int>(nullable: false),
                    ExamTime = table.Column<DateTime>(nullable: false),
                    ExaminationRoomId = table.Column<int>(nullable: false),
                    Num = table.Column<int>(nullable: false),
                    PlanStatusId = table.Column<int>(nullable: false),
                    SelectFinishTime = table.Column<DateTime>(nullable: false),
                    SelectTime = table.Column<DateTime>(nullable: false),
                    SignInOffset = table.Column<int>(nullable: false),
                    SignInTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExaminationRoomPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanStatus", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExaminationRoomPlans");

            migrationBuilder.DropTable(
                name: "PlanStatus");
        }
    }
}
