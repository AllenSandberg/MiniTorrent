using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TorrentDataAccessLayer;

public partial class TorrentMonitorForm : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        base.PageLoadEvent(sender, e);
        PopulateGridView(true);
    }


    protected void PopulateGridView(bool firstLoad)
    {
        if (firstLoad)
        {
            if (!Page.IsPostBack)
            {

                List<File> allfilesList = DataAccessLayer.GetFiles(false);
                if (allfilesList == null)
                    No_Data_Found();

                FileGridView.DataSource = allfilesList;
                FileGridView.DataBind();
            }
        }

        else
        {
            FileGridView.DataSource = null;


            List<File> allfilesList = DataAccessLayer.GetFiles(false);
            if (allfilesList == null)
                No_Data_Found();


            FileGridView.DataSource = allfilesList;
            FileGridView.DataBind();
        }

        Populate_TotalConnectedUsers_Column();
        PopulateTableResults();
    }

    protected void No_Data_Found()
    {
        FileGridView.Rows[0].Cells[0].ColumnSpan = FileGridView.Columns.Count;
        FileGridView.Rows[0].Cells[0].Text = Utility.Util.NODATAFOUNDERROR;
        FileGridView.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    }


    protected void Populate_TotalConnectedUsers_Column()
    {
        foreach (GridViewRow row in FileGridView.Rows)
            row.Cells[4].Text = DataAccessLayer.CountFileResources(row.Cells[1].Text).ToString();

    }


    protected void PopulateTableResults()
    {
        TotalUsersResult.Text = DataAccessLayer.GetUsersCount(false).ToString();
        TotalActiveUsersResult.Text = DataAccessLayer.GetUsersCount(true).ToString();
        TotalFilesResult.Text = DataAccessLayer.GetFilesCount().ToString();
        TotalUsersResult.Visible = true;
        TotalFilesResult.Visible = true;
        TotalActiveUsersResult.Visible = true;

    }


    protected void SearchBy_FileID_TB_TextChanged(object sender, EventArgs e)
    {
        File f = null;

        string file_id = SearchBy_FileID_TB.Text;
        f = DataAccessLayer.FileExists(file_id);
        List<File> file = new List<File> { f };

        if (file.Count == 1 && file.First() != null)
        {

            FileGridView.DataSource = null;
            FileGridView.DataSource = file;
            FileGridView.DataBind();
            Populate_TotalConnectedUsers_Column();


        }
        else
            PopulateGridView(false);

    }


    protected void SearchBy_FileName_TB_TextChanged(object sender, EventArgs e)
    {

        string file_name = SearchBy_FileName_TB.Text;
        List<File> file = DataAccessLayer.GetFiles(false, file_name);


        if (file.Count > 0)
        {
            FileGridView.DataSource = null;
            FileGridView.DataSource = file;
            FileGridView.DataBind();
            Populate_TotalConnectedUsers_Column();
        }
        else
            PopulateGridView(false);
    }

    protected void SearchBy_FileSize_TB_TextChanged(object sender, EventArgs e)
    {
        string file_size = SearchBy_FileSize_TB.Text;
        List<File> file = DataAccessLayer.GetFiles(false, "", file_size, "");


        if (file.Count > 0)
        {
            FileGridView.DataSource = null;
            FileGridView.DataSource = file;
            FileGridView.DataBind();
            Populate_TotalConnectedUsers_Column();
        }
        else
            PopulateGridView(false);

    }


    /*
    protected void SearchBy_DateAdded_TB_TextChanged(object sender, EventArgs e)
    {

        string file_date = SearchBy_DateAdded_TB.Text;
        List<File> file = DataAccessLayer.GetFiles(false, "", "", file_date);


        if (file.Count > 0)
        {
            FileGridView.DataSource = null;
            FileGridView.DataSource = file;
            FileGridView.DataBind();
            Populate_TotalConnectedUsers_Column();
        }
        else
            PopulateGridView(false);

    }
    */
}