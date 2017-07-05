using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class GeneralItems : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.IsAuthenticated)
        {
            LogInOutButton.Text = "LogOut";
            ProfileButton.Visible = true;
            WelcomeMessage.Text = "You are conneted as " + Membership.GetUser().UserName;
        }
        else
        {
            LogInOutButton.Text = "LogIn";
            ProfileButton.Visible = false;
            WelcomeMessage.Text = "Your are not conencted";
        }
    }
    protected void LogInOutButton_Click(object sender, EventArgs e)
    {
        if (Request.IsAuthenticated)
            FormsAuthentication.SignOut();
        Response.Redirect("login.aspx");
    }

    protected void ProfileButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("UserPage.aspx");
    }

    protected void HomeButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }

    protected void SearchButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("Search.aspx");
    }
}
