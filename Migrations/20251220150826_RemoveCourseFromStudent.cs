using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebMVC.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCourseFromStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Courses_CourseId",
                table: "Students");

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Enrollments",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Courses",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Courses_CourseId",
                table: "Students",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Courses_CourseId",
                table: "Students");

            migrationBuilder.InsertData(
                table: "Courses",
                columns: new[] { "Id", "CourseCode", "CourseName", "Description", "Instructor" },
                values: new object[,]
                {
                    { 1, "CS101", "Pemrograman Dasar", "Mempelajari dasar-dasar pemrograman menggunakan C#", "Prof. Budi Santoso" },
                    { 2, "MTK102", "Matematika Diskrit", "Logika matematika dan struktur diskrit", "Dr. Siti Aminah" },
                    { 3, "CS103", "Struktur Data", "Array, Linked List, Stack, Queue, Tree, Graph", "Prof. Ahmad Dahlan" },
                    { 4, "CS104", "Basis Data", "Desain database, SQL, normalisasi", "Dr. Rina Wijaya" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Address", "CourseId", "Email", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "Jl. Merdeka No. 10, Jakarta", null, "budi.setiawan@mail.com", "Budi Setiawan", "081234567890" },
                    { 2, "Jl. Sudirman No. 25, Bandung", null, "siti.nurhaliza@mail.com", "Siti Nurhaliza", "081234567891" },
                    { 3, "Jl. Diponegoro No. 15, Surabaya", null, "ahmad.yani@mail.com", "Ahmad Yani", "081234567892" },
                    { 4, "Jl. Gatot Subroto No. 30, Yogyakarta", null, "dewi.lestari@mail.com", "Dewi Lestari", "081234567893" }
                });

            migrationBuilder.InsertData(
                table: "Enrollments",
                columns: new[] { "Id", "CourseId", "EnrollmentDate", "Grade", "Status", "StudentId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 1 },
                    { 2, 2, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 1 },
                    { 3, 3, new DateTime(2024, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 1 },
                    { 4, 1, new DateTime(2024, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 2 },
                    { 5, 4, new DateTime(2024, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 2 },
                    { 6, 2, new DateTime(2024, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 3 },
                    { 7, 3, new DateTime(2024, 9, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 3 },
                    { 8, 4, new DateTime(2024, 9, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Active", 4 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Courses_CourseId",
                table: "Students",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
