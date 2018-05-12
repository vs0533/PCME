using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PCME.Infrastructure.Migrations
{
    public partial class examsubject_examsubjectopeninfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamSubjects_StudentType_OpenTypeId",
                table: "ExamSubjects");

            migrationBuilder.AlterColumn<int>(
                name: "OpenTypeId",
                table: "ExamSubjects",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "ExamSubjects",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreditHour",
                table: "ExamSubjects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExamSubjectStatusId",
                table: "ExamSubjects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExamTypeId",
                table: "ExamSubjects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MSCount",
                table: "ExamSubjects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SeriesId",
                table: "ExamSubjects",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuditStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExamSubjectStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSubjectStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExamType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamSubjects_ExamSubjectStatusId",
                table: "ExamSubjects",
                column: "ExamSubjectStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSubjects_ExamTypeId",
                table: "ExamSubjects",
                column: "ExamTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSubjects_ExamSubjectStatus_ExamSubjectStatusId",
                table: "ExamSubjects",
                column: "ExamSubjectStatusId",
                principalTable: "ExamSubjectStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSubjects_ExamType_ExamTypeId",
                table: "ExamSubjects",
                column: "ExamTypeId",
                principalTable: "ExamType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSubjects_OpenType_OpenTypeId",
                table: "ExamSubjects",
                column: "OpenTypeId",
                principalTable: "OpenType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamSubjects_ExamSubjectStatus_ExamSubjectStatusId",
                table: "ExamSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamSubjects_ExamType_ExamTypeId",
                table: "ExamSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamSubjects_OpenType_OpenTypeId",
                table: "ExamSubjects");

            migrationBuilder.DropTable(
                name: "AuditStatus");

            migrationBuilder.DropTable(
                name: "ExamSubjectStatus");

            migrationBuilder.DropTable(
                name: "ExamType");

            migrationBuilder.DropTable(
                name: "OpenType");

            migrationBuilder.DropIndex(
                name: "IX_ExamSubjects_ExamSubjectStatusId",
                table: "ExamSubjects");

            migrationBuilder.DropIndex(
                name: "IX_ExamSubjects_ExamTypeId",
                table: "ExamSubjects");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "ExamSubjects");

            migrationBuilder.DropColumn(
                name: "CreditHour",
                table: "ExamSubjects");

            migrationBuilder.DropColumn(
                name: "ExamSubjectStatusId",
                table: "ExamSubjects");

            migrationBuilder.DropColumn(
                name: "ExamTypeId",
                table: "ExamSubjects");

            migrationBuilder.DropColumn(
                name: "MSCount",
                table: "ExamSubjects");

            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "ExamSubjects");

            migrationBuilder.AlterColumn<int>(
                name: "OpenTypeId",
                table: "ExamSubjects",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSubjects_StudentType_OpenTypeId",
                table: "ExamSubjects",
                column: "OpenTypeId",
                principalTable: "StudentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
