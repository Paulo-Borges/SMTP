using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMTP.API.Migrations
{
    /// <inheritdoc />
    public partial class adicionandoRelacionamentoUserMembro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Membros",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(@"
        DECLARE @UserPadraoId INT;

        SELECT TOP 1 @UserPadraoId = Id
        FROM Users
        ORDER BY Id;

        IF @UserPadraoId IS NULL
        BEGIN
            INSERT INTO Users (Nome, Email, Cpf)
            VALUES ('Usuário Legado', 'legado@local', '00000000000');

            SET @UserPadraoId = SCOPE_IDENTITY();
        END

        UPDATE Membros
        SET UserId = @UserPadraoId
        WHERE UserId IS NULL;
    ");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Membros",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Membros_UserId",
                table: "Membros",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Membros_Users_UserId",
                table: "Membros",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Membros_Users_UserId",
                table: "Membros");

            migrationBuilder.DropIndex(
                name: "IX_Membros_UserId",
                table: "Membros");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Membros");
        }
    }
}
