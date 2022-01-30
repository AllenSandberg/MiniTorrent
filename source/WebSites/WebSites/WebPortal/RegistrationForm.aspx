<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistrationForm.aspx.cs" Inherits="RegistrationForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

</head>
<body style="background-image: url(Images/AfekaLogoBack.jpg); width: 100%; height: 100%; background-position: center; background-repeat: repeat-x; background-size: cover; background-color: forestgreen;">
    <img src="Images/logoeng.png" width="500" height="50" />
    <form id="form1" runat="server">
        <h1>
            <asp:Label ID="Label1" runat="server" Text="Afeka MiniTorrent Registration Form" Font-Bold="True" Font-Names="David" ForeColor="White"></asp:Label><br />
        </h1>

        <asp:Label ForeColor="White" runat="server">Username</asp:Label>
        <asp:TextBox ID="inputUsername" placeholder="Username" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="White" runat="server" ErrorMessage="Username required" ControlToValidate="inputUsername"></asp:RequiredFieldValidator>
        <br />
        <asp:Label ForeColor="White" runat="server">Password</asp:Label>
        <asp:TextBox ID="inputPassword" placeholder="Password" runat="server" />
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ForeColor="White" runat="server" ErrorMessage="Password required" ControlToValidate="inputPassword"></asp:RequiredFieldValidator>
        <br />
        <asp:Button runat="server" Text="Submit" OnClick="SubmitButton_Clicked" ID="SubmitButton" />
        <br />
        <asp:Label ID="LabelResult" ForeColor="White" runat="server" Text="Label" Visible="False"></asp:Label><br />
        <a href="HomePage.html">Back to homepage</a>
        <p>
            <asp:Image ID="TorrentImage" runat="server" Width="750px" ImageUrl="~/utorrent-logo-796x398.jpg" />
        </p>
    </form>
</body>
</html>
