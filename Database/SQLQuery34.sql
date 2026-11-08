SELECT COUNT(*) AS TotalLancamentos
FROM dbo.Movimentacao
WHERE DataMovimentacao >= '20261001'
  AND DataMovimentacao < '20261101';