<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistrationForm.aspx.cs" Inherits="RegistrationForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <h1>Register</h1>
        <asp:Label runat="server">Username</asp:Label>
        <asp:TextBox id="inputUsername" placeholder="Username" runat="server"/>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Username required" ControlToValidate="inputUsername"></asp:RequiredFieldValidator>
        <br />
        <asp:Label runat="server">Password</asp:Label>
        <asp:TextBox id="inputPassword" placeholder="Password" runat="server"/>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Password required" ControlToValidate="inputPassword"></asp:RequiredFieldValidator>
        <br />
        <asp:Button runat="server" Text="Submit" OnClick="Unnamed3_Click"/>
    </form>
    <asp:Label ID="LabelResult" runat="server" Text="Label" Visible="False"></asp:Label><br />
    <a href="HomePage.html">Back to homepage</a>
</body>
</html>
