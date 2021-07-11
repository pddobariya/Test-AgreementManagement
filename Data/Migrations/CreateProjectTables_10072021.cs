using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace AgreementManagement.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("CreateProjectTables_10072021")]
    public partial class CreateProjectTables_10072021 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                 name: "ProductGroup",
                 columns: table => new
                 {
                     Id = table.Column<int>(nullable: false)
                         .Annotation("SqlServer:Identity", "1,1"),
                     GroupDescription = table.Column<string>(maxLength: 1000, nullable: true),
                     GroupCode = table.Column<string>(maxLength: 256, nullable: true),
                     Active = table.Column<bool>(nullable: true)
                 },
                 constraints: table =>
                 {
                     table.PrimaryKey("PK_ProductGroup", x => x.Id);
                     table.UniqueConstraint("UK_ProductGroup_GroupCode", x => x.GroupCode);
                 });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1,1"),
                    ProductGroupId = table.Column<int>(nullable: false),
                    ProductDescription = table.Column<string>(maxLength: 1000, nullable: true),
                    ProductNumber = table.Column<string>(maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    Active = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.UniqueConstraint("UK_Product_ProductNumber", x => x.ProductNumber);
                    table.ForeignKey(
                        name: "FK_Product_ProductGroup_ProductGroupId",
                        column: x => x.ProductGroupId,
                        principalTable: "ProductGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);

                });

            migrationBuilder.CreateTable(
                name: "Agreement",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1,1"),
                    UserId = table.Column<string>(maxLength: 1000, nullable: true),
                    ProductGroupId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    EffectiveDate = table.Column<DateTime>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false),
                    ProductPrice = table.Column<decimal>(nullable: false),
                    NewPrice = table.Column<decimal>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agreement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agreement_ProductGroup_ProductGroupId",
                        column: x => x.ProductGroupId,
                        principalTable: "ProductGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);

                    table.ForeignKey(
                        name: "FK_Agreement_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
               name: "ProductGroup");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Agreement");
        }
    }
}
