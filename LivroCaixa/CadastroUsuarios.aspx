
<%@ Page Language="C#" AutoEventWireup="true"
    CodeBehind="CadastroUsuarios.aspx.cs"
    Inherits="LivroCaixa.CadastroUsuarios" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Cadastro de Usuários</title>
</head>
<body>
    <form id="form1" runat="server">

        <h2>Cadastro de Usuários</h2>

        <p>Nome:</p>
        <asp:TextBox ID="txtNome" runat="server" />

        <p>Login:</p>
        <asp:TextBox ID="txtLogin" runat="server" />

        <p>CPF:</p>
        <asp:TextBox ID="txtCPF" runat="server" />

        <p>Perfil:</p>
        <asp:DropDownList ID="ddlPerfil" runat="server">
            <asp:ListItem Text="Usuário" Value="USU" />
            <asp:ListItem Text="Administrador" Value="ADM" />
        </asp:DropDownList>

        <p>
            <asp:Button ID="btnIncluir"
                runat="server"
                Text="Incluir"
                OnClick="btnIncluir_Click" />

            <asp:Button
                ID="btnAtualizar"
                runat="server"
                Text="Atualizar"
                OnClick="btnAtualizar_Click" />

            <asp:Button
                ID="btnExcluir"
                runat="server"
                Text="Excluir"
                OnClick="btnExcluir_Click"
                OnClientClick="return confirm('Deseja realmente excluir este usuário?');" />
        </p>

        <p>Senha inicial:</p>
        <asp:TextBox ID="txtSenhaInicial" runat="server" TextMode="Password" />

        <h3>Pesquisar usuários</h3>

            <asp:TextBox ID="txtPesquisa" runat="server"
                placeholder="Nome ou login" />

            <asp:Button ID="btnPesquisar" runat="server"
                Text="Pesquisar"
                OnClick="btnPesquisar_Click" />

          <asp:GridView
                ID="gvUsuarios"
                runat="server"
                AutoGenerateColumns="true"
                AutoGenerateSelectButton="true"
                DataKeyNames="Id"
                OnSelectedIndexChanged="gvUsuarios_SelectedIndexChanged" />


        <asp:HiddenField ID="hfUsuarioId" runat="server" />

        <asp:Label ID="lblMensagem" runat="server" />

    </form>
</body>
</html>
