using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class UserPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        String userName = Request.QueryString["UserId"];

        // Check if it has permission to access this page
        if (!Request.IsAuthenticated && (userName == null || userName.Equals("")))
        {
            Response.Redirect("Home.aspx");
        }
        
        //Usefull stuff
        String[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg" };
        String path = Server.MapPath("~/ProfilePics/");
        String currentUserName = "";
        if (Request.IsAuthenticated)
        {
            currentUserName = Membership.GetUser().UserName;
        }
        String profileUserName = currentUserName;
        if(userName != null && !userName.Equals(""))
        {
            profileUserName = userName;
        }


        // On Change Profile Picture
        if (Page.IsPostBack && ProfilePictureUpload.HasFile)
        {
            Boolean fileOK = false;
            String fileExtension = "";
            if (ProfilePictureUpload.HasFile)
            {
                fileExtension = System.IO.Path.GetExtension(ProfilePictureUpload.FileName).ToLower();
                for (int i = 0; i < allowedExtensions.Length; i++)
                {
                    if (fileExtension == allowedExtensions[i])
                    {
                        fileOK = true;
                    }
                }
            }
            ProfileLabel.Visible = true;
            if (fileOK)
            {
                try
                {
                    SqlConnection sqlConnection2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                    SqlCommand cmd1 = new SqlCommand();

                    cmd1.CommandText = "Update Users Set ProfilePicture = @ProfilePicture WHERE UserName = @UName";
                    cmd1.Parameters.Add("@UName", SqlDbType.NVarChar, 50).Value = Membership.GetUser().UserName;
                    cmd1.Parameters.Add("@ProfilePicture", SqlDbType.NVarChar, 50).Value = Membership.GetUser().UserName.ToString() + fileExtension;
                    cmd1.CommandType = CommandType.Text;
                    cmd1.Connection = sqlConnection2;

                    sqlConnection2.Open();

                    cmd1.ExecuteNonQuery();
                    cmd1.Dispose();

                    sqlConnection2.Close();

                    if(File.Exists(path + Membership.GetUser().UserName.ToString() + fileExtension))
                            File.Delete(path + Membership.GetUser().UserName.ToString() + fileExtension);
                    ProfilePictureUpload.PostedFile.SaveAs(path + Membership.GetUser().UserName.ToString() + fileExtension);

                    ProfileLabel.Text = "Profile Picture Changed";
                    ProfileLabel.ForeColor = System.Drawing.Color.White;
                }
                catch (Exception ex)
                {
                    ProfileLabel.Text = "Image could not be uploaded.";
                }
            }
            else
            {
                ProfileLabel.Text = "Cannot accept files of this type.";
            }
        }

        // Setting the Appearance of the Profile depending on the user

        // Setting the Profile Picture
        SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        SqlCommand cmD = new SqlCommand();

        cmD.CommandText = "SELECT ProfilePicture FROM Users WHERE UserName = @UName";
        cmD.Parameters.Add("@UName", SqlDbType.NVarChar, 50).Value = profileUserName;
        cmD.CommandType = CommandType.Text;
        cmD.Connection = sqlConnection;

        sqlConnection.Open();

        SqlDataReader reader1;
        reader1 = cmD.ExecuteReader();
        if (reader1.HasRows)
        {
            reader1.Read();
            string pic = reader1["ProfilePicture"].ToString();
            ProfilePicture.ImageUrl = "ProfilePics/" + pic;
        }

        sqlConnection.Close();

        
        // Allowing to change the profile picture if it's his own profile
        ProfilePictureUpload.Visible = (profileUserName == currentUserName);

        // Allowing to change the profile visibility if it's his own profile
        PublicProfileCheckBox.Visible = (profileUserName == currentUserName);
        PublicProfileLabel.Visible = (profileUserName == currentUserName);

        // Change things if it's his own profile
        if (profileUserName == currentUserName)
        {

            // Allowing to access profileButtons

            SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT ProfilePublic FROM Users WHERE UserName = @UName";
            cmd.Parameters.Add("@UName", SqlDbType.NVarChar, 50).Value = Membership.GetUser().UserName;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            reader = cmd.ExecuteReader();
            if(reader.HasRows)
            {
                reader.Read();
                bool isPublic = reader["ProfilePublic"].ToString() == "True" ? true:false;
                PublicProfileCheckBox.Checked = isPublic;
            }

            sqlConnection1.Close();

            // Change Text to Profile Buttons
            AddFriendButton.Text = "Friend Requests";
            SendMessageButton.Text = "Mail Box";
        }
        else
        {

            // Change Text to Profile Buttons

            SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT Status FROM Friends WHERE (FromUser = @CurrentUserName AND ToUser = @ProfileUserName)";
            cmd.Parameters.Add("@CurrentUserName", SqlDbType.NVarChar, 50).Value = currentUserName;
            cmd.Parameters.Add("@ProfileUserName", SqlDbType.NVarChar, 50).Value = profileUserName;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                switch (reader["Status"].ToString()) {
                    case "Pending":
                        {
                            AddFriendButton.Enabled = false;
                            AddFriendButton.Text = "Friendship Request Sent";
                            SendMessageButton.Text = "Send Message";
                            break;
                        }
                    case "Accepted":
                        {
                            AddFriendButton.Enabled = false;
                            AddFriendButton.Text = "You are friends!";
                            SendMessageButton.Text = "Send Message";
                            break;
                        }
                    case "Denied":
                        {

                            reader.Close();
                            AddFriendButton.Enabled = false;
                            AddFriendButton.Text = "He denied your request"; 
                            SendMessageButton.Text = "Send Message";
                            cmd.CommandText = "DELETE FROM Friends WHERE (FromUser = @CurrentUserName AND ToUser = @ProfileUserName)";
                            cmd.ExecuteNonQuery();
                            break;
                        }
                    default: {

                            AddFriendButton.Enabled = true;
                            AddFriendButton.Text = "Add friend";
                            SendMessageButton.Text = "Send Message";
                            break;
                        }
                }
            }
            reader.Close();
            cmd.CommandText = "SELECT Status FROM Friends WHERE (FromUser = @ProfileUserName AND ToUser = @CurrentUserName)";
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                switch (reader["Status"].ToString())
                {
                    case "Pending":
                        {
                            AddFriendButton.Enabled = false;
                            AddFriendButton.Text = "He sent you a request!";
                            SendMessageButton.Text = "Send Message";
                            break;
                        }
                    case "Accepted":
                        {
                            AddFriendButton.Enabled = false;
                            AddFriendButton.Text = "You are friends!";
                            SendMessageButton.Text = "Send Message";
                            break;
                        }
                    default:
                        {

                            AddFriendButton.Enabled = true;
                            AddFriendButton.Text = "Add friend";
                            SendMessageButton.Text = "Send Message";
                            break;
                        }
                }
            }
            sqlConnection1.Close();
        }
        

        if (!Request.IsAuthenticated)
        {
            AddFriendButton.Visible = false;
            SendMessageButton.Visible = false;
        }

        if(profileUserName != currentUserName)
        {
            AddAlbumButton.Visible = false;
            AlbumTextBox.Visible = false;
        }
        Control sendMessage = FindHtmlControlByIdInControl(this, "MessageToSend"); ;
        sendMessage.Visible = false;

    }


    protected void PublicProfileCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        SqlCommand cmd = new SqlCommand();

        cmd.CommandText = "Update Users Set ProfilePublic = @Checked WHERE UserName = @UName";
        cmd.Parameters.Add("@UName", SqlDbType.NVarChar, 50).Value = Membership.GetUser().UserName;
        cmd.Parameters.Add("@Checked", SqlDbType.Bit).Value = !PublicProfileCheckBox.Checked;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = sqlConnection1;

        sqlConnection1.Open();

        cmd.ExecuteNonQuery();
        cmd.Dispose();

        sqlConnection1.Close();

        Response.Redirect("UserPage.aspx");
    }


    protected void AddFriendButton_Click(object sender, EventArgs e)
    {
        String userName = Request.QueryString["UserId"];
        String currentUserName = "";
        if (Request.IsAuthenticated)
            currentUserName = Membership.GetUser().UserName;
        String profileUserName = currentUserName;
        if (userName != null && !userName.Equals(""))
            profileUserName = userName;

        if (profileUserName != currentUserName)
        {
            SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO Friends(FromUser,ToUSer) VALUES (@FromUser,@ToUser)";
            cmd.Parameters.Add("@FromUser", SqlDbType.NVarChar, 50).Value = Membership.GetUser().UserName;
            cmd.Parameters.Add("@ToUser", SqlDbType.NVarChar, 50).Value = profileUserName;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            cmd.ExecuteNonQuery();
            cmd.Dispose();

            sqlConnection1.Close();

            AddFriendButton.DataBind();
            Response.Redirect(Request.RawUrl);
        }
        else
        {
            Response.Redirect("FriendsRequests.aspx");
        }
    }

    protected void SqlDbSelecting(object sender, SqlDataSourceSelectingEventArgs e)
    {
        String userName = Request.QueryString["UserId"];
        String currentUserName = "";
        if (Request.IsAuthenticated)
            currentUserName = Membership.GetUser().UserName;
        String profileUserName = currentUserName;
        if (userName != null && !userName.Equals(""))
            profileUserName = userName;
        
        e.Command.Parameters[0].Value = profileUserName;

    }

    protected void SendMessageButton_Click(object sender, EventArgs e)
    {
        String userName = Request.QueryString["UserId"];
        String currentUserName = "";
        if (Request.IsAuthenticated)
            currentUserName = Membership.GetUser().UserName;
        String profileUserName = currentUserName;
        if (userName != null && !userName.Equals(""))
            profileUserName = userName;

        if (profileUserName != currentUserName)
        {

            Control sendMessage = FindHtmlControlByIdInControl(this, "MessageToSend"); ;
            sendMessage.Visible = true;
        }
        else
        {
            Response.Redirect("MailBox.aspx");
        }
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

    protected void AcceptMessageButton_Click(object sender, EventArgs e)
    {
        if (SendMessageTextBox.Text.Length <=0)
            return;
        String userName = Request.QueryString["UserId"];
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
        cmd.Parameters.Add("@Message", SqlDbType.NVarChar, 50).Value = SendMessageTextBox.Text;
        cmd.Parameters.Add("@Date", SqlDbType.DateTime).Value = DateTime.Now.ToString();
        cmd.CommandType = CommandType.Text;
        cmd.Connection = sqlConnection1;

        sqlConnection1.Open();

        cmd.ExecuteNonQuery();
        cmd.Dispose();

        sqlConnection1.Close();
    }

    protected void SqlDbSelecting1(object sender, SqlDataSourceSelectingEventArgs e)
    {
        String userName = Request.QueryString["UserId"];
        String currentUserName = "";
        if (Request.IsAuthenticated)
            currentUserName = Membership.GetUser().UserName;
        String profileUserName = currentUserName;
        if (userName != null && !userName.Equals(""))
            profileUserName = userName;

        e.Command.Parameters[0].Value = profileUserName;
    }

    protected void ItemRepeaterClicked(object sender, CommandEventArgs e)
    {
        if (e.CommandName != "AlbumButton" && e.CommandName != "RemoveAlbum")
            return;

        if(e.CommandName == "RemoveAlbum")
        {
            SqlConnection sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            SqlCommand cmd = new SqlCommand();
            
            cmd.CommandText = "Delete From Comments Where PhotoId IN (Select Id From Photos Where AlbumId = @Id)";
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = int.Parse(e.CommandArgument.ToString());
            cmd.Connection = sqlcon;

            sqlcon.Open();
            cmd.ExecuteNonQuery();
            cmd.CommandText = "Delete From Photos Where AlbumId = @Id";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "Delete From Albums Where Id = @Id";
            cmd.ExecuteNonQuery();
            sqlcon.Close();
            Repeater2.DataBind();
        }
        else
        {
            string albumId = e.CommandArgument.ToString();
            Response.Redirect("Album.aspx?AlbumId=" + albumId);
        }
    }

    protected void AddAlbumButton_Click(object sender, EventArgs e)
    {
        if (AlbumTextBox.Text.Length <= 0)
            return;
        SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO Albums(AlbumName,UserName) VALUES (@AlbumName,@UserName)";
        cmd.Parameters.Add("@AlbumName", SqlDbType.NVarChar, 50).Value = AlbumTextBox.Text;
        cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = Membership.GetUser().UserName;
        cmd.CommandType = CommandType.Text;
        cmd.Connection = sqlConnection1;

        sqlConnection1.Open();

        cmd.ExecuteNonQuery();
        cmd.Dispose();

        sqlConnection1.Close();

        Response.Redirect(Request.RawUrl);
    }


    protected Boolean HasPermission()
    {
        if (Roles.IsUserInRole("Admin"))
            return true;

        String userName = Request.QueryString["UserId"];
        String currentUserName = "";
        if (Request.IsAuthenticated)
            currentUserName = Membership.GetUser().UserName;
        String profileUserName = currentUserName;
        if (userName != null && !userName.Equals(""))
            profileUserName = userName;

        if (profileUserName == currentUserName)
            return true;

        return false;
    }
}