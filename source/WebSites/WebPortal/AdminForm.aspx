<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminForm.aspx.cs" Inherits="AdminForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Table ID="TableAllUsers" runat="server">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>ID</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Username</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Enabled</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Connected</asp:TableHeaderCell>
                    <asp:TableHeaderCell>PortIn</asp:TableHeaderCell>
                    <asp:TableHeaderCell>PortOut</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </form>
    <a href="HomePage.html">Back to homepage</a>
</body>
</html>
