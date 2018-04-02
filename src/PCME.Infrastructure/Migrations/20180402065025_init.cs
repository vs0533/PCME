using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PCME.Infrastructure.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sex",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sex", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnitNature",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitNature", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    Balance = table.Column<decimal>(nullable: false),
                    BirthDay = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailIsValid = table.Column<bool>(nullable: false),
                    GraduationSchool = table.Column<string>(nullable: true),
                    IDCard = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OfficeName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Photo = table.Column<string>(nullable: true),
                    PhotoIsValid = table.Column<bool>(nullable: false),
                    SexId = table.Column<int>(nullable: true),
                    Specialty = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: true),
                    WorkDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Sex_SexId",
                        column: x => x.SexId,
                        principalTable: "Sex",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_StudentType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "StudentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    LinkMan = table.Column<string>(nullable: true),
                    LinkPhoto = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    PID = table.Column<int>(nullable: true),
                    ParentId = table.Column<int>(nullable: true),
                    UnitNatureId = table.Column<int>(nullable: false),
                    Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                    WorkUnitNatureId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Unit_Unit_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Unit_UnitNature_UnitNatureId",
                        column: x => x.UnitNatureId,
                        principalTable: "UnitNature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_SexId",
                table: "Students",
                column: "SexId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_TypeId",
                table: "Students",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_ParentId",
                table: "Unit",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_UnitNatureId",
                table: "Unit",
                column: "UnitNatureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Unit");

            migrationBuilder.DropTable(
                name: "Sex");

            migrationBuilder.DropTable(
                name: "StudentType");

            migrationBuilder.DropTable(
                name: "UnitNature");
        }
    }
}
