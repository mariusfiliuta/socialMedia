<%@ Page Title="" Language="C#" MasterPageFile="~/GeneralItems.master" AutoEventWireup="true" CodeFile="FriendsRequests.aspx.cs" Inherits="FriendsRequests" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1" OnItemCommand="ItemRepeaterClicked">
        <HeaderTemplate>
            <table class="table table-bordered">
        </HeaderTemplate>
        
        <ItemTemplate>
        <tr>
            <td>
                <asp:Label ID="UserNameLabel" runat="server" Text='<%# Eval("FromUser") %>'></asp:Label>
            </td>
            <td>
                <asp:Button ID="AcceptButton" runat="server" Text="Accept" CommandName="AcceptRequest" CommandArgument='<%# Eval("FromUser") %>'/>
            </td>
            <td>
                <asp:Button ID="DenyButton" runat="server" Text="Deny" CommandName="DenyRequest" CommandArgument='<%# Eval("FromUser") %>'/>
            </td>
        </tr>
        </ItemTemplate>

        <FooterTemplate>
            </table>
        </FooterTemplate>


    </asp:Repeater>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT [FromUser] FROM [Friends] WHERE (([ToUser] = @ToUser) AND ([Status] = @Status))" OnSelecting="SqlDbSelecting">
        <SelectParameters>
            <asp:Parameter Name="ToUser" Type="String" />
            <asp:Parameter DefaultValue="Pending" Name="Status" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

