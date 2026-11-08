
CREATE TABLE dbo.Movimentacao
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    DataMovimentacao DATE NOT NULL,

    Descricao NVARCHAR(200) NOT NULL,

    Tipo CHAR(1) NOT NULL,

    Valor DECIMAL(18,2) NOT NULL,

    UsuarioId INT NOT NULL,

    CONSTRAINT CK_Movimentacao_Tipo
        CHECK (Tipo IN ('C', 'D')),

    CONSTRAINT CK_Movimentacao_Valor
        CHECK (Valor > 0),

    CONSTRAINT FK_Movimentacao_Usuario
        FOREIGN KEY (UsuarioId)
        REFERENCES dbo.Usuario(Id)
);
