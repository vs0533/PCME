using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PCME.Infrastructure.Migrations
{
    public partial class signupcollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignUpCollections_SignUpForUnit_SignUpForUnitId",
                table: "SignUpCollections");

            migrationBuilder.AlterColumn<int>(
                name: "SignUpForUnitId",
                table: "SignUpCollections",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SignUpForUnitId1",
                table: "SignUpCollections",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SignUpCollections_ExamSubjectId",
                table: "SignUpCollections",
                column: "ExamSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SignUpCollections_SignUpForUnitId1",
                table: "SignUpCollections",
                column: "SignUpForUnitId1");

            migrationBuilder.CreateIndex(
                name: "IX_SignUpCollections_StudentId",
                table: "SignUpCollections",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SignUpCollections_ExamSubjects_ExamSubjectId",
                table: "SignUpCollections",
                column: "ExamSubjectId",
                principalTable: "ExamSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SignUpCollections_SignUpForUnit_SignUpForUnitId",
                table: "SignUpCollections",
                column: "SignUpForUnitId",
                principalTable: "SignUpForUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SignUpCollections_SignUpForUnit_SignUpForUnitId1",
                table: "SignUpCollections",
                column: "SignUpForUnitId1",
                principalTable: "SignUpForUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SignUpCollections_Students_StudentId",
                table: "SignUpCollections",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignUpCollections_ExamSubjects_ExamSubjectId",
                table: "SignUpCollections");

            migrationBuilder.DropForeignKey(
                name: "FK_SignUpCollections_SignUpForUnit_SignUpForUnitId",
                table: "SignUpCollections");

            migrationBuilder.DropForeignKey(
                name: "FK_SignUpCollections_SignUpForUnit_SignUpForUnitId1",
                table: "SignUpCollections");

            migrationBuilder.DropForeignKey(
                name: "FK_SignUpCollections_Students_StudentId",
                table: "SignUpCollections");

            migrationBuilder.DropIndex(
                name: "IX_SignUpCollections_ExamSubjectId",
                table: "SignUpCollections");

            migrationBuilder.DropIndex(
                name: "IX_SignUpCollections_SignUpForUnitId1",
                table: "SignUpCollections");

            migrationBuilder.DropIndex(
                name: "IX_SignUpCollections_StudentId",
                table: "SignUpCollections");

            migrationBuilder.DropColumn(
                name: "SignUpForUnitId1",
                table: "SignUpCollections");

            migrationBuilder.AlterColumn<int>(
                name: "SignUpForUnitId",
                table: "SignUpCollections",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_SignUpCollections_SignUpForUnit_SignUpForUnitId",
                table: "SignUpCollections",
                column: "SignUpForUnitId",
                principalTable: "SignUpForUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
