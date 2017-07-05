using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FriendsRequests : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Request.IsAuthenticated)
        {
            Response.Redirect("Home.aspx");
        }
    }

    protected void SqlDbSelecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters[0].Value = Membership.GetUser().UserName;

    }

    protected void ItemRepeaterClicked(object sender, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "AcceptRequest":
                {
                    SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "Update Friends Set Status = @Status WHERE FromUser = @FromUser AND ToUser = @ToUser";
                    cmd.Parameters.Add("@FromUser", SqlDbType.NVarChar, 50).Value = e.CommandArgument.ToString();
                    cmd.Parameters.Add("@ToUser", SqlDbType.NVarChar, 50).Value = Membership.GetUser().UserName;
                    cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = "Accepted";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = sqlConnection1;

                    sqlConnection1.Open();

                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "INSERT INTO Friends(FromUser,ToUSer,Status) VALUES (@ToUser,@FromUser,'Accepted')";
                    cmd.ExecuteNonQuery();

                    sqlConnection1.Close();
                    Repeater1.DataBind();
                    break;
                }
            case "DenyRequest":
                {
                    SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "Update Friends Set Status = @Status WHERE FromUser = @FromUser AND ToUser = @ToUser";
                    cmd.Parameters.Add("@FromUser", SqlDbType.NVarChar, 50).Value = e.CommandArgument.ToString();
                    cmd.Parameters.Add("@ToUser", SqlDbType.NVarChar, 50).Value = Membership.GetUser().UserName;
                    cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 50).Value = "Denied";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = sqlConnection1;

                    sqlConnection1.Open();

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();

                    sqlConnection1.Close();

                    Repeater1.DataBind();
                    break;
                }
            default: break;
        }
    }
}