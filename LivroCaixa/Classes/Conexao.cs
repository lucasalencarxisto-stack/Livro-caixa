
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


        public DataRow BuscarCredenciais(string login)
        {
            string sql = @"
        SELECT Id, Nome, SenhaHash, SenhaSalt
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
                @"Data Source=(localdb)\MSSQLLocalDB;" +
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

                comando.Parameters.Add("login", SqlDbType.NVarChar, 50)
                                  .Value = login;

                return comando.ExecuteNonQuery();
           }
         }
       }
     }
    



    

