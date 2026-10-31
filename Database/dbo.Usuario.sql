
CREATE TABLE [dbo].[Usuario]
(
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Nome] NVARCHAR(100) NOT NULL,
    [Login] NVARCHAR(50) NOT NULL,
    [SenhaHash] VARBINARY(32) NULL,
    [SenhaSalt] VARBINARY(16) NULL,

    CONSTRAINT [PK_Usuario]
        PRIMARY KEY CLUSTERED ([Id]),

    CONSTRAINT [UQ_Usuario_Login]
        UNIQUE ([Login])
);
