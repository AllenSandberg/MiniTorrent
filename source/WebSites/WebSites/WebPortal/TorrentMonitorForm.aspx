<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TorrentMonitorForm.aspx.cs" Inherits="TorrentMonitorForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>TorrentMonitorForm Page</title>
    <!-- Load css styles -->
    <style type="text/css">
        .Main {
            background-color: white;
            margin-left: auto; /* Center mainTable for >= IE6  */
            margin-right: auto; /* Center mainTable for >= IE6  */
        }

        .LeftLabelColumn {
            display: block;
            margin-left: 10px;
            margin-right: 10px;
            height: 22px;
            padding-top: 5px;
            color: papayawhip;
        }

        .LeftValueColumn {
            display: block;
            margin-left: 10px;
            margin-right: 10px;
        }
    </style>

    <style type="text/css">
        .UnderLine {
            text-decoration: underline !important;
        }
    </style>
    <script type="text/javascript">
</script>
</head>




<body style="background-image: url(Images/AfekaLogoBack.jpg); width: 100%; height: 100%; background-position: center; background-repeat: repeat-x; background-size: cover; background-color: forestgreen;">
    <img src="Images/logoeng.png" width="500" height="50" />

    <h1>
        <asp:Label ID="Label2" runat="server" Text="Afeka MiniTorrent Monitor" Font-Bold="True" Font-Names="David" ForeColor="White"></asp:Label><br />
    </h1>

    <form id="form1" runat="server">



        <div>

            <asp:GridView ID="FileGridView" runat="server" AutoGenerateColumns="false" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">
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

                    <asp:TemplateField HeaderText="FileID">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("Id") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="FileID" Text='<%# Eval("Id") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtFileIDFooter" runat="server" />

                        </FooterTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="FileName">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("FileName") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtFileName" Text='<%# Eval("FileName") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtFileNameFooter" runat="server" />

                        </FooterTemplate>

                    </asp:TemplateField>



                    <asp:TemplateField HeaderText="FileSize">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("FileSize") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtFileSize" Text='<%# Eval("FileSize") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtFileSizeFooter" runat="server" />

                        </FooterTemplate>

                    </asp:TemplateField>






                    <asp:TemplateField HeaderText="DateAdded">
                        <ItemTemplate>
                            <asp:Label Text='<%# Eval("DateAdded") %>' runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDateAdded" Text='<%# Eval("DateAdded") %>' runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtDateAddedFooter" runat="server" />

                        </FooterTemplate>

                    </asp:TemplateField>



                    <asp:TemplateField HeaderText="TotalConnectedUsers">
                        <ItemTemplate>
                            <asp:Label Text="" runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtConnectedUsers" Text="" runat="server" />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtConnectedUsers" runat="server" />

                        </FooterTemplate>

                    </asp:TemplateField>


                </Columns>
            </asp:GridView>
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />

            <div style="float: left;">
                <asp:Label ID="SearchBy_FileID_lbl" CssClass="LeftLabelColumn" runat="server">Search_By_FileID</asp:Label>
                <br />
                <asp:Label ID="SearchBy_FileName_lbl" CssClass="LeftLabelColumn" runat="server">Search_By_FileName</asp:Label>
                <br />
                <asp:Label ID="SearchBy_FileSize_lbl" CssClass="LeftLabelColumn" runat="server">Search_By_FileSize</asp:Label>
                <br />
            </div>

            <div>
                <asp:TextBox ID="SearchBy_FileID_TB" CssClass="LeftValueColumn" runat="server" AutoPostBack="True" OnTextChanged="SearchBy_FileID_TB_TextChanged"></asp:TextBox>
                <br />

                <asp:TextBox ID="SearchBy_FileName_TB" CssClass="LeftValueColumn" runat="server" AutoPostBack="True" OnTextChanged="SearchBy_FileName_TB_TextChanged"></asp:TextBox>
                <br />

                <asp:TextBox ID="SearchBy_FileSize_TB" CssClass="LeftValueColumn" runat="server" AutoPostBack="True" OnTextChanged="SearchBy_FileSize_TB_TextChanged"></asp:TextBox>
                <br />


            </div>

            <br />
            <br />
            <asp:Label ID="SearchBy_TotalUsers_lbl" Style="text-decoration: underline;" CssClass="LeftLabelColumn" Text="Total AfekaMiniTorrent Users   " runat="server" />

            <asp:Label ID="TotalUsersResult" CssClass="LeftLabelColumn" runat="server" />



            <br />
            <br />
            <asp:Label ID="TotalActiveUsers" Style="text-decoration: underline;" CssClass="LeftLabelColumn" Text="Total AfekaMiniTorrent Active Users   " runat="server" />
            <asp:Label ID="TotalActiveUsersResult" CssClass="LeftLabelColumn" Text="" runat="server" />

            <br />
            <br />
            <asp:Label ID="TotalFiles" Style="text-decoration: underline;" CssClass="LeftLabelColumn " Text="Total AfekaMiniTorrent Files :  " runat="server" />
            <asp:Label ID="TotalFilesResult" CssClass="LeftLabelColumn" Text="" runat="server" />

        </div>
        <a href="HomePage.html">Back to homepage</a>
    </form>
</body>
</html>
