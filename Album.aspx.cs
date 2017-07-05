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

public partial class Album : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (Request.QueryString["AlbumId"] == null || Request.QueryString["AlbumId"] == "")
        {
            Response.Redirect("Home.aspx");
        }

        String albumId = Request.QueryString["AlbumId"];

        String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg" };
        // On Change Profile Picture
        if (Page.IsPostBack && PictureUpload.HasFile)
        {
            Boolean fileOK = false;
            String fileExtension = "";
            if (PictureUpload.HasFile)
            {
                fileExtension = System.IO.Path.GetExtension(PictureUpload.FileName).ToLower();
                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        fileOK = true;
                    }
                }
            }
            if (fileOK)
            {
                try
                {
                    SqlConnection sqlConnection2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                    SqlCommand cmd1 = new SqlCommand();

                    cmd1.CommandText = "Insert INTO Photos(AlbumId, PhotoName) VALUES (@AlbumId, @PhotoName)";
                    cmd1.Parameters.Add("@AlbumId", SqlDbType.Int).Value = albumId;
                    cmd1.Parameters.Add("@PhotoName", SqlDbType.NVarChar, 200).Value = Membership.GetUser().UserName.ToString() + PictureUpload.FileName;
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Connection = sqlConnection2;

                    sqlConnection2.Open();

                    cmd1.ExecuteNonQuery();
                    cmd1.Dispose();

                    sqlConnection2.Close();

                    PictureUpload.PostedFile.SaveAs(Server.MapPath("~/Photos/") + Membership.GetUser().UserName.ToString() + PictureUpload.FileName.ToString());
                    Repeater1.DataBind();
                    
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
            }
        }

        SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        SqlCommand cmD = new SqlCommand();

        cmD.CommandText = "SELECT UserName FROM Albums WHERE Id = @AlbumId";
        cmD.Parameters.Add("@AlbumId", SqlDbType.Int).Value = albumId;
        cmD.CommandType = CommandType.Text;
        cmD.Connection = sqlConnection;

        sqlConnection.Open();

        SqlDataReader reader1;
        reader1 = cmD.ExecuteReader();
        if (reader1.HasRows)
        {
            reader1.Read();
            string userName = reader1["UserName"].ToString();
            if(Request.IsAuthenticated && Membership.GetUser().UserName == userName)
            {
                PictureUpload.Visible = true;
            }
        }

        sqlConnection.Close();
    }


    protected void SqlDbSelecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        e.Command.Parameters[0].Value = Request.QueryString["AlbumId"];
    }

    protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "ImageButton" && e.CommandName != "RemoveImage")
            return;

        if(e.CommandName == "RemoveImage")
        {
            SqlConnection sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "Delete From Comments Where PhotoId = @Id";
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = int.Parse(e.CommandArgument.ToString());
            cmd.Connection = sqlcon;
            sqlcon.Open();
            cmd.ExecuteNonQuery();
            cmd.CommandText = "Delete From Photos Where Id = @Id";
            cmd.ExecuteNonQuery();
            sqlcon.Close();
            Repeater1.DataBind();
        }
        else
            Response.Redirect("Photo.aspx?PhotoId=" + e.CommandArgument);
    }


    protected Boolean HasPermission()
    {
        if (Roles.IsUserInRole("Admin"))
        {
            return true;
        }
        SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        SqlCommand cmD = new SqlCommand();

        cmD.CommandText = "Select UserName From Albums Where Albums.Id = @AlbumId";
        cmD.Parameters.Add("@AlbumId", SqlDbType.Int).Value = Request.QueryString["AlbumId"];

        cmD.CommandType = CommandType.Text;
        cmD.Connection = sqlConnection;

        sqlConnection.Open();
        SqlDataReader reader = cmD.ExecuteReader();
        if (reader.HasRows)
        {
            reader.Read();
            string userName = reader["UserName"].ToString();
            if (Request.IsAuthenticated && Membership.GetUser().UserName == userName)
            {
                return true;
            }
        }
        sqlConnection.Close();

        return false;
    }
}