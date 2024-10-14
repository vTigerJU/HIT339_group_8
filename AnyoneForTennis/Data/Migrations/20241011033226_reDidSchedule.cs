using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AnyoneForTennis.Data.Migrations
{
    /// <inheritdoc />
    public partial class reDidSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserSchedule");

            migrationBuilder.CreateTable(
                name: "ApplicationUserNewSchedule",
                columns: table => new
                {
                    MembersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SchedulesScheduleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserNewSchedule", x => new { x.MembersId, x.SchedulesScheduleId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserNewSchedule_AspNetUsers_MembersId",
                        column: x => x.MembersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserNewSchedule_Schedules_SchedulesScheduleId",
                        column: x => x.SchedulesScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserNewSchedule_SchedulesScheduleId",
                table: "ApplicationUserNewSchedule",
                column: "SchedulesScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserNewSchedule");

            migrationBuilder.CreateTable(
                name: "ApplicationUserSchedule",
                columns: table => new
                {
                    MembersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SchedulesScheduleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserSchedule", x => new { x.MembersId, x.SchedulesScheduleId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserSchedule_AspNetUsers_MembersId",
                        column: x => x.MembersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserSchedule_Schedules_SchedulesScheduleId",
                        column: x => x.SchedulesScheduleId,
                        principalTable: "Schedules",
                        principalColumn: "ScheduleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserSchedule_SchedulesScheduleId",
                table: "ApplicationUserSchedule",
                column: "SchedulesScheduleId");
        }
    }
}
