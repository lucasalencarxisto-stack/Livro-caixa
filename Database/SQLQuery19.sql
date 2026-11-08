SELECT
	Id,
	DataMovimentacao,
	Descricao,
	Tipo,
	Valor,
	UsuarioId
FROM dbo.Movimentacao
ORDER BY Id DESC;

