
using LivroCaixa.Classes;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;

namespace LivroCaixa
{
    public partial class CadastroUsuarios : Page
    {
        // Verifica o acesso à página
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UsuarioEhAdministrador())
            {
                Response.Redirect("Index.aspx");
                return;
            }
        }

        // Centraliza a verificação de autorização
        private bool UsuarioEhAdministrador()
        {
            return Session["UsuarioId"] != null &&
                   Session["UsuarioPerfil"]?.ToString() == "ADM";
        }

        //Evento do botão Atualizar

        protected void btnAtualizar_Click(object sender, EventArgs e)
        {
            if (!UsuarioEhAdministrador())
            {
                Response.Redirect("Index.aspx");
                return;
            }

            if (!int.TryParse(hfUsuarioId.Value, out int id) || id <= 0)
            {
                lblMensagem.Text = "Selecione um usuário para editar.";
                return;
            }

            if (!ValidarDadosUsuario())
                return;

            string nome = txtNome.Text.Trim();
            string login = txtLogin.Text.Trim();
            string cpf = NormalizarCpf(txtCPF.Text);
            string perfil = ddlPerfil.SelectedValue;

            Conexao banco = new Conexao();

            try
            {
                banco.Abrir();

                int linhas = banco.AtualizarUsuario(
                    id, nome, login, cpf, perfil
                );

                lblMensagem.Text = linhas == 1
                    ? "Usuário atualizado com sucesso!"
                    : "Usuário não encontrado.";
            }
            catch (SqlException ex)
                when (ex.Number == 2601 || ex.Number == 2627)
            {
                lblMensagem.Text = "Login ou identificador já cadastrado.";
            }
            catch (SqlException)
            {
                lblMensagem.Text = "Erro ao atualizar usuário.";
            }
            finally
            {
                banco.Fechar();
            }
        }


        // Evento do botão Incluir
        protected void btnIncluir_Click(object sender, EventArgs e)
        {
            if (!UsuarioEhAdministrador())
            {
                Response.Redirect("Index.aspx");
                return;
            }

            lblMensagem.Text = "";

            if (!ValidarCampos())
            {
                return;
            }

            string nome = txtNome.Text.Trim();
            string login = txtLogin.Text.Trim();
            string cpf = NormalizarCpf(txtCPF.Text);
            string perfil = ddlPerfil.SelectedValue;
            string senha = txtSenhaInicial.Text;

            // Gera as credenciais protegidas
            Autenticacao autenticacao = new Autenticacao();

            byte[] salt = autenticacao.GerarSalt();
            byte[] hash = autenticacao.GerarHash(senha, salt);

            Conexao banco = new Conexao();

            try
            {
                banco.Abrir();

                if (banco.LoginExiste(login))
                {
                    lblMensagem.Text = "Este login já está cadastrado.";
                    return;
                }

                int linhasAfetadas = banco.InserirUsuario(
                    nome,
                    login,
                    cpf,
                    perfil,
                    hash,
                    salt
                );

                if (linhasAfetadas == 1)
                {
                    lblMensagem.Text =
                        "Usuário cadastrado com sucesso!";

                    LimparFormulario();
                }
                else
                {
                    lblMensagem.Text =
                        "Não foi possível cadastrar o usuário.";
                }
            }
            catch (SqlException ex)
                when (ex.Number == 2601 || ex.Number == 2627)
            {
                lblMensagem.Text =
                    "Login ou identificador já cadastrado.";
            }
            catch (SqlException)
            {
                lblMensagem.Text =
                    "Erro de banco de dados ao cadastrar usuário.";
            }
            finally
            {
                banco.Fechar();
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (!UsuarioEhAdministrador())
            {
                Response.Redirect("Index.aspx");
                return;
            }

            string termo = txtPesquisa.Text.Trim();

            Conexao banco = new Conexao();

            try
            {
                banco.Abrir();

                gvUsuarios.DataSource = banco.PesquisarUsuario(termo);
                gvUsuarios.DataBind();

                lblMensagem.Text = "Pesquisa concluída.";
            }
            catch (System.Data.SqlClient.SqlException)
            {
                lblMensagem.Text = "Erro ao pesquisar usuários.";
            }
            finally
            {
                banco.Fechar();
            }
        }



        protected void gvUsuarios_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            if (!UsuarioEhAdministrador())
            {
                Response.Redirect("Index.aspx");
                return;
            }

            if (gvUsuarios.SelectedDataKey == null)
            {
                lblMensagem.Text = "Nenhum usuário selecionado.";
                return;
            }

            int id = Convert.ToInt32(
                gvUsuarios.SelectedDataKey.Value
            );

            Conexao banco = new Conexao();

            try
            {
                banco.Abrir();

                DataRow usuario = banco.BuscarUsuarioPorId(id);

                if (usuario == null)
                {
                    lblMensagem.Text = "Usuário não encontrado.";
                    return;
                }

                hfUsuarioId.Value = usuario["Id"].ToString();
                txtNome.Text = usuario["Nome"].ToString();
                txtLogin.Text = usuario["Login"].ToString();

                txtCPF.Text = usuario["CPF"] == DBNull.Value
                    ? ""
                    : usuario["CPF"].ToString();

                ddlPerfil.SelectedValue = usuario["Perfil"].ToString();

                // Nunca recuperamos a senha original
                txtSenhaInicial.Text = "";

                lblMensagem.Text = "Usuário carregado para edição.";
            }
            finally
            {
                banco.Fechar();
            }
        }


        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            if (!UsuarioEhAdministrador())
            {
                Response.Redirect("Index.aspx");
                return;
            }

            if (!int.TryParse(hfUsuarioId.Value, out int id) || id <= 0)
            {
                lblMensagem.Text = "Selecione um usuário para excluir.";
                return;
            }

            Conexao banco = new Conexao();

            try
            {
                banco.Abrir();

                int linhas = banco.ExcluirUsuario(id);

                if (linhas == 1)
                {
                    hfUsuarioId.Value = "";
                    LimparFormulario();

                    gvUsuarios.DataSource = null;
                    gvUsuarios.DataBind();

                    lblMensagem.Text = "Usuário excluído com sucesso!";
                }
                else
                {
                    lblMensagem.Text =
                        "Usuário não encontrado ou exclusão não permitida.";
                }
            }
            catch (SqlException)
            {
                lblMensagem.Text = "Não foi possível excluir o usuário.";
            }
            finally
            {
                banco.Fechar();
            }
        }



        // Validação dos campos do formulário
        private bool ValidarCampos()
        {
            if (!ValidarDadosUsuario())
                return false;

            if (string.IsNullOrWhiteSpace(txtSenhaInicial.Text) ||
                txtSenhaInicial.Text.Length < 12)
            {
                lblMensagem.Text = "A senha deve ter pelo menos 12 caracteres.";
                return false;
            }

            return true;
        }


        private bool ValidarDadosUsuario()
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                txtNome.Text.Trim().Length > 100)
            {
                lblMensagem.Text = "Nome inválido.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLogin.Text) ||
                txtLogin.Text.Trim().Length > 50)
            {
                lblMensagem.Text = "Login inválido.";
                return false;
            }

            if (!CpfValido(txtCPF.Text))
            {
                lblMensagem.Text = "CPF inválido.";
                return false;
            }

            if (ddlPerfil.SelectedValue != "ADM" &&
                ddlPerfil.SelectedValue != "USU")
            {
                lblMensagem.Text = "Perfil inválido.";
                return false;
            }

            return true;
        }

        
        // Remove a pontuação do CPF
        private string NormalizarCpf(string cpf)
        {
            return cpf.Trim()
                .Replace(".", "")
                .Replace("-", "");
        }

        // Valida os dígitos verificadores do CPF
        private bool CpfValido(string cpf)
        {
            cpf = NormalizarCpf(cpf);

            if (cpf.Length != 11 || !cpf.All(char.IsDigit))
            {
                return false;
            }

            if (cpf.Distinct().Count() == 1)
            {
                return false;
            }

            for (int digito = 9; digito < 11; digito++)
            {
                int soma = 0;

                for (int i = 0; i < digito; i++)
                {
                    soma += (cpf[i] - '0') * (digito + 1 - i);
                }

                int resto = soma % 11;
                int verificador = resto < 2 ? 0 : 11 - resto;

                if (verificador != cpf[digito] - '0')
                {
                    return false;
                }
            }

            return true;
        }

        // Limpa os campos após o cadastro
        private void LimparFormulario()
        {
            txtNome.Text = "";
            txtLogin.Text = "";
            txtCPF.Text = "";
            txtSenhaInicial.Text = "";
            ddlPerfil.SelectedValue = "USU";
        }
    }
}
