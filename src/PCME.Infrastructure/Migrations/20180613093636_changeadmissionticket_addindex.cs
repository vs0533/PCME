using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PCME.Infrastructure.Migrations
{
    public partial class changeadmissionticket_addindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Num",
                table: "AdmissionTickets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AdmissionTicketLogs",
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
                    table.PrimaryKey("PK_AdmissionTicketLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionTickets_Num",
                table: "AdmissionTickets",
                column: "Num",
                unique: true,
                filter: "[Num] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AdmissionTickets_ExamSubjectId_StudentId",
                table: "AdmissionTickets",
                columns: new[] { "ExamSubjectId", "StudentId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdmissionTicketLogs");

            migrationBuilder.DropIndex(
                name: "IX_AdmissionTickets_Num",
                table: "AdmissionTickets");

            migrationBuilder.DropIndex(
                name: "IX_AdmissionTickets_ExamSubjectId_StudentId",
                table: "AdmissionTickets");

            migrationBuilder.AlterColumn<string>(
                name: "Num",
                table: "AdmissionTickets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
