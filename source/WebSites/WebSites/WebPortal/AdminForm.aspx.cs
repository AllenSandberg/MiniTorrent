using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TorrentDataAccessLayer;


public partial class AdminForm : BasePage
{
    /*
IsPostBack== false - Means it works for first time form load.it does not work when button is clicked.

IsPostBack== true - Means it does not work for first time form load. It works when button is clicked.

     */

    protected override void PageLoadEvent(object sender, EventArgs e)
    {
        // pageloginrequired = false
        base.PageLoadEvent(sender, e);
        PopulateGridView(true);
        //this.Visible = true;

        //  UpdateAllUsersTable();
    }

    /*
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
            row.Cells.Add(new TableCell { Text = u.PortOut.ToString() });
            TableAllUsers.Rows.Add(row);
        }

    }
    */


    protected void PopulateGridView(bool firstload)
    {
        if (firstload)
        {
            if (!Page.IsPostBack)
            {
                List<User> users = DataAccessLayer.GetAllUsers(false);
                if (users == null)
                {
                    AdminGridView.Rows[0].Cells[0].ColumnSpan = AdminGridView.Columns.Count;
                    AdminGridView.Rows[0].Cells[0].Text = Utility.Util.NODATAFOUNDERROR;
                    AdminGridView.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;

                }

                AdminGridView.DataSource = users;
                AdminGridView.DataBind();

            }
        }

        else
        {
            AdminGridView.DataSource = null;
            List<User> users = DataAccessLayer.GetAllUsers(false);
            if (users == null)
            {
                AdminGridView.Rows[0].Cells[0].ColumnSpan = AdminGridView.Columns.Count;
                AdminGridView.Rows[0].Cells[0].Text = "No Data Found ! ";
                AdminGridView.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;

            }

            AdminGridView.DataSource = users;
            AdminGridView.DataBind();
        }


    }

    protected void EnableButton_Click(object sender, ImageClickEventArgs e)
    {

        //Get the button that raised the event
        ImageButton btn = (ImageButton)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;

        int index = gvr.RowIndex;
        string id_text = ((Label)AdminGridView.Rows[gvr.RowIndex].Cells[0].FindControl("lbl_id")).Text;
        int id = int.Parse(id_text);


        User user = DataAccessLayer.GetUserDetails(id);
        if (DataAccessLayer.EnableUser(user.Username))
            AdminGridView.Rows[index].Cells[2].Text = "True";

        else
            AdminGridView.Rows[index].Cells[2].Text = "False";
    }

    protected void DisableButton_Click(object sender, ImageClickEventArgs e)
    {
        //Get the button that raised the event
        ImageButton btn = (ImageButton)sender;

        //Get the row that contains this button
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        int index = gvr.RowIndex;

        string id_text = ((Label)AdminGridView.Rows[gvr.RowIndex].Cells[0].FindControl("lbl_id")).Text;
        int id_user = int.Parse(id_text);

        User user = DataAccessLayer.GetUserDetails(id_user);
        string username = user.Username;

        if (DataAccessLayer.DisableUser(username))
            AdminGridView.Rows[index].Cells[2].Text = "False";

        else
            AdminGridView.Rows[index].Cells[2].Text = "True";

    }

    protected void DeleteButton_Click(object sender, ImageClickEventArgs e)
    {
        string message = "User has been successfully deleted ";
        //Get the button that raised the event
        ImageButton btn = (ImageButton)sender;
        //Get the row that contains this button
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        int index = gvr.RowIndex;
        string id_text = ((Label)AdminGridView.Rows[gvr.RowIndex].Cells[0].FindControl("lbl_id")).Text;
        int id_user = int.Parse(id_text);
        User user = DataAccessLayer.GetUserDetails(id_user);
        string username = user.Username;
        if (DataAccessLayer.DeleteUser(username))
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + message + "');", true);

        else
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + message + "');", true);
    }

    protected void AdminGridView_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        /*
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Here you can pick any value using cell no, Cell numbers start from 0 to onwards
            /*
            string Id = e.Row.Cells[0].Text;
            string Username = e.Row.Cells[1].Text;
            string Password = e.Row.Cells[2].Text;
            string Enabled = e.Row.Cells[3].Text;
            string Connected = e.Row.Cells[4].Text;
            string PortIn = e.Row.Cells[5].Text;
            string PortOut = e.Row.Cells[6].Text;
            }
            */

    }



    protected void AdminGridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        //base.PageLoadEvent(sender, e);
    }


    protected void AdminGridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        PopulateGridView(false);
       // UpdateAllUsersTable();
    }

    protected void AdminGridView_RowEditing(object sender, GridViewEditEventArgs e)
    {
        AdminGridView.EditIndex = e.NewEditIndex;
        PopulateGridView(false);
    }

    protected void AdminGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {

    }

    protected void AdminGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        PopulateGridView(false);
    }


    protected void AdminGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        PopulateGridView(false);
        //UpdateAllUsersTable();

    }
}
