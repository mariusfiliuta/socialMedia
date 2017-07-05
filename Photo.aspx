<%@ Page Title="" Language="C#" MasterPageFile="~/GeneralItems.master" AutoEventWireup="true" CodeFile="Photo.aspx.cs" Inherits="Photo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Image ID="PhotoImage" class="img-responsive center-block" style="max-height:600px;max-width:50%" runat="server" />
    <asp:Repeater ID="Repeater1" runat="server" DataSourceID="CommentDataSource" OnItemCommand="Repeater1_ItemCommand">
        <HeaderTemplate>
            <div class="container center-block">
            <table class="table table-bordered">
                <tr>
                    <th>
                        UserName
                    </th>
                    <th>
                        Comment
                    </th>
                    <th>
                        Date
                    </th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <asp:LinkButton runat="server" class="btn btn-info btn-lg" Visible='<%# HasPermission() %>' CommandName="Remove" CommandArgument='<%# Eval("Id") %>'>
                      <span class="glyphicon glyphicon-remove"></span> Remove 
                    </asp:LinkButton>
                    <asp:Label ID="UserNameLabel" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                </td>
                <td>
                    <asp:Label ID="CommentTextLabel" runat="server" Text='<%# Eval("CommentMessage") %>'></asp:Label>
                </td>
                <td>
                    <asp:Label ID="DateLabel" runat="server" Text='<%# Eval("Date") %>'></asp:Label>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
            </div>
        </FooterTemplate>
    </asp:Repeater>
    <div id="AddComment" runat="server" class="center-block"><asp:TextBox ID="CommentTextBox" class="center-block" runat="server" MaxLength="50"></asp:TextBox><asp:Button class="center-block" ID="SendButton" runat="server" Text="Add Comment" OnClick="SendButton_Click" /></div>
    <asp:SqlDataSource ID="CommentDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT * FROM [Comments] WHERE ([PhotoId] = @PhotoId) Order By Date" OnSelecting="CommentsDataSource_OnSelecting">
        <SelectParameters>
            <asp:Parameter Name="PhotoId" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

