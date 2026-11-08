# Livro-caixa

# 📚 LivroCaixa

### Sistema de Controle Financeiro desenvolvido com ASP.NET Web Forms, C# e SQL Server

![.NET Framework](https://img.shields.io/badge/.NET_Framework-4.8-512BD4?style=for-the-badge)
![C#](https://img.shields.io/badge/C%23-Language-239120?style=for-the-badge)
![SQL Server](https://img.shields.io/badge/SQL_Server-Database-CC2927?style=for-the-badge)
![IIS](https://img.shields.io/badge/IIS-Web_Server-0078D4?style=for-the-badge)

## 📖 Sobre o projeto

O **LivroCaixa** é uma aplicação web desenvolvida em **C# com ASP.NET Web Forms**, utilizando **SQL Server** para armazenamento de dados.

O sistema permite gerenciar usuários, registrar movimentações financeiras e acompanhar o fluxo de caixa mensal, incluindo o fechamento de períodos contábeis.

O projeto foi desenvolvido como atividade prática de aprendizagem em desenvolvimento web, integração com bancos de dados relacionais e publicação de aplicações no IIS.

## 🚀 Funcionalidades

### 🔐 Autenticação e usuários

- Login e logout com controle de sessão.
- Armazenamento seguro de senhas com PBKDF2 e SHA-256, utilizando salt individual.
- Perfis de acesso: administrador (`ADM`) e usuário (`USU`).
- Cadastro, consulta, edição e exclusão de usuários.
- Validação de dados e prevenção de logins duplicados.

### 💰 Controle financeiro

- Registro de créditos (entradas).
- Registro de débitos (saídas).
- Consulta de movimentações por período.
- Cálculo de receitas, despesas e saldo mensal.
- Apuração do saldo anterior e saldo final.
- Transferência do saldo acumulado entre períodos.

### 📅 Fechamento mensal

- Fechamento de períodos por administradores.
- Registro do saldo final do mês.
- Bloqueio de novas movimentações em períodos fechados.
- Validação das regras financeiras no banco de dados.

## 🛠️ Tecnologias utilizadas

| Tecnologia | Utilização |
|---|---|
| C# | Regras de negócio e desenvolvimento backend |
| ASP.NET Web Forms | Interface e processamento de requisições |
| .NET Framework 4.8 | Plataforma de execução |
| SQL Server | Banco de dados relacional |
| ADO.NET | Comunicação com o banco de dados |
| T-SQL | Consultas e procedimentos armazenados |
| HTML e CSS | Estrutura e estilização das páginas |
| IIS | Hospedagem local da aplicação |
| Visual Studio | Desenvolvimento e publicação |
| Git e GitHub | Versionamento do projeto |

## 🏗️ Organização da aplicação

O projeto utiliza a estrutura do ASP.NET Web Forms, com páginas `.aspx` e arquivos C# de code-behind.

Principais componentes:

- `Index.aspx` — autenticação e menu principal.
- `CadastroUsuarios.aspx` — gerenciamento de usuários.
- `FluxoCaixa.aspx` — movimentações e consultas financeiras.
- `Classes/Conexao.cs` — operações de acesso ao banco de dados.
- `Classes/Autenticacao.cs` — geração e verificação de hashes de senha.
- `Classes/Movimentacao.cs` — representação das movimentações financeiras.

### Estrutura do banco de dados

**Usuario:** cadastro de usuários, perfis e credenciais protegidas.

**Movimentacao:** histórico das operações financeiras de crédito e débito.

**PeriodoCaixa:** controle de períodos mensais, fechamento e saldos.

O banco também utiliza procedimentos armazenados para registrar movimentações e executar o fechamento mensal.

## 💻 Executando localmente

### Pré-requisitos

- Windows
- Visual Studio com suporte ao desenvolvimento ASP.NET
- .NET Framework 4.8
- SQL Server LocalDB ou instância compatível do SQL Server
- IIS, caso deseje executar a versão publicada

### 1. Clonar o repositório

```bash
git clone https://github.com/lucasalencarxisto-stack/Livro-caixa.git
```

### 2. Abrir o projeto

Abra a solução do LivroCaixa no Visual Studio.

### 3. Configurar o banco de dados

Configure uma instância do SQL Server e crie a estrutura necessária para as tabelas e procedimentos armazenados.

O ambiente de desenvolvimento original utiliza:

```text
Servidor: (localdb)\MSSQLLocalDB
Banco: LivroCaixa
Autenticação: Windows
```

> Atenção: os scripts de criação do banco e eventuais dados iniciais precisam estar disponíveis para que outra pessoa consiga reproduzir integralmente o ambiente.

### 4. Executar

Compile a solução e inicie a aplicação pelo Visual Studio.

Para publicação no IIS, utilize a configuração Release e configure um Application Pool compatível com o .NET Framework 4.x.

O ambiente publicado deve utilizar uma identidade com as permissões necessárias para acessar o banco de dados.

## 🔒 Considerações de segurança

O projeto aplica técnicas de proteção de credenciais e validação de dados, incluindo:

- Hash de senhas com PBKDF2-HMAC-SHA256.
- Salt individual por senha.
- Consultas SQL parametrizadas.
- Verificação de sessão e perfil de acesso.
- Restrições de movimentações em períodos fechados.

O projeto tem finalidade educacional. Antes de disponibilizá-lo publicamente, é necessário realizar uma revisão adicional de segurança, permissões, configurações, tratamento de exceções e infraestrutura.

## ✅ Testes realizados

Durante o desenvolvimento, foram realizados testes manuais de:

- Autenticação de usuários.
- Cadastro e gerenciamento de usuários.
- Lançamento de créditos e débitos.
- Cálculo de saldos mensais.
- Transporte de saldo entre períodos.
- Fechamento mensal.
- Bloqueio de lançamentos em períodos encerrados.
- Execução da aplicação publicada em IIS local.

## 📚 Aprendizados

Este projeto proporcionou experiência prática em:

- Desenvolvimento backend com C#.
- Ciclo de vida de páginas ASP.NET Web Forms.
- Persistência de dados com SQL Server.
- Autenticação baseada em sessão.
- Operações CRUD.
- Procedimentos armazenados e regras de negócio.
- Depuração de aplicações web.
- Publicação e configuração de aplicações no IIS.

## 👨‍💻 Autor

**Lucas Xisto de Alencar**

Estudante de Análise e Desenvolvimento de Sistemas.

[GitHub](https://github.com/lucasalencarxisto-stack)

---

⭐ Projeto desenvolvido para fins de aprendizado e evolução profissional em desenvolvimento de software.
