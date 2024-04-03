using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using static System.Runtime.InteropServices.JavaScript.JSType;

#nullable disable

namespace PlainBlog.Store.PostgreSQL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    surname = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    author_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Posts_Authors_author_id",
                        column: x => x.author_id,
                        principalTable: "Authors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
            name: "IX_Posts_author_id",
            table: "Posts",
                column: "author_id");

            // Seeding data for the 'YourEntities' table
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "id", "name", "surname" }, // Columns you want to insert data into
                values: new object[,]
                {
                { 1, "Name 1", "Surname1" },
                { 2, "Name 2", "Surname2" },
                                   // Add more data as needed
                });

            // Seeding data for another table 'AnotherEntities', if needed
            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "id", "author_id", "title", "description", "content" }, // Columns you want to insert data into
                values: new object[,]
                {
                { 1, 1,"Title1", "Descriptionn1","content1" },
                { 2, 1,"Title2", "Descriptionn2","content2" },
                { 3, 1,"Title3", "Descriptionn3","content3" },
                { 4, 2,"Title4", "Descriptionn4","content4" },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
