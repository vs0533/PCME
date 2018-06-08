using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PCME.Infrastructure.Migrations
{
    public partial class roomplanaddgalleryful : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Galleryful",
                table: "ExaminationRoomPlans",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Galleryful",
                table: "ExaminationRoomPlans");
        }
    }
}
