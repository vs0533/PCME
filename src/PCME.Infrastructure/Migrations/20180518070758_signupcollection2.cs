using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PCME.Infrastructure.Migrations
{
    public partial class signupcollection2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SignUpCollections_SignUpForUnit_SignUpForUnitId1",
                table: "SignUpCollections");

            migrationBuilder.DropIndex(
                name: "IX_SignUpCollections_SignUpForUnitId1",
                table: "SignUpCollections");

            migrationBuilder.DropColumn(
                name: "SignUpForUnitId1",
                table: "SignUpCollections");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SignUpForUnitId1",
                table: "SignUpCollections",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SignUpCollections_SignUpForUnitId1",
                table: "SignUpCollections",
                column: "SignUpForUnitId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SignUpCollections_SignUpForUnit_SignUpForUnitId1",
                table: "SignUpCollections",
                column: "SignUpForUnitId1",
                principalTable: "SignUpForUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
