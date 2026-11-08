
DECLARE @nome NVARCHAR(100) = 'Fernando Oliveira';
DECLARE @login NVARCHAR(50) = 'nando.oliveira';
DECLARE @cpf VARCHAR(11) = '12345678912';
DECLARE @perfil VARCHAR(3) = 'USU';

SELECT
    @nome AS Nome,
    @login AS Login,
    @cpf AS CPF,
    @perfil AS Perfil;
