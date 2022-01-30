using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using WebPortal.WebPortalServiceLayer;

/// <summary>
/// Summary description for BasePage
/// </summary>
public class BasePage : System.Web.UI.Page
{
   // protected readonly IWebPortalServices proxy;

    public BasePage()
    {

        this.Load += new EventHandler(Page_Load);
        this.Init += new EventHandler(Page_Load);
     //   proxy = new ChannelFactory<IWebPortalServices>("WebPortalServicesEndpoint").CreateChannel();


    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageLoadEvent(sender, e);
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        PageInitEvent(sender, e);
    }

    virtual protected void PageLoadEvent(object sender, EventArgs e)
    { }


    virtual protected void PageInitEvent(object sender, EventArgs e)
    { }

    private void InitWebService()
    {

    }





}