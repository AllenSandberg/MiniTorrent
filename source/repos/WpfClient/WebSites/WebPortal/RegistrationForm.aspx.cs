using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TorrentDataAccessLayer;

public partial class RegistrationForm : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        base.PageLoadEvent(sender, e);
        TorrentImage.Visible = true;

    }


    protected void SubmitButton_Clicked(object sender, EventArgs e)
    {
        string username = inputUsername.Text.Trim();
        string password = inputPassword.Text;
        int result = ValidateUserDetails(username, password);
        LabelResult.Visible = true;
        if (result == 0)
        {
            User newUser = DataAccessLayer.Register(username, password);
            if (newUser == null)
                LabelResult.Text = Utility.Util.EXISTINGUSER_ERROR;
            else
            {

                // form1.Visible = false;
                LabelResult.Text = "Registered as: " + newUser.Username + " (UserID = " + newUser.Id + ")";
            }

        }
        else
        {
            switch (result)
            {
                case Utility.Util.EMPTY_USERNAME:
                    LabelResult.Text = Utility.Util.EMPTY_USERNAME_ERROR;
                    break;
                case Utility.Util.EMPTY_PASSWORD:
                    LabelResult.Text = Utility.Util.EMPTY_PASSWORD_ERROR;
                    break;
                case Utility.Util.PASSWORD_SHORTLENGTH:
                    LabelResult.Text = Utility.Util.PASSWORD_SHORTLENGTH_ERROR;
                    break;
                case Utility.Util.PASSWORD_SPACE:
                    LabelResult.Text = Utility.Util.PASSWORD_SPACE_ERROR;
                    break;
                case Utility.Util.USERNAME_ALLDIGITS:
                    LabelResult.Text = Utility.Util.USERNAME_ALLDIGITS_ERROR;
                    break;
                default:
                    break;

            }

        }



    }


    public int ValidateUserDetails(string username, string password)
    {

        if (username.Equals(""))
            return Utility.Util.EMPTY_USERNAME;
        else if (password.Equals(""))
            return Utility.Util.EMPTY_PASSWORD;
        else if (password.Length <= 2)
            return Utility.Util.PASSWORD_SHORTLENGTH;
        else if (password.Contains(" "))
            return Utility.Util.PASSWORD_SPACE;
        else if (username.All(char.IsDigit))
            return Utility.Util.USERNAME_ALLDIGITS;
        else
            return 0;
    }



}