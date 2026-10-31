<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="LivroCaixa.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Livro Caixa</title>
</head>
<body>
    <form id="form1" runat="server">
            <h2>Livro Caixa</h2>

        <asp:Panel ID="pnLogin" runat="server">
            <h3>Indentificação do usúario</h3>

              <asp:label
                 ID="lblNome"
                 runat="server"
                 Text="Login:" />
               <asp:TextBox
                 ID="txtNome"
                 runat="server" />
              <asp:Button
                 ID="btnEntrar"
                 runat="server"
                 Text="Entrar"
                 OnClick="btnEntrar_Click" />
            <br /> 
              <asp:label
                 ID="lblSenha"
                 runat="server"
                 Text="Senha:" />
             <asp:TextBox
                  ID="txtSenha"
                  runat="server"
                  TextMode="Password"/>
            </asp:Panel>

               <asp:Panel
                 ID="pnMenu"
                 runat="server" 
                 Visible="false">

        <h3>Menu Principal</h3>
        <p>Bem-vindo ao Livro Caixa!</p>
        <p>Cadastro do Usuário</p>
        <p>Fluxo de caixa</p>
        <asp:Button
            ID="btnSair"
            runat="server"
            Text="Sair"
            OnClick="btnSair_Click" /> 
        </asp:Panel> 

        <asp:Label ID="lblMensagem"
            runat="server" />
        <div>
        </div>
    </form>
</body>
</html>
