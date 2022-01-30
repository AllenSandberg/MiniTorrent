using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TorrentDataAccessLayer;

public partial class AdminForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UpdateAllUsersTable();
    }

    private void UpdateAllUsersTable()
    {
        // Clearing the table
        TableAllUsers.Rows.Clear();

        // Add header
        TableHeaderRow header = new TableHeaderRow();
        header.Cells.Add(new TableCell { Text = "ID" });
        header.Cells.Add(new TableCell { Text = "Username" });
        header.Cells.Add(new TableCell { Text = "Enabled" });
        header.Cells.Add(new TableCell { Text = "Connected" });
        header.Cells.Add(new TableCell { Text = "PortIn" });
        header.Cells.Add(new TableCell { Text = "PortOut" });
        TableAllUsers.Rows.Add(header);

        // Add rows
        foreach (User u in DataAccessLayer.GetAllUsers(false))
        {
            TableRow row = new TableRow();
            row.Cells.Add(new TableCell { Text = u.Id.ToString() });
            row.Cells.Add(new TableCell { Text = u.Username.ToString() });
            row.Cells.Add(new TableCell { Text = u.Enabled.ToString() });
            row.Cells.Add(new TableCell { Text = u.Connected.ToString() });
            row.Cells.Add(new TableCell { Text = u.PortIn.ToString() });

            TableCell tableCell = new TableCell();
            tableCell.Controls.Add(new Button());
            row.Cells.Add(tableCell);
            //row.Cells.Add(new TableCell { Text = u.PortOut.ToString() });
            TableAllUsers.Rows.Add(row);
        }
    }
}