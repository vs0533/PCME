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
                name: "DutyLevels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DutyLevels", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "PromoteType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoteType", x => x.Id);
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
                name: "SignUp",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    ExamSubjectId = table.Column<int>(nullable: false),
                    SignUpForUnitId = table.Column<int>(nullable: true),
                    StudentId = table.Column<int>(nullable: false),
                    TicketIsCreate = table.Column<bool>(nullable: false),
                    TrainingCenterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignUp", x => x.Id);
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
                name: "StudentStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false, defaultValue: 1),
                    Name = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentStatus", x => x.Id);
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
                name: "TrainingCenter",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(nullable: true),
                    LogName = table.Column<string>(nullable: true),
                    LogPassWord = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OpenTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingCenter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingCenter_OpenType_OpenTypeId",
                        column: x => x.OpenTypeId,
                        principalTable: "OpenType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    CreditHour = table.Column<int>(nullable: false),
                    ExamSubjectStatusId = table.Column<int>(nullable: false),
                    ExamTypeId = table.Column<int>(nullable: false),
                    MSCount = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    OpenTypeId = table.Column<int>(nullable: false),
                    SeriesId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamSubjects_ExamSubjectStatus_ExamSubjectStatusId",
                        column: x => x.ExamSubjectStatusId,
                        principalTable: "ExamSubjectStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamSubjects_ExamType_ExamTypeId",
                        column: x => x.ExamTypeId,
                        principalTable: "ExamType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamSubjects_OpenType_OpenTypeId",
                        column: x => x.OpenTypeId,
                        principalTable: "OpenType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamSubjects_Seriess_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Seriess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    Name = table.Column<string>(maxLength: 60, nullable: false),
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
                    table.ForeignKey(
                        name: "FK_ExamSubjectOpenInfo_TrainingCenter_TrainingCenterId",
                        column: x => x.TrainingCenterId,
                        principalTable: "TrainingCenter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SignUpForUnit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    IsLock = table.Column<bool>(nullable: false),
                    IsPay = table.Column<bool>(nullable: false),
                    TrainingCenterId = table.Column<int>(nullable: false),
                    WorkUnitId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignUpForUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignUpForUnit_TrainingCenter_TrainingCenterId",
                        column: x => x.TrainingCenterId,
                        principalTable: "TrainingCenter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SignUpForUnit_WorkUnit_WorkUnitId",
                        column: x => x.WorkUnitId,
                        principalTable: "WorkUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Address = table.Column<string>(maxLength: 60, nullable: true),
                    BalanceActual = table.Column<decimal>(nullable: false),
                    BalanceVirtual = table.Column<decimal>(nullable: false),
                    BirthDay = table.Column<DateTime>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailIsValid = table.Column<bool>(nullable: false),
                    GraduationSchool = table.Column<string>(maxLength: 60, nullable: true),
                    IDCard = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OfficeName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Photo = table.Column<string>(nullable: true),
                    PhotoIsValid = table.Column<bool>(nullable: false),
                    SexId = table.Column<int>(nullable: false),
                    Specialty = table.Column<string>(nullable: true),
                    StudentStatusId = table.Column<int>(nullable: false),
                    StudentTypeId = table.Column<int>(nullable: false),
                    TicketCtr = table.Column<int>(nullable: false, defaultValue: 0),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_StudentStatus_StudentStatusId",
                        column: x => x.StudentStatusId,
                        principalTable: "StudentStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_StudentType_StudentTypeId",
                        column: x => x.StudentTypeId,
                        principalTable: "StudentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_WorkUnit_WorkUnitId",
                        column: x => x.WorkUnitId,
                        principalTable: "WorkUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkUnitAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccountName = table.Column<string>(nullable: false),
                    HolderName = table.Column<string>(nullable: true),
                    PassWord = table.Column<string>(nullable: false),
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

            migrationBuilder.CreateTable(
                name: "CivilServantInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Duty = table.Column<string>(nullable: true),
                    DutyLevelId = table.Column<int>(nullable: false),
                    EducationFirst = table.Column<string>(nullable: true),
                    EducationHeight = table.Column<string>(nullable: true),
                    JoinPromote = table.Column<bool>(nullable: false),
                    StudentId = table.Column<int>(nullable: false),
                    TakeDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CivilServantInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CivilServantInfos_DutyLevels_DutyLevelId",
                        column: x => x.DutyLevelId,
                        principalTable: "DutyLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CivilServantInfos_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfessionalInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CalculateDate = table.Column<DateTime>(nullable: false),
                    Education = table.Column<string>(nullable: true),
                    GetDate = table.Column<DateTime>(nullable: false),
                    ProfessionalTitleId = table.Column<int>(nullable: false),
                    PromoteTypeId = table.Column<int>(nullable: false),
                    StudentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionalInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfessionalInfos_ProfessionalTitle_ProfessionalTitleId",
                        column: x => x.ProfessionalTitleId,
                        principalTable: "ProfessionalTitle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfessionalInfos_PromoteType_PromoteTypeId",
                        column: x => x.PromoteTypeId,
                        principalTable: "PromoteType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfessionalInfos_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SignUpCollections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ExamSubjectId = table.Column<int>(nullable: false),
                    SignUpForUnitId = table.Column<int>(nullable: false),
                    StudentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignUpCollections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SignUpCollections_ExamSubjects_ExamSubjectId",
                        column: x => x.ExamSubjectId,
                        principalTable: "ExamSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SignUpCollections_SignUpForUnit_SignUpForUnitId",
                        column: x => x.SignUpForUnitId,
                        principalTable: "SignUpForUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SignUpCollections_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CivilServantInfos_DutyLevelId",
                table: "CivilServantInfos",
                column: "DutyLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_CivilServantInfos_StudentId",
                table: "CivilServantInfos",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSubjectOpenInfo_AuditStatusId",
                table: "ExamSubjectOpenInfo",
                column: "AuditStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSubjectOpenInfo_ExamSubjectId",
                table: "ExamSubjectOpenInfo",
                column: "ExamSubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSubjectOpenInfo_TrainingCenterId",
                table: "ExamSubjectOpenInfo",
                column: "TrainingCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSubjects_ExamSubjectStatusId",
                table: "ExamSubjects",
                column: "ExamSubjectStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSubjects_ExamTypeId",
                table: "ExamSubjects",
                column: "ExamTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSubjects_OpenTypeId",
                table: "ExamSubjects",
                column: "OpenTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSubjects_SeriesId",
                table: "ExamSubjects",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalInfos_ProfessionalTitleId",
                table: "ProfessionalInfos",
                column: "ProfessionalTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalInfos_PromoteTypeId",
                table: "ProfessionalInfos",
                column: "PromoteTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfessionalInfos_StudentId",
                table: "ProfessionalInfos",
                column: "StudentId");

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
                name: "IX_SignUpCollections_SignUpForUnitId",
                table: "SignUpCollections",
                column: "SignUpForUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_SignUpCollections_StudentId",
                table: "SignUpCollections",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SignUpCollections_ExamSubjectId_StudentId",
                table: "SignUpCollections",
                columns: new[] { "ExamSubjectId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SignUpForUnit_TrainingCenterId",
                table: "SignUpForUnit",
                column: "TrainingCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_SignUpForUnit_WorkUnitId",
                table: "SignUpForUnit",
                column: "WorkUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_SexId",
                table: "Students",
                column: "SexId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentStatusId",
                table: "Students",
                column: "StudentStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentTypeId",
                table: "Students",
                column: "StudentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_WorkUnitId",
                table: "Students",
                column: "WorkUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCenter_OpenTypeId",
                table: "TrainingCenter",
                column: "OpenTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkUnit_Code",
                table: "WorkUnit",
                column: "Code",
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
                name: "CivilServantInfos");

            migrationBuilder.DropTable(
                name: "ExamSubjectOpenInfo");

            migrationBuilder.DropTable(
                name: "ProfessionalInfos");

            migrationBuilder.DropTable(
                name: "SignUp");

            migrationBuilder.DropTable(
                name: "SignUpCollections");

            migrationBuilder.DropTable(
                name: "WorkUnitAccounts");

            migrationBuilder.DropTable(
                name: "DutyLevels");

            migrationBuilder.DropTable(
                name: "AuditStatus");

            migrationBuilder.DropTable(
                name: "ProfessionalTitle");

            migrationBuilder.DropTable(
                name: "PromoteType");

            migrationBuilder.DropTable(
                name: "ExamSubjects");

            migrationBuilder.DropTable(
                name: "SignUpForUnit");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "WorkUnitAccountType");

            migrationBuilder.DropTable(
                name: "Levels");

            migrationBuilder.DropTable(
                name: "Specialtys");

            migrationBuilder.DropTable(
                name: "ExamSubjectStatus");

            migrationBuilder.DropTable(
                name: "ExamType");

            migrationBuilder.DropTable(
                name: "Seriess");

            migrationBuilder.DropTable(
                name: "TrainingCenter");

            migrationBuilder.DropTable(
                name: "Sex");

            migrationBuilder.DropTable(
                name: "StudentStatus");

            migrationBuilder.DropTable(
                name: "StudentType");

            migrationBuilder.DropTable(
                name: "WorkUnit");

            migrationBuilder.DropTable(
                name: "OpenType");

            migrationBuilder.DropTable(
                name: "WorkUnitNature");
        }
    }
}
