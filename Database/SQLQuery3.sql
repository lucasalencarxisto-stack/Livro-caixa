SELECT * FROM Usuario;
SELECT Nome, Login
FROM dbo.Usuario 
WHERE Login = 'lucas.dev';


ALTER TABLE dbo.Usuario
ADD
    SenhaHash VARBINARY(32) NULL,
    SenhaSalt VARBINARY(16) NULL;
