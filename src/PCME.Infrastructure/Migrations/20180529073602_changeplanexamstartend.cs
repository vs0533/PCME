using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PCME.Infrastructure.Migrations
{
    public partial class changeplanexamstartend : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExamTime",
                table: "ExaminationRoomPlans",
                newName: "ExamEndTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExamEndTime",
                table: "ExaminationRoomPlans",
                newName: "ExamTime");
        }
    }
}
