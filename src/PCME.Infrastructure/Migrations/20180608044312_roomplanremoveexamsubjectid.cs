using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PCME.Infrastructure.Migrations
{
    public partial class roomplanremoveexamsubjectid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamSubjectID",
                table: "ExaminationRoomPlans");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExamSubjectID",
                table: "ExaminationRoomPlans",
                nullable: false,
                defaultValue: 0);
        }
    }
}
