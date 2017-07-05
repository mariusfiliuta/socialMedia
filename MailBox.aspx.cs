using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MailBox : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void SqlDbSelecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters[0].Value = Membership.GetUser().UserName;
    }

    protected void ItemRepeaterClicked(object sender, CommandEventArgs e)
    {
        if (e.CommandName != "AccessMessages")
            return;
        string userName = e.CommandArgument.ToString();
        Response.Redirect("Message.aspx?Friend=" + userName);
    }
}