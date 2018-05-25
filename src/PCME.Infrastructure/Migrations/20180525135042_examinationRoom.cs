using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PCME.Infrastructure.Migrations
{
    public partial class examinationRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExaminationRooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Galleryful = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TrainingCenterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExaminationRooms", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SignUp_StudentId_ExamSubjectId",
                table: "SignUp",
                columns: new[] { "StudentId", "ExamSubjectId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExaminationRooms");

            migrationBuilder.DropIndex(
                name: "IX_SignUp_StudentId_ExamSubjectId",
                table: "SignUp");
        }
    }
}
