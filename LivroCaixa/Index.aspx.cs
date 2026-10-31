
using System;
using System.Data;
using LivroCaixa.Classes;

namespace LivroCaixa
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        private void CadastrarSenhaTeste()
        {
            Autenticacao auth = new Autenticacao();

            string senha = "LivroCaixa123!";
            byte[] salt = auth.GerarSalt();
            byte[] hash = auth.GerarHash(senha, salt);

            Conexao banco = new Conexao();

            try
            {
                banco.Abrir();

                int linhas = banco.SalvarSenha(
                    "lucas.dev", hash, salt);

                System.Diagnostics.Debug.WriteLine(
                    "Usuários atualizados: " + linhas);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
                banco.Fechar();
            }
        }

        private void TestarAutenticacao()
        {
            Autenticacao auth = new Autenticacao();

            string senha = "LivroCaixa123!";

            byte[] salt1 = auth.GerarSalt();
            byte[] hash1 = auth.GerarHash(senha, salt1);

            bool teste1 = auth.VerificarSenha(
                "LivroCaixa123!", salt1, hash1);

            bool teste2 = auth.VerificarSenha(
                "SenhaErrada", salt1, hash1);

            byte[] salt2 = auth.GerarSalt();
            byte[] hash2 = auth.GerarHash(senha, salt2);

            bool teste3 =
                !System.Linq.Enumerable.SequenceEqual(hash1, hash2);

            System.Diagnostics.Debug.WriteLine(
                "Senha correta: " + teste1);

            System.Diagnostics.Debug.WriteLine(
                "Senha incorreta: " + teste2);

            System.Diagnostics.Debug.WriteLine(
                "Hashes diferentes: " + teste3);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }
        protected void btnSair_Click(object sender, EventArgs e)
        {
            Session.Clear();

            pnMenu.Visible = false;
            pnLogin.Visible = true;

            txtNome.Text = "";
            lblMensagem.Text = "Sessão encerrada!";
        }

        protected void btnEntrar_Click(object sender, EventArgs e)
        {
            Conexao banco = new Conexao();

            try
            {
                string login = txtNome.Text.Trim();
                string senha = txtSenha.Text;
                   


                banco.Abrir();

                DataRow usuario = banco.BuscarCredenciais(login);

                if (usuario == null ||
                    usuario["SenhaHash"] == DBNull.Value ||
                    usuario["SenhaSalt"] == DBNull.Value)
                {
                    lblMensagem.Text = "Login ou senha inválidos.";
                    return;
                }

                Autenticacao auth = new Autenticacao();

                bool senhaValida = auth.VerificarSenha(
                    senha,
                    (byte[])usuario["SenhaSalt"],
                    (byte[])usuario["SenhaHash"]
                );

                if (!senhaValida)
                {
                    lblMensagem.Text = "Login ou senha inválidos.";
                    return;
                }

                Session["UsuarioId"] = usuario["Id"];
                Session["UsuarioNome"] = usuario["Nome"].ToString();

                pnLogin.Visible = false;
                pnMenu.Visible = true;

                lblMensagem.Text = "Bem-vindo, " +
                    Server.HtmlEncode(usuario["Nome"].ToString()) + "!";
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao processar o login.";
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
                banco.Fechar();
            }
        }
    }
}

    