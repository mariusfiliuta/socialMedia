<%@ Page Title="" Language="C#" MasterPageFile="~/GeneralItems.master" AutoEventWireup="true" CodeFile="UserPage.aspx.cs" Inherits="UserPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <link href="UserPage.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="BodyDiv">
    
        <div id="PublicProfileSetterDiv">
            <asp:Label ID="PublicProfileLabel" runat="server" Text="Public Profile"></asp:Label>
            <asp:CheckBox ID="PublicProfileCheckBox" runat="server" OnCheckedChanged="PublicProfileCheckBox_CheckedChanged" AutoPostBack="True" ClientIDMode="Static" />
        </div>

        <div id="TopDiv" class="row">
            <div id="ProfilePictureDiv" class="topdiv col-xs-4">
                <asp:Image ID="ProfilePicture" runat="server" CssClass="rounded" ImageUrl="DefaultProfilePicture.jpg" style="max-width:35%; max-height:10%;"/>
                <asp:FileUpload ID="ProfilePictureUpload" ClientIDMode="Static" onchange="this.form.submit()" runat="server" ViewStateMode="Inherit" />
                <asp:Label ID="ProfileLabel" runat="server" Text="Label" Visible="False"></asp:Label>
            </div>
            <div id="ProfileButtons" class="topdiv col-xs-4 col-xs-offset-4 text-right">
                <asp:Button ID="AddFriendButton" class="text-right" runat="server" Text="Add Friend" OnClick="AddFriendButton_Click" />
                <asp:Button ID="SendMessageButton"  class="text-right" runat="server" Text="Send Message" OnClick="SendMessageButton_Click" />
             
             
                <div id="MessageToSend" style="position:relative;" runat="server">
                        <asp:Textbox runat="server" style="text-align:center;" class="form-control" ID="SendMessageTextBox" MaxLength="50"></asp:Textbox>
                        <asp:Button runat="server"  style="position:absolute;left:0;" text="Send" ID="AcceptMessageButton" onclick="AcceptMessageButton_Click" />
                        <asp:Button runat="server"  style="position:absolute;right:0;" text="Cancel" ID="CancelMessageButton" />
                </div>
            </div>
       </div>
        <br />
        <br />
        <br />
        Friends
            <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1">
                 <HeaderTemplate>
                     <div id="FriendsDiv" class="pre-scrollable" style="max-height:125px;overflow-y:scroll;" class="container col-xs-12">
                 </HeaderTemplate>
        
                <ItemTemplate>
                    <div style="height:125px;width:125px;display:inline-block" class="friends">
                    <asp:Image ID="FriendProfile" class="img-circle" runat="server" ImageUrl='<%# "~/ProfilePics/" + Eval("ProfilePicture") %>' Height="125px" Width="125px"/>
                    </div>
                </ItemTemplate>

                <FooterTemplate>
                     </div>
                </FooterTemplate>
            </asp:Repeater>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT DISTINCT [ToUser], [ProfilePicture] FROM [Friends], [Users] WHERE (([FromUser] = @FromUser) AND ([Status] = @Status) AND (UserName = ToUser)) ORDER BY [ToUser]" OnSelecting="SqlDbSelecting">
                <SelectParameters>
                    <asp:Parameter Name="FromUser" Type="String" />
                    <asp:Parameter DefaultValue="Accepted" Name="Status" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>

        Albums

        <asp:Repeater ID="Repeater2" runat="server" DataSourceID="PhotoSqlDataSource" OnItemCommand="ItemRepeaterClicked">
            <HeaderTemplate>
                <div class="container row">
                <div class="list-group">
            </HeaderTemplate>
            <ItemTemplate>
                <div style="display:inline-block; position: relative;">
                    <asp:LinkButton runat="server" class="glyphicon glyphicon-remove" style="position: absolute; top:0; right:0; z-index:51;" Visible='<%# HasPermission() %>' CommandName="RemoveAlbum" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                    <asp:Button ID="AlbumButton"  class="list-group-item" style="display:inline;" runat="server" CommandName="AlbumButton" CommandArgument='<%# Eval("Id") %>' Text='<%# Eval("AlbumName") %>' />
                </div>
            </ItemTemplate>
            <FooterTemplate>
                </div></div>
            </FooterTemplate>
        </asp:Repeater>
        <asp:TextBox ID="AlbumTextBox" runat="server" MaxLength="50" ControlToValidate="AlbumTextBox"></asp:TextBox> <asp:Button ID="AddAlbumButton" runat="server" Text="Create Album" OnClick="AddAlbumButton_Click" />
        <asp:SqlDataSource ID="PhotoSqlDataSource"  runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="Select [AlbumName],[Id] From [Albums] Where ([UserName] = @UserName) Order By [Id]" OnSelecting="SqlDbSelecting1">
                <SelectParameters>
                    <asp:Parameter Name="UserName" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>

    </div>
</asp:Content>

