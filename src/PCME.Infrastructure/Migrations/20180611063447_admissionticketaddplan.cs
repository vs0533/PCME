using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PCME.Infrastructure.Migrations
{
    public partial class admissionticketaddplan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdmissionTickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ExamSubjectId = table.Column<int>(nullable: false),
                    ExaminationRoomId = table.Column<int>(nullable: false),
                    ExaminationRoomPlanId = table.Column<int>(nullable: false),
                    LoginTime = table.Column<DateTime>(nullable: true),
                    Num = table.Column<string>(nullable: true),
                    PostPaperTime = table.Column<DateTime>(nullable: true),
                    SignInTime = table.Column<DateTime>(nullable: true),
                    SignUpId = table.Column<int>(nullable: false),
                    StudentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdmissionTickets", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdmissionTickets");
        }
    }
}
