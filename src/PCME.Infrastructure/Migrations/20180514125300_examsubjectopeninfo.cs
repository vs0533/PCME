using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PCME.Infrastructure.Migrations
{
    public partial class examsubjectopeninfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExamSubjectOpenInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuditStatusId = table.Column<int>(nullable: false),
                    DisplayExamTime = table.Column<string>(nullable: true),
                    ExamSubjectId = table.Column<int>(nullable: false),
                    SignUpFinishOffset = table.Column<int>(nullable: false),
                    SignUpFinishTime = table.Column<DateTime>(nullable: false),
                    SignUpTime = table.Column<DateTime>(nullable: false),
                    TrainingCenterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSubjectOpenInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamSubjectOpenInfo_AuditStatus_AuditStatusId",
                        column: x => x.AuditStatusId,
                        principalTable: "AuditStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamSubjectOpenInfo_ExamSubjects_ExamSubjectId",
                        column: x => x.ExamSubjectId,
                        principalTable: "ExamSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamSubjects_SeriesId",
                table: "ExamSubjects",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSubjectOpenInfo_AuditStatusId",
                table: "ExamSubjectOpenInfo",
                column: "AuditStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSubjectOpenInfo_ExamSubjectId",
                table: "ExamSubjectOpenInfo",
                column: "ExamSubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSubjects_Seriess_SeriesId",
                table: "ExamSubjects",
                column: "SeriesId",
                principalTable: "Seriess",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamSubjects_Seriess_SeriesId",
                table: "ExamSubjects");

            migrationBuilder.DropTable(
                name: "ExamSubjectOpenInfo");

            migrationBuilder.DropIndex(
                name: "IX_ExamSubjects_SeriesId",
                table: "ExamSubjects");
        }
    }
}
