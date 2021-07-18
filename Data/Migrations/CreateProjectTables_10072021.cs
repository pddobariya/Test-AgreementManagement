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
                    Active = table.Column<bool>(nullable: false),
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

            migrationBuilder.InsertData(table: "ProductGroup", columns: new[] { "GroupDescription", "GroupCode", "Active" }, values: new object[] { "Group 1", "Code1", true });
            migrationBuilder.InsertData(table: "ProductGroup", columns: new[] { "GroupDescription", "GroupCode", "Active" }, values: new object[] { "Group 2", "Code2", true });
            migrationBuilder.InsertData(table: "ProductGroup", columns: new[] { "GroupDescription", "GroupCode", "Active" }, values: new object[] { "Group 3", "Code3", true });

            migrationBuilder.InsertData(table: "Product", columns: new[] { "ProductGroupId", "ProductDescription", "ProductNumber", "Price", "Active" }, values: new object[] { 1, "Product description1", "P-1",10,true });
            migrationBuilder.InsertData(table: "Product", columns: new[] { "ProductGroupId", "ProductDescription", "ProductNumber", "Price", "Active" }, values: new object[] { 1, "Product description2", "P-2", 15, true });

            migrationBuilder.InsertData(table: "Product", columns: new[] { "ProductGroupId", "ProductDescription", "ProductNumber", "Price", "Active" }, values: new object[] { 2, "Product description11", "P-3", 25, true });
            migrationBuilder.InsertData(table: "Product", columns: new[] { "ProductGroupId", "ProductDescription", "ProductNumber", "Price", "Active" }, values: new object[] { 2, "Product description22", "P-4", 20, true });

            migrationBuilder.InsertData(table: "Product", columns: new[] { "ProductGroupId", "ProductDescription", "ProductNumber", "Price", "Active" }, values: new object[] { 3, "Product description31", "P-5", 21, true });
            migrationBuilder.InsertData(table: "Product", columns: new[] { "ProductGroupId", "ProductDescription", "ProductNumber", "Price", "Active" }, values: new object[] { 3, "Product description32", "P-6", 22, true });
            migrationBuilder.InsertData(table: "Product", columns: new[] { "ProductGroupId", "ProductDescription", "ProductNumber", "Price", "Active" }, values: new object[] { 3, "Product description33", "P-7", 23, true });

            var sp = @"CREATE PROCEDURE SearchAgreement
                    (
	                    @USERId NVARCHAR(max) = '',
	                    @SearchValue NVARCHAR(max) = '',
	                    @SortColumn NVARCHAR(100) ='',
	                    @SortColumnDirection NVARCHAR(100) ='',
	                    @Skip INT =0,
	                    @PageSize INT=10
                    )
                    AS
                    BEGIN
	                    DECLARE @SQL NVARCHAR(MAX)
	                    IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'#tmpagreement')
	                    BEGIN
		                    DROP TABLE #tmpagreement
	                    END

	                    CREATE TABLE  #tmpagreement (Id int  NOT NULL identity(1,1), AgreementId int)
	                    SET @SQL ='INSERT INTO #tmpagreement 
	                    SELECT 
		                    a.Id 
	                    FROM Agreement a
	                    INNER JOIN ProductGroup pg on pg.Id = a.ProductGroupId
	                    INNER JOIN Product p on p.Id = a.ProductId
	                    WHERE UserId = '''+@USERId+'''
		                    AND (
			                    ('''+@SearchValue+''' = '''' OR '''+ISNULL(@SearchValue,'')+''' IS NULL OR pg.GroupCode LIKE ''%'+@SearchValue+'%'' )
			                    OR ('''+@SearchValue+''' = '''' OR '''+ISNULL(@SearchValue,'')+''' IS NULL OR p.ProductNumber LIKE ''%'+@SearchValue+'%'' ))'
	                    if(@SortColumn != '' AND @SortColumnDirection != '')
	                    BEGIN
		                    SET @SQL = @SQL + ' ORDER BY '+@SortColumn + ' ' +@SortColumnDirection+''
	                    END

	                    EXEC(@SQL)

	                    SELECT TOP(@PageSize)
		                    a.* 
	                    FROM Agreement a
		                    INNER JOIN #tmpagreement tmp on a.Id = tmp.AgreementId
		                    INNER JOIN ProductGroup pg on pg.Id = a.ProductGroupId
		                    INNER JOIN Product p on p.Id = a.ProductId
	                    WHERE tmp.Id > @Skip
	                    ORDER BY tmp.Id

	                    DROP TABLE #tmpagreement
                    END
                    GO
                ";

            migrationBuilder.Sql(sp);
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
