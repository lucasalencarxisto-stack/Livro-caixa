
-- Cadastrar usuários de teste
-- Executar apenas se ainda não estiverem cadastrados

IF NOT EXISTS (
    SELECT 1 FROM dbo.Usuario
    WHERE Login = 'lucas.dev'
)
BEGIN
    INSERT INTO dbo.Usuario (Nome, Login)
    VALUES ('Lucas', 'lucas.dev');
END;

IF NOT EXISTS (
    SELECT 1 FROM dbo.Usuario
    WHERE Login = 'carlos.dev'
)
BEGIN
    INSERT INTO dbo.Usuario (Nome, Login)
    VALUES ('Carlos', 'carlos.dev');
END;

-- Consultar todos os usuários
SELECT * FROM dbo.Usuario;

-- Consultar um usuário específico
SELECT Nome, Login
FROM dbo.Usuario
WHERE Login = 'lucas.dev';






