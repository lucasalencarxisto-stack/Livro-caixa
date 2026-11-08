SELECT
    Id,
    Ano,
    Mes,
    Fechado,
    DataFechamento,
    SaldoFinal
FROM dbo.PeriodoCaixa
ORDER BY Ano, Mes;