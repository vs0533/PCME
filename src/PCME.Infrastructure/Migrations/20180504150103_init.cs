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
                name: "Levels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Levels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seriess",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seriess", x => x.Id);
                });

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
                name: "Specialtys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialtys", x => x.Id);
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
                name: "WorkUnitAccountType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkUnitAccountType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkUnitNature",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkUnitNature", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfessionalTitle",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LevelId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    SeriesId = table.Column<int>(nullable: true),
                    SpecialtyId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionalTitle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfessionalTitle_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfessionalTitle_Seriess_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Seriess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProfessionalTitle_Specialtys_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalTable: "Specialtys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    ProfessionalTitleId = table.Column<int>(nullable: false),
                    SexId = table.Column<int>(nullable: true),
                    Specialty = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: true),
                    WorkDate = table.Column<DateTime>(nullable: true),
                    WorkUnitId = table.Column<int>(nullable: false)
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
                name: "WorkUnit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Level = table.Column<int>(nullable: false),
                    LinkMan = table.Column<string>(nullable: true),
                    LinkPhone = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    PID = table.Column<int>(nullable: true),
                    Version = table.Column<byte[]>(rowVersion: true, nullable: true),
                    WorkUnitNatureId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkUnit_WorkUnit_PID",
                        column: x => x.PID,
                        principalTable: "WorkUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkUnit_WorkUnitNature_WorkUnitNatureId",
                        column: x => x.WorkUnitNatureId,
                        principalTable: "WorkUnitNature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkUnitAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountName = table.Column<string>(nullable: true),
                    HolderName = table.Column<string>(nullable: true),
                    PassWord = table.Column<string>(nullable: true),
                    WorkUnitAccountTypeId = table.Column<int>(nullable: false),
                    WorkUnitId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkUnitAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkUnitAccounts_WorkUnitAccountType_WorkUnitAccountTypeId",
                        column: x => x.WorkUnitAccountTypeId,
                        principalTable: "WorkUnitAccountType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkUnitAccounts_WorkUnit_WorkUnitId",
                        column: x => x.WorkUnitId,
                        principalTable: "WorkUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalTitle_LevelId",
                table: "ProfessionalTitle",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalTitle_SeriesId",
                table: "ProfessionalTitle",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalTitle_SpecialtyId",
                table: "ProfessionalTitle",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_SexId",
                table: "Students",
                column: "SexId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_TypeId",
                table: "Students",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkUnit_Code",
                table: "WorkUnit",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkUnit_Name",
                table: "WorkUnit",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkUnit_PID",
                table: "WorkUnit",
                column: "PID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkUnit_WorkUnitNatureId",
                table: "WorkUnit",
                column: "WorkUnitNatureId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkUnitAccounts_WorkUnitAccountTypeId",
                table: "WorkUnitAccounts",
                column: "WorkUnitAccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkUnitAccounts_WorkUnitId",
                table: "WorkUnitAccounts",
                column: "WorkUnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfessionalTitle");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "WorkUnitAccounts");

            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.DropTable(
                name: "Seriess");

            migrationBuilder.DropTable(
                name: "Specialtys");

            migrationBuilder.DropTable(
                name: "Sex");

            migrationBuilder.DropTable(
                name: "StudentType");

            migrationBuilder.DropTable(
                name: "WorkUnitAccountType");

            migrationBuilder.DropTable(
                name: "WorkUnit");

            migrationBuilder.DropTable(
                name: "WorkUnitNature");
        }
    }
}
