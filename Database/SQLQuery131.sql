
CREATE OR ALTER PROCEDURE dbo.FecharPeriodoCaixa
    @ano INT,
    @mes INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @ano NOT BETWEEN 2000 AND 9998
       OR @mes NOT BETWEEN 1 AND 12
       OR @ano IS NULL
       OR @mes IS NULL
    BEGIN
        THROW 50003, 'Periodo invalido.', 1;
    END;

    DECLARE @inicio DATE = DATEFROMPARTS(@ano, @mes, 1);
    DECLARE @fim DATE = DATEADD(MONTH, 1, @inicio);
    DECLARE @saldo DECIMAL(18,2);

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Bloqueia o periodo durante o fechamento
        IF NOT EXISTS (
            SELECT 1
            FROM dbo.PeriodoCaixa WITH (UPDLOCK, HOLDLOCK)
            WHERE Ano = @ano AND Mes = @mes
        )
        BEGIN
            INSERT INTO dbo.PeriodoCaixa (Ano, Mes)
            VALUES (@ano, @mes);
        END;

        IF EXISTS (
            SELECT 1
            FROM dbo.PeriodoCaixa
            WHERE Ano = @ano
              AND Mes = @mes
              AND Fechado = 1
        )
        BEGIN
            THROW 50004, 'Periodo ja fechado.', 1;
        END;

        -- Calcula o saldo acumulado ate o fim do mes
        SELECT @saldo = COALESCE(SUM(
            CASE
                WHEN Tipo = 'C' THEN Valor
                WHEN Tipo = 'D' THEN -Valor
                ELSE 0
            END
        ), 0)
        FROM dbo.Movimentacao
        WHERE DataMovimentacao < @fim;

        UPDATE dbo.PeriodoCaixa
        SET Fechado = 1,
            DataFechamento = SYSDATETIME(),
            SaldoFinal = @saldo
        WHERE Ano = @ano AND Mes = @mes;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH
END;
