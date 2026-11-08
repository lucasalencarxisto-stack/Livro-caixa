
SELECT
    COALESCE(SUM(
        CASE WHEN Tipo = 'C' THEN Valor ELSE 0 END
    ), 0) AS TotalCreditos,

    COALESCE(SUM(
        CASE WHEN Tipo = 'D' THEN Valor ELSE 0 END
    ), 0) AS TotalDebitos,

    COALESCE(SUM(
        CASE
            WHEN Tipo = 'C' THEN Valor
            WHEN Tipo = 'D' THEN -Valor
            ELSE 0
        END
    ), 0) AS Saldo
FROM dbo.Movimentacao
WHERE DataMovimentacao >= '20261101'
  AND DataMovimentacao < '20261201';
