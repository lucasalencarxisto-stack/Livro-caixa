
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;

namespace LivroCaixa.Classes
{
    public class Conexao
    {


        public DataRow ConsultarResumoMensal(int ano, int mes)
        {
            DateTime inicio = new DateTime(ano, mes, 1);
            DateTime fim = inicio.AddMonths(1);

            string sql = @"
        SELECT
            COALESCE(SUM(
                CASE WHEN Tipo = 'C' THEN Valor ELSE 0 END
            ), 0) AS TotalCreditos,

            COALESCE(SUM(
                CASE WHEN Tipo = 'D' THEN Valor ELSE 0 END
            ), 0) AS TotalDebitos,

            COALESCE(SUM(
                CASE
                    WHEN Tipo = 'C' THEN Valor
                    WHEN Tipo = 'D' THEN -Valor
                    ELSE 0
                END
            ), 0) AS Saldo
        FROM dbo.Movimentacao
        WHERE DataMovimentacao >= @inicio
          AND DataMovimentacao < @fim";

            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.Add("@inicio", SqlDbType.Date)
                    .Value = inicio;

                comando.Parameters.Add("@fim", SqlDbType.Date)
                    .Value = fim;

                using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                {
                    DataTable tabela = new DataTable();
                    adapter.Fill(tabela);

                    return tabela.Rows[0];
                }
            }
        }


        public DataRow BuscarCredenciais(string login)
        {
            string sql = @"
                SELECT Id, Nome, Perfil, SenhaHash, SenhaSalt
                FROM dbo.Usuario
                WHERE Login = @login";

            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.Add(
                    "@login", SqlDbType.NVarChar, 50
                ).Value = login;

                using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                {
                    DataTable tabela = new DataTable();
                    adapter.Fill(tabela);

                    if (tabela.Rows.Count == 0)
                        return null;

                    return tabela.Rows[0];
                }
            }
        }

        private SqlConnection conexao;

        public Conexao()
        {
            string connectionString =
                @"Data Source=(localdb)\.\LivroCaixaLab;" +
                "Initial Catalog=LivroCaixa;" +
                "Integrated Security=True;";

            conexao = new SqlConnection(connectionString);
        }

        public void Abrir()
        {
            conexao.Open();
        }

        public void Fechar()
        {
            if (conexao.State != ConnectionState.Closed)
            {
                conexao.Close();
            }
        }
        public string BuscarUsuario(string login)
        {
            string sql = "SELECT Nome FROM dbo.Usuario WHERE Login = @login";

            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.Add("@login", SqlDbType.NVarChar, 50)
                                  .Value = login;

                object resultado = comando.ExecuteScalar();

                return resultado == null ? null : resultado.ToString();

            }
        }
        public int SalvarSenha(
            string login,
            byte[] hash,
            byte[] salt)
        {
            string sql = @"
                        UPDATE dbo.Usuario
                        SET SenhaHash = @hash,
                            SenhaSalt = @salt
                        WHERE Login = @login";


            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.Add("@hash", SqlDbType.VarBinary, 32)
                                  .Value = hash;

                comando.Parameters.Add("@salt", SqlDbType.VarBinary, 16)
                                  .Value = salt;

                comando.Parameters.Add("@login", SqlDbType.NVarChar, 50)
                                  .Value = login;

                return comando.ExecuteNonQuery();
            }
        }



        public bool LoginExiste(string login)
        {
            string sql = @"
        SELECT COUNT(*)
        FROM dbo.Usuario
        WHERE Login = @login";

            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.Add(
                    "@login", SqlDbType.NVarChar, 50
                ).Value = login;

                int quantidade = (int)comando.ExecuteScalar();

                return quantidade > 0;
            }
        }
        public int InserirUsuario(
            string nome,
            string login,
            string cpf,
            string perfil,
            byte[] hash,
            byte[] salt)
        {

            string sql = @"
                    INSERT INTO dbo.Usuario
                    (Nome, Login, CPF, Perfil, SenhaHash, SenhaSalt)
                    VALUES
                    (@nome, @login, @cpf, @perfil, @hash, @salt)";
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.Add("@nome", SqlDbType.NVarChar, 100)
                                  .Value = nome;
                comando.Parameters.Add("@login", SqlDbType.NVarChar, 50)
                                  .Value = login;
                comando.Parameters.Add("@cpf", SqlDbType.NVarChar, 14)
                                  .Value = cpf;
                comando.Parameters.Add("@perfil", SqlDbType.NVarChar, 3)
                                  .Value = perfil;
                comando.Parameters.Add("@hash", SqlDbType.VarBinary, 32)
                                  .Value = hash;
                comando.Parameters.Add("@salt", SqlDbType.VarBinary, 16)
                                  .Value = salt;
                return comando.ExecuteNonQuery();
            }
        }


        public DataTable PesquisarUsuario(string termo)
        {
            string sql = @"
                    SELECT Id, Nome, Login, CPF, Perfil
                    FROM dbo.Usuario
                    WHERE Nome LIKE @termo
                        OR Login LIKE @termo
                    ORDER BY Nome ASC";

            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.Add(
                    "@termo", SqlDbType.NVarChar, 102
                    ).Value = "%" + termo + "%";

                using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                {
                    DataTable tabela = new DataTable();

                    adapter.Fill(tabela);

                    return tabela;

                }
            }
         }



        public DataRow BuscarUsuarioPorId(int id)
        {
            string sql = @"
                SELECT Id, Nome, Login, CPF, Perfil
                FROM dbo.Usuario
                WHERE Id = @id";

            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.Add(
                    "@id", SqlDbType.Int
                ).Value = id;

                using (SqlDataAdapter adapter = new SqlDataAdapter(comando))
                {
                    DataTable tabela = new DataTable();
                    adapter.Fill(tabela);

                    return tabela.Rows.Count > 0
                        ? tabela.Rows[0]
                        : null;
                }
            }
        }

         public int AtualizarUsuario(
             int id,
             string nome,
             string login,
             string cpf,
             string perfil)
         {
            string sql = @"
                UPDATE dbo.Usuario
                SET Nome = @nome,
                Login = @login,
                CPF = @cpf,
                Perfil = @perfil
                WHERE Id = @id";

            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.Add("@id", SqlDbType.Int).Value = id;
                comando.Parameters.Add("@nome", SqlDbType.NVarChar, 100).Value = nome;
                comando.Parameters.Add("@login", SqlDbType.NVarChar, 50).Value = login;
                comando.Parameters.Add("@cpf", SqlDbType.VarChar, 11).Value = cpf;
                comando.Parameters.Add("@perfil", SqlDbType.VarChar, 3).Value = perfil;

                return comando.ExecuteNonQuery();
            }
        }
        public int ExcluirUsuario(int id)
        {
            string sql = @"
                DELETE FROM dbo.Usuario
                WHERE Id = @id";
            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.Add("@id", SqlDbType.Int).Value = id;
                return comando.ExecuteNonQuery();
            }
        }

        public decimal ConsultarSaldoAnterior(int ano, int mes)
        {
            DateTime inicio = new DateTime(ano, mes, 1);

            string sql = @"
                SELECT COALESCE(SUM(
                    CASE
                        WHEN Tipo = 'C' THEN Valor
                        WHEN Tipo = 'D' THEN -Valor
                        ELSE 0
                    END
                ), 0) 
                FROM dbo.Movimentacao
                WHERE DataMovimentacao < @inicio";

            using (SqlCommand comando = new SqlCommand(sql, conexao))
            {
                comando.Parameters.Add("@inicio", SqlDbType.Date
                    ).Value = inicio;

                return Convert.ToDecimal(comando.ExecuteScalar());
            }
        }





        public int InserirMovimentacao(Movimentacao movimento)
        {
            using (SqlCommand comando = new SqlCommand(
                "dbo.RegistrarMovimentacao", conexao))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add("@data", SqlDbType.Date)
                    .Value = movimento.DataMovimentacao.Date;

                comando.Parameters.Add("@descricao", SqlDbType.NVarChar, 200)
                    .Value = movimento.Descricao;

                comando.Parameters.Add("@tipo", SqlDbType.Char, 1)
                    .Value = movimento.Tipo;

                SqlParameter parametroValor =
                    comando.Parameters.Add("@valor", SqlDbType.Decimal);

                parametroValor.Precision = 18;
                parametroValor.Scale = 2;
                parametroValor.Value = movimento.Valor;

                comando.Parameters.Add("@usuarioId", SqlDbType.Int)
                    .Value = movimento.UsuarioId;

                comando.ExecuteNonQuery();

                return 1;
            }
        }

        public void FecharPeriodoCaixa(int ano, int mes)
        {
            using (SqlCommand comando = new SqlCommand(
                "dbo.FecharPeriodoCaixa", conexao))
            {
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.Add("@ano", SqlDbType.Int)
                    .Value = ano;

                comando.Parameters.Add("@mes", SqlDbType.Int)
                    .Value = mes;

                comando.ExecuteNonQuery();
            }
        }

    }
}



    
  

            
        
     

  

     

    

   
  
       
     
    



    

