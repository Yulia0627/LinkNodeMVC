using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LinkNodeInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameAdminToAdminAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.CreateTable(
                name: "AdminActions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    adminId = table.Column<int>(type: "integer", nullable: false),
                    actionId = table.Column<int>(type: "integer", nullable: false),
                    targetUserId = table.Column<int>(type: "integer", nullable: true),
                    targetVacancyId = table.Column<int>(type: "integer", nullable: true),
                    description = table.Column<string>(type: "text", nullable: false),
                    createdDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("AdminActions_pkey", x => x.id);
                    table.ForeignKey(
                        name: "AdminActions_actionId_fkey",
                        column: x => x.actionId,
                        principalTable: "ActionTypes",
                        principalColumn: "actionId");
                    table.ForeignKey(
                        name: "AdminActions_adminId_fkey",
                        column: x => x.adminId,
                        principalTable: "Users",
                        principalColumn: "userId");
                    table.ForeignKey(
                        name: "AdminActions_targetUserId_fkey",
                        column: x => x.targetUserId,
                        principalTable: "Users",
                        principalColumn: "userId");
                    table.ForeignKey(
                        name: "AdminActions_targetVacancyId_fkey",
                        column: x => x.targetVacancyId,
                        principalTable: "Vacancies",
                        principalColumn: "vacancyId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminActions_actionId",
                table: "AdminActions",
                column: "actionId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminActions_adminId",
                table: "AdminActions",
                column: "adminId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminActions_targetUserId",
                table: "AdminActions",
                column: "targetUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminActions_targetVacancyId",
                table: "AdminActions",
                column: "targetVacancyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminActions");

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    adminId = table.Column<int>(type: "integer", nullable: false),
                    actionId = table.Column<int>(type: "integer", nullable: false),
                    targetUserId = table.Column<int>(type: "integer", nullable: true),
                    targetVacancyId = table.Column<int>(type: "integer", nullable: true),
                    createdDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Admins_pkey", x => x.adminId);
                    table.ForeignKey(
                        name: "Admins_actionId_fkey",
                        column: x => x.actionId,
                        principalTable: "ActionTypes",
                        principalColumn: "actionId");
                    table.ForeignKey(
                        name: "Admins_adminId_fkey",
                        column: x => x.adminId,
                        principalTable: "Users",
                        principalColumn: "userId");
                    table.ForeignKey(
                        name: "Admins_targetUserId_fkey",
                        column: x => x.targetUserId,
                        principalTable: "Users",
                        principalColumn: "userId");
                    table.ForeignKey(
                        name: "Admins_targetVacancyId_fkey",
                        column: x => x.targetVacancyId,
                        principalTable: "Vacancies",
                        principalColumn: "vacancyId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_actionId",
                table: "Admins",
                column: "actionId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_targetUserId",
                table: "Admins",
                column: "targetUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_targetVacancyId",
                table: "Admins",
                column: "targetVacancyId");
        }
    }
}
