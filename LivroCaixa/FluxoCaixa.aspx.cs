using LivroCaixa.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LivroCaixa
{
    public partial class FluxoCaixa : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("Index.aspx");
                return;
            }
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("Index.aspx");
                return;
            }

            if (!DateTime.TryParseExact(
                txtData.Text,
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateTime data))
            {
                lblMensagem.Text = "Informe uma data válida.";
                return;
            }

            string descricao = txtDescricao.Text.Trim();
            string tipo = ddlTipo.SelectedValue;

            if (string.IsNullOrWhiteSpace(descricao))
            {
                lblMensagem.Text = "Informe a descrição.";
                return;
            }

            if (tipo != "C" && tipo != "D")
            {
                lblMensagem.Text = "Tipo de movimentação inválido.";
                return;
            }

            if (!decimal.TryParse(
                txtValor.Text,
                System.Globalization.NumberStyles.Number,
                System.Globalization.CultureInfo.InvariantCulture,
                out decimal valor) || valor <= 0)
            {
                lblMensagem.Text = "Informe um valor positivo válido.";
                return;
            }

            if  (!int.TryParse(
                Session["UsuarioId"]?.ToString(),
                out  int usuarioId))
                {
                lblMensagem.Text = "Sessão inválida. Entre novamente";
                return;
            }

            if (descricao.Length > 200)
            {
                lblMensagem.Text = "Descrição muito longa.";
                return;
            }

            if (valor > 9999999999999999.99m)
            {
                lblMensagem.Text = "Valor acima do limite permitido.";
                return;
            }

            Movimentacao movimento = new Movimentacao
            {
                DataMovimentacao = data,
                Descricao = descricao,
                Tipo = tipo,
                Valor = valor,
                UsuarioId = usuarioId
            };

            Conexao banco = new Conexao();

            try
            {
                banco.Abrir();

                int linhas = banco.InserirMovimentacao(movimento);

                lblMensagem.Text = linhas == 1
                    ? "Movimentação registrada com sucesso! 🥹"
                    : "Não foi possível registrar a movimentação. 😵🫠";
            }
            catch (SqlException ex) when (ex.Number == 50002)
            {
                lblMensagem.Text =
                    "Não é permitido registrar movimentações em um mês fechado.";
            }
            catch (SqlException)
            {
                lblMensagem.Text = "Erro ao salvar a movimentação.";
            }
            finally
            {
                banco.Fechar();
            }
        }


        protected void btnConsultarResumo_Click(object sender, EventArgs e)
        {
            if (Session["UsuarioId"] == null)
            {
                Response.Redirect("Index.aspx");
                return;
            }

            if (!int.TryParse(txtAno.Text, out int ano) ||
                ano < 2000 || ano > 9998 ||
                !int.TryParse(ddlMes.SelectedValue, out int mes) ||
                mes < 1 || mes > 12)
            {
                lblMensagem.Text = "Informe um mês e ano válidos.";
                return;
            }

            Conexao banco = new Conexao();

            try
            {
                banco.Abrir();

                DataRow resumo = banco.ConsultarResumoMensal(ano, mes);

                decimal creditos = Convert.ToDecimal(resumo["TotalCreditos"]);
                decimal debitos = Convert.ToDecimal(resumo["TotalDebitos"]);
                decimal saldo = Convert.ToDecimal(resumo["Saldo"]);
                decimal saldoInicial = banco.ConsultarSaldoAnterior(ano, mes);
                decimal saldoFinal = saldoInicial + saldo;

                lblCreditos.Text = creditos.ToString("C2");
                lblDebitos.Text = debitos.ToString("C2");
                lblSaldo.Text = saldo.ToString("C2");
                lblSaldoInicial.Text = saldoInicial.ToString("C2");
                lblSaldoFinal.Text = saldoFinal.ToString("C2");

                lblMensagem.Text = "Resumo consultado com sucesso!";
            }
            catch (System.Data.SqlClient.SqlException)
            {
                lblMensagem.Text = "Erro ao consultar o resumo.";
            }
            finally
            {
                banco.Fechar();
            }
        }


        protected void btnFecharMes_Click(object sender, EventArgs e)
        {
            if (Session["UsuarioId"] == null ||
                Session["UsuarioPerfil"]?.ToString() != "ADM")
            {
                lblMensagem.Text =
                    "Somente administradores podem fechar períodos.";
                return;
            }

            if (!int.TryParse(txtAno.Text, out int ano) ||
                !int.TryParse(ddlMes.SelectedValue, out int mes) ||
                ano < 2000 || ano > 9998 ||
                mes < 1 || mes > 12)
            {
                lblMensagem.Text = "Mês ou ano inválido.";
                return;
            }

            Conexao banco = new Conexao();

            try
            {
                banco.Abrir();

                banco.FecharPeriodoCaixa(ano, mes);

                lblMensagem.Text =
                    "Período fechado com sucesso!";
            }
            catch (System.Data.SqlClient.SqlException ex)
                when (ex.Number == 50004)
            {
                lblMensagem.Text = "Este período já está fechado.";
            }
            catch (System.Data.SqlClient.SqlException)
            {
                lblMensagem.Text = "Erro ao fechar o período.";
            }
            finally
            {
                banco.Fechar();
            }
        }

    }
}