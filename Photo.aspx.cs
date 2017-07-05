using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Photo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["PhotoId"] == null || Request.QueryString["PhotoId"] == "")
        {
            Response.Redirect("Home.aspx");
        }

        String photoId = Request.QueryString["PhotoId"];

        if(Request.IsAuthenticated)
        {
            Control addComment = FindHtmlControlByIdInControl(this, "AddComment");
            addComment.Visible = true;
        }
        else
        {
            Control addComment = FindHtmlControlByIdInControl(this, "AddComment");
            addComment.Visible = false;
        }

        SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        SqlCommand cmD = new SqlCommand();

        cmD.CommandText = "SELECT PhotoName FROM Photos WHERE Id = @PhotoId";
        cmD.Parameters.Add("@PhotoId", SqlDbType.Int).Value = photoId;
        cmD.CommandType = CommandType.Text;
        cmD.Connection = sqlConnection;

        sqlConnection.Open();

        SqlDataReader reader1;
        reader1 = cmD.ExecuteReader();
        if (reader1.HasRows)
        {
            reader1.Read();
            string photoName = reader1["PhotoName"].ToString();
            PhotoImage.ImageUrl = "~/Photos/" + photoName;
        }

        sqlConnection.Close();
    }


    private HtmlControl FindHtmlControlByIdInControl(Control control, string id)
    {
        foreach (Control childControl in control.Controls)
        {
            if (childControl.ID != null && childControl.ID.Equals(id, StringComparison.OrdinalIgnoreCase) && childControl is HtmlControl)
            {
                return (HtmlControl)childControl;
            }

            if (childControl.HasControls())
            {
                HtmlControl result = FindHtmlControlByIdInControl(childControl, id);
                if (result != null) return result;
            }
        }

        return null;
    }

    protected void CommentsDataSource_OnSelecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters[0].Value = Request.QueryString["PhotoId"];
    }

    protected void SendButton_Click(object sender, EventArgs e)
    {
        if (CommentTextBox.Text.Length <= 0)
            return;

        SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        SqlCommand cmD = new SqlCommand();

        cmD.CommandText = "INSERT INTO Comments(PhotoId, UserName, CommentMessage, Date) VALUES (@PhotoId, @UserName, @CommentMessage, @Date)";
        cmD.Parameters.Add("@PhotoId", SqlDbType.Int).Value = Request.QueryString["PhotoId"];
        cmD.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = Membership.GetUser().UserName;
        cmD.Parameters.Add("@CommentMessage", SqlDbType.NVarChar, 50).Value = CommentTextBox.Text;
        cmD.Parameters.Add("@Date", SqlDbType.DateTime).Value = DateTime.Now.ToString();

        cmD.CommandType = CommandType.Text;
        cmD.Connection = sqlConnection;

        sqlConnection.Open();
        cmD.ExecuteNonQuery();
        cmD.Dispose();
        sqlConnection.Close();

        Repeater1.DataBind();
        Response.Redirect(Request.RawUrl);
    }

    protected Boolean HasPermission()
    {
        if (Roles.IsUserInRole("Admin"))
        {
            return true;
        }
        SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        SqlCommand cmD = new SqlCommand();

        cmD.CommandText = "Select UserName From Albums, Photos Where Photos.Id = @PhotoId AND Albums.Id = Photos.AlbumId";
        cmD.Parameters.Add("@PhotoId", SqlDbType.Int).Value = Request.QueryString["PhotoId"];

        cmD.CommandType = CommandType.Text;
        cmD.Connection = sqlConnection;

        sqlConnection.Open();
        SqlDataReader reader = cmD.ExecuteReader();
        if(reader.HasRows)
        {
            reader.Read();
            string userName = reader["UserName"].ToString();
            if(Request.IsAuthenticated && Membership.GetUser().UserName == userName)
            {
                return true;
            }
        }
        sqlConnection.Close();

        return false;
    }


    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "Remove")
            return;
        
        SqlConnection sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        SqlCommand cmd = new SqlCommand();

        cmd.CommandText = "Delete From Comments Where Id = @Id";
        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = int.Parse(e.CommandArgument.ToString());
        cmd.Connection = sqlcon;
        sqlcon.Open();
        cmd.ExecuteNonQuery();
        sqlcon.Close();

        Repeater1.DataBind();
    }
}