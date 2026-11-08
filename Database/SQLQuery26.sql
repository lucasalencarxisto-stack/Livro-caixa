
CREATE OR ALTER PROCEDURE dbo.RegistrarMovimentacao
    @data DATE,
    @descricao NVARCHAR(200),
    @tipo CHAR(1),
    @valor DECIMAL(18,2),
    @usuarioId INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @tipo NOT IN ('C', 'D')
       OR @tipo IS NULL
       OR @valor IS NULL
       OR @valor <= 0
       OR @data IS NULL
       OR NULLIF(LTRIM(RTRIM(@descricao)), '') IS NULL
    BEGIN
        THROW 50001, 'Movimentação inválida.', 1;
    END;

    BEGIN TRANSACTION;

    DECLARE @ano INT = YEAR(@data);
    DECLARE @mes INT = MONTH(@data);

    -- Garante um registro para o período.
    IF NOT EXISTS (
        SELECT 1
        FROM dbo.PeriodoCaixa WITH (UPDLOCK, HOLDLOCK)
        WHERE Ano = @ano AND Mes = @mes
    )
    BEGIN
        INSERT INTO dbo.PeriodoCaixa (Ano, Mes)
        VALUES (@ano, @mes);
    END;

    -- Bloqueia a linha do período durante o lançamento.
    IF EXISTS (
        SELECT 1
        FROM dbo.PeriodoCaixa WITH (UPDLOCK, HOLDLOCK)
        WHERE Ano = @ano
          AND Mes = @mes
          AND Fechado = 1
    )
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 50002, 'Este período está fechado.', 1;
    END;

    INSERT INTO dbo.Movimentacao
        (DataMovimentacao, Descricao, Tipo, Valor, UsuarioId)
    VALUES
        (@data, @descricao, @tipo, @valor, @usuarioId);

    COMMIT TRANSACTION;
END;
