<%@ Page Title="" Language="C#" MasterPageFile="~/GeneralItems.master" AutoEventWireup="true" CodeFile="MailBox.aspx.cs" Inherits="MailBox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <link href="SearchStyle.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1"  OnItemCommand="ItemRepeaterClicked">
        <HeaderTemplate>
            <div style="left: 50%; margin-right: -50%;" >
            <div id="Repeater" class="container">
            <table class="table table-bordered" style="overflow:scroll;">
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <asp:Image ID="FriendProfile" runat="server" ImageUrl='<%# "~/ProfilePics/" + Eval("ProfilePicture") %>' Width="50%"/>
                </td>
                <td>
                    <asp:Label ID="Friend" runat="server" Text='<%# Eval("Friend") %>'></asp:Label>
                </td>
                <td>
                    <asp:Label ID="NumberMessages" runat="server" Text='<%# Eval("NrMessages") %>'></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("NrMessages") %>'></asp:Label>
                </td>
                <td>
                    <asp:Button ID="AccesMessagesButton" runat="server" Text="Go To Messages" CommandName="AccessMessages" CommandArgument='<%# Eval("Friend") %>'/>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
            </div>
            </div>
        </FooterTemplate>
    </asp:Repeater>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT DISTINCT [Friends].[ToUser] Friend, [ProfilePicture], Count([Messages].[Message]) NrMessages FROM [Friends], [Users], [Messages] WHERE (([Friends].[FromUser] = @FromUser) AND ([Status] = @Status) AND ([UserName] = [Friends].[ToUser]) AND ((([Messages].[FromUser] = [Friends].[FromUser]) AND  [Messages].[ToUser] = [Friends].[ToUser])OR (([Messages].[FromUser] = [Friends].[ToUser]) AND  [Messages].[ToUser] = [Friends].[FromUser]))) GROUP BY [Friends].[ToUser], [Friends].[FromUser], [ProfilePicture] ORDER BY [Friends].[ToUser]" OnSelecting="SqlDbSelecting">
                <SelectParameters>
                    <asp:Parameter Name="FromUser" Type="String" />
                    <asp:Parameter DefaultValue="Accepted" Name="Status" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
</asp:Content>

