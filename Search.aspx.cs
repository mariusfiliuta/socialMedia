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

public partial class Search : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void SearchTextBox_TextChanged(object sender, EventArgs e)
    {
        Session["shearchedName"] = SearchTextBox.Text;
        Repeater1.DataBind();
    }
    protected void ItemRepeaterClicked(object sender, CommandEventArgs e)
    {
        if (e.CommandName != "AccessProfile")
            return;
        string userName = e.CommandArgument.ToString();
        SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        SqlCommand cmd = new SqlCommand();
        SqlDataReader reader;

        cmd.CommandText = "SELECT ProfilePublic FROM Users WHERE UserName = @UName";
        cmd.Parameters.Add("@UName", SqlDbType.NVarChar, 50).Value = userName;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = sqlConnection1;

        sqlConnection1.Open();

        reader = cmd.ExecuteReader();
        if (reader.HasRows)
        {
            reader.Read();
            bool isPublic = reader["ProfilePublic"].ToString() == "True" ? true : false;
            if(isPublic)
            {
                Response.Redirect("UserPage.aspx?UserId=" + userName);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('That Profile is PRIVATE!')", true);
            }
        }

        sqlConnection1.Close();
    }
}