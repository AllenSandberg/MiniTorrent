using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TorrentDataAccessLayer;

public partial class RegistrationForm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Unnamed3_Click(object sender, EventArgs e)
    {
        string username = inputUsername.Text.Trim();
        string password = inputPassword.Text;
        User newUser = DataAccessLayer.Register(username, password);

        if (newUser != null)
        {
            form1.Visible = false;
            LabelResult.Text = "Registered as: " + newUser.Username + " (UserID = " + newUser.Id + ")";
        }
        else
        {
            LabelResult.Text = "Registration Error: The username: " + username + " already exists.";
        }
        LabelResult.Visible = true;
    }
}