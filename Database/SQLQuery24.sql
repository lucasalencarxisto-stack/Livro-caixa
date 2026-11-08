CREATE TABLE dbo.PeriodoCaixa
(
	Id INT IDENTITY(1,1) PRIMARY KEY,

	Ano INT NOT NULL,
	Mes INT NOT NULL,

	Fechado BIT NOT NULL
		CONSTRAINT DF_PeriodoCaixa_Fechado DEFAULT 0,

	DataFechamento DATETIME2 NULL,

	SaldoFinal DECIMAL(18,2) NULL,

	CONSTRAINT UQ_PeriodoCaixa_AnoMes
		UNIQUE (Ano, Mes),

	CONSTRAINT CK_PeriodoCaixa_Mes
		CHECK (Mes BETWEEN 1 AND 12),

	CONSTRAINT CK_PeriodoCaixa_Ano
        CHECK (Ano BETWEEN 2000 AND 9998)
);