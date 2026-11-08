SELECT
	Nome,
	Login,
	DATALENGTH(SenhaHash) AS BytesHash,
	DATALENGTH(SenhaSalt) AS ByteSalt
FROM dbo.Usuario; 

