
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="FluxoCaixa.aspx.cs"
    Inherits="LivroCaixa.FluxoCaixa" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Fluxo de Caixa</title>
</head>

<body>
    <form id="form1" runat="server">

        <h2>Fluxo de Caixa</h2>

        <p>Data da Movimentação:</p>
        <asp:TextBox
            ID="txtData"
            runat="server"
            TextMode="Date" />

        <p>Descrição:</p>
        <asp:TextBox
            ID="txtDescricao"
            runat="server"
            MaxLength="200" />

        <p>Tipo:</p>
        <asp:DropDownList
            ID="ddlTipo"
            runat="server">

            <asp:ListItem
                Text="Crédito (Entrada)"
                Value="C" />

            <asp:ListItem
                Text="Débito (Saída)"
                Value="D" />

        </asp:DropDownList>

        <p>Valor (R$):</p>
        <asp:TextBox
            ID="txtValor"
            runat="server"
            TextMode="Number"
            step="0.01"
            min="0.01" />

        <p>
            <asp:Button
                ID="btnRegistrar"
                runat="server"
                Text="Registrar movimentação"
                OnClick="btnRegistrar_Click" />
        </p>

        
 <h3>Resumo mensal</h3>

   <p>Mês:</p>
     <asp:DropDownList ID="ddlMes" runat="server">
       <asp:ListItem Value="1">Janeiro</asp:ListItem>
       <asp:ListItem Value="2">Fevereiro</asp:ListItem>
       <asp:ListItem Value="3">Março</asp:ListItem>
       <asp:ListItem Value="4">Abril</asp:ListItem>
       <asp:ListItem Value="5">Maio</asp:ListItem>
       <asp:ListItem Value="6">Junho</asp:ListItem>
       <asp:ListItem Value="7">Julho</asp:ListItem>
       <asp:ListItem Value="8">Agosto</asp:ListItem>
       <asp:ListItem Value="9">Setembro</asp:ListItem>
       <asp:ListItem Value="10">Outubro</asp:ListItem>
       <asp:ListItem Value="11">Novembro</asp:ListItem>
       <asp:ListItem Value="12">Dezembro</asp:ListItem>
     </asp:DropDownList>

    <p>
        Saldo Inicial:
        <asp:Label ID="lblSaldoInicial" runat="server" />
    </p>

    <p>
        Saldo Final:
        <asp:Label ID="lblSaldoFinal" runat="server" />
    </p>

    <p>Ano:</p>
     <asp:TextBox
        ID="txtAno"
        runat="server"
        TextMode="Number" />

      <p>
        <asp:Button
            ID="btnConsultarResumo"
            runat="server"
            Text="Consultar resumo"
            OnClick="btnConsultarResumo_Click" />
        </p>

        
            <asp:Button
                ID="btnFecharMes"
                runat="server"
                Text="Fechar mês"
                OnClick="btnFecharMes_Click"
                OnClientClick="return confirm('Confirma o fechamento deste mês?');" />


        <p>Créditos: <asp:Label ID="lblCreditos" runat="server" /></p>
        <p>Débitos: <asp:Label ID="lblDebitos" runat="server" /></p>
        <p>Saldo do mês: <asp:Label ID="lblSaldo" runat="server" /></p>

        <asp:Label
            ID="lblMensagem"
            runat="server" />

    </form>
</body>
</html>
