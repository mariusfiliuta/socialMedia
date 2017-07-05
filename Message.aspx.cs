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

public partial class Message : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!Request.IsAuthenticated || Request.QueryString["Friend"] == "" || Request.QueryString["Friend"] == null)
        {
            Response.Redirect("Home.aspx");
        }
    }

    protected void SqlDbSelecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters[0].Value = Membership.GetUser().UserName;
        e.Command.Parameters[1].Value = Request.QueryString["Friend"];
    }

    protected void SendMessageButton_Click(object sender, EventArgs e)
    {
        if (MessageTextBox.Text.Length <= 0)
            return;
        String userName = Request.QueryString["Friend"];
        String currentUserName = "";
        if (Request.IsAuthenticated)
            currentUserName = Membership.GetUser().UserName;
        String profileUserName = currentUserName;
        if (userName != null && !userName.Equals(""))
            profileUserName = userName;

        SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO Messages(FromUser,ToUSer,Message,Date) VALUES (@FromUser,@ToUser,@Message,@Date)";
        cmd.Parameters.Add("@FromUser", SqlDbType.NVarChar, 50).Value = Membership.GetUser().UserName;
        cmd.Parameters.Add("@ToUser", SqlDbType.NVarChar, 50).Value = profileUserName;
        cmd.Parameters.Add("@Message", SqlDbType.NVarChar, 50).Value = MessageTextBox.Text;
        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = DateTime.Now.ToString();
        cmd.CommandType = CommandType.Text;
        cmd.Connection = sqlConnection1;

        sqlConnection1.Open();

        cmd.ExecuteNonQuery();
        cmd.Dispose();

        sqlConnection1.Close();

        Repeater1.DataBind();
        Response.Redirect(Request.RawUrl);
    }
}