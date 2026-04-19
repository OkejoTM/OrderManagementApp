using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressNormalizedName : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Добавляем колонку NormalizedName (пока nullable, чтобы успеть заполнить)
            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Addresses",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);
 
            // 2. Нормализуем Name (trim + collapse spaces) для существующих записей
            migrationBuilder.Sql(@"
                UPDATE ""Addresses""
                SET ""Name"" = REGEXP_REPLACE(TRIM(""Name""), '\s+', ' ', 'g');
            ");
 
            // 3. Заполняем NormalizedName = lower(нормализованного Name)
            migrationBuilder.Sql(@"
                UPDATE ""Addresses""
                SET ""NormalizedName"" = LOWER(""Name"");
            ");
 
            // 4. Мерджим дубликаты (AreaId + NormalizedName).
            //    Победитель выбирается через ROW_NUMBER() ORDER BY Id::text — детерминированно,
            //    при этом не полагаемся на MIN(uuid) (которой в PostgreSQL нет).
            //    Все AddressHistories перевешиваем на победителя, проигравшие адреса удаляем.
            migrationBuilder.Sql(@"
                WITH ranked AS (
                    SELECT
                        ""Id"",
                        ""AreaId"",
                        ""NormalizedName"",
                        ROW_NUMBER() OVER (
                            PARTITION BY ""AreaId"", ""NormalizedName""
                            ORDER BY ""Id""::text
                        ) AS rn
                    FROM ""Addresses""
                ),
                winners AS (
                    SELECT ""AreaId"", ""NormalizedName"", ""Id"" AS ""WinnerId""
                    FROM ranked
                    WHERE rn = 1
                ),
                losers AS (
                    SELECT r.""Id"" AS ""LoserId"", w.""WinnerId""
                    FROM ranked r
                    INNER JOIN winners w
                        ON r.""AreaId"" = w.""AreaId""
                        AND r.""NormalizedName"" = w.""NormalizedName""
                    WHERE r.rn > 1
                )
                UPDATE ""AddressHistories"" h
                SET ""AddressId"" = l.""WinnerId""
                FROM losers l
                WHERE h.""AddressId"" = l.""LoserId"";
            ");
 
            migrationBuilder.Sql(@"
                DELETE FROM ""Addresses"" a
                USING (
                    SELECT
                        ""Id"",
                        ROW_NUMBER() OVER (
                            PARTITION BY ""AreaId"", ""NormalizedName""
                            ORDER BY ""Id""::text
                        ) AS rn
                    FROM ""Addresses""
                ) d
                WHERE a.""Id"" = d.""Id"" AND d.rn > 1;
            ");
 
            // 5. Делаем NormalizedName NOT NULL
            migrationBuilder.AlterColumn<string>(
                name: "NormalizedName",
                table: "Addresses",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256,
                oldNullable: true);
 
            // 6. Создаём unique index на (AreaId, NormalizedName)
            migrationBuilder.CreateIndex(
                name: "IX_Addresses_AreaId_NormalizedName",
                table: "Addresses",
                columns: new[] { "AreaId", "NormalizedName" },
                unique: true);
        }
 
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_AreaId_NormalizedName",
                table: "Addresses");
 
            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Addresses");
        }
    }
}
