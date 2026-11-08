SELECT Id, Nome, Perfil, SenhaHash, SenhaSalt
FROM dbo.Usuario
WHERE Login = @login;