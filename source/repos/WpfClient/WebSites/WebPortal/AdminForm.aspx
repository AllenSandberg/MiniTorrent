<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminForm.aspx.cs" Inherits="AdminForm" Theme="" %>

<!DOCTYPE html>
<script runat="server">

    protected void AdminGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
</script>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Administrator Page</title>
    <style type="text/css">
        #form1 {
            height: 304px;
            width: 546px;
        }
    </style>
</head>
<body style="background-image: url(Images/AfekaLogoBack.jpg); width: 100%; height: 100%; background-position: center; background-repeat: repeat-x; background-size: cover; background-color: forestgreen;">
    <img src="Images/logoeng.png" width="500" height="50" />
    <h1>
        <asp:Label ID="Label3" runat="server" Text="Afeka MiniTorrent Admin Form" Font-Bold="True" Font-Names="David" ForeColor="White"></asp:Label><br />
    </h1>
    <form id="form1" runat="server">
        <div>

            <%--d  
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
            --%>

            <asp:GridView ID="AdminGridView" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="4" Height="409px" Width="884px"
                OnSelectedIndexChanged="AdminGridView_SelectedIndexChanged" OnRowCommand="AdminGridView_RowCommand"
                OnRowEditing="AdminGridView_RowEditing" OnRowCancelingEdit="AdminGridView_RowCancelingEdit"
                OnRowUpdating="AdminGridView_RowUpdating" OnRowDataBound="AdminGridView_RowDataBound"
                OnRowDeleting="AdminGridView_RowDeleting">


                <FooterStyle BackColor="White" ForeColor="#000066" />
                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <RowStyle ForeColor="#000066" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                <SortedDescendingHeaderStyle BackColor="#00547E" />
                <Columns>
                    <asp:TemplateField HeaderText="ID">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Id") %>' runat="server" ID="lbl_id" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtId" Text='<%# Eval("Id") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtIdFooter" runat="server" />

                        </FooterTemplate>

                    </asp:TemplateField>



                    <asp:TemplateField HeaderText="Username">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Username") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtUsername" Text='<%# Eval("Username") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtUsernameFooter" runat="server" />

                        </FooterTemplate>

                    </asp:TemplateField>



                    <asp:TemplateField HeaderText="Enabled">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Enabled") %>' runat="server" ID="EnabledLabel" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="EnabledId" Text='<%# Eval("Enabled") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtEnabledFooter" runat="server" />

                        </FooterTemplate>

                    </asp:TemplateField>



                    <asp:TemplateField HeaderText="Connected">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Connected") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtConnected" Text='<%# Eval("Connected") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtConnectedFooter" runat="server" />

                        </FooterTemplate>

                    </asp:TemplateField>



                    <asp:TemplateField HeaderText="PortIn">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("PortIn") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtPortIn" Text='<%# Eval("PortIn") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtPortInFooter" runat="server" />

                        </FooterTemplate>

                    </asp:TemplateField>



                    <asp:TemplateField HeaderText="PortOut">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("PortOut") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtPortOut" Text='<%# Eval("PortOut") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtPortOutFooter" runat="server" />

                        </FooterTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="~/Images/enable.png" runat="server" CommandName="Enable" ToolTip="Enable" OnClick="EnableButton_Click" Width="40px" Height="40px" />
                            <asp:ImageButton ImageUrl="~/Images/disable.png" runat="server" CommandName="Disable" ToolTip="Disable" OnClick="DisableButton_Click" Width="40px" Height="40px" />
                            <asp:ImageButton ImageUrl="~/Images/delete.png" runat="server" CommandName="Delete" ToolTip="Delete" OnClick="DeleteButton_Click" Width="40px" Height="40px" />
                            <asp:ImageButton ImageUrl="~/Images/update.png" runat="server" CommandName="Update" ToolTip="Update" Width="40px" Height="40px" />
                            <asp:ImageButton ImageUrl="~/Images/cancel.png" runat="server" CommandName="CancelUpdate" ToolTip="CancelUpdate" Width="40px" Height="40px" />

                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:ImageButton ImageUrl="~/Images/enable.png" runat="server" CommandName="Enable" ToolTip="Enable" OnClick="EnableButton_Click" Width="40px" Height="40px" />
                            <asp:ImageButton ImageUrl="~/Images/delete.png" runat="server" CommandName="Delete" ToolTip="Delete" Width="40px" Height="40px" />
                            <asp:ImageButton ImageUrl="~/Images/cancel.png" runat="server" CommandName="CancelUpdate" ToolTip="Cancel" Width="40px" Height="40px" />

                        </EditItemTemplate>

                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:TemplateField>


                </Columns>
            </asp:GridView>

            <br />
            <br />





        </div>
        <a href="HomePage.html">Back to homepage</a>
        <p>
            &nbsp;
        </p>
    </form>
</body>
</html>
