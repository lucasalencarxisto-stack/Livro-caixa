SELECT Nome, SenhaHash,SenhaSalt
FROM dbo.Usuario
WHERE Login = @login;


