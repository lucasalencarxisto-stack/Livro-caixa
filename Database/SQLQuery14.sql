SELECT
    Id,
    Nome,
    Login,
    Perfil,
    DATALENGTH(SenhaHash) AS BytesHash,
    DATALENGTH(SenhaSalt) AS BytesSalt
FROM dbo.Usuario
ORDER BY Id DESC;