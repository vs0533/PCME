using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PCME.Infrastructure.Migrations
{
    public partial class trainingcenteropentypeid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OpenTypeId",
                table: "TrainingCenter",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCenter_OpenTypeId",
                table: "TrainingCenter",
                column: "OpenTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSubjectOpenInfo_TrainingCenterId",
                table: "ExamSubjectOpenInfo",
                column: "TrainingCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSubjectOpenInfo_TrainingCenter_TrainingCenterId",
                table: "ExamSubjectOpenInfo",
                column: "TrainingCenterId",
                principalTable: "TrainingCenter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingCenter_OpenType_OpenTypeId",
                table: "TrainingCenter",
                column: "OpenTypeId",
                principalTable: "OpenType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamSubjectOpenInfo_TrainingCenter_TrainingCenterId",
                table: "ExamSubjectOpenInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingCenter_OpenType_OpenTypeId",
                table: "TrainingCenter");

            migrationBuilder.DropIndex(
                name: "IX_TrainingCenter_OpenTypeId",
                table: "TrainingCenter");

            migrationBuilder.DropIndex(
                name: "IX_ExamSubjectOpenInfo_TrainingCenterId",
                table: "ExamSubjectOpenInfo");

            migrationBuilder.DropColumn(
                name: "OpenTypeId",
                table: "TrainingCenter");
        }
    }
}
