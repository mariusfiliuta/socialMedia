<%@ Page Title="" Language="C#" MasterPageFile="~/GeneralItems.master" AutoEventWireup="true" CodeFile="Message.aspx.cs" Inherits="Message" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <link href="SearchStyle.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1">
        
        <HeaderTemplate>
            <div style="overflow:scroll;height:400px">
            <table class="table table-bordered">
                <tr>
                    <th>
                        UserName
                    </th>
                    <th>
                        Message
                    </th>
                    <th>
                        DateTime
                    </th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <asp:Label ID="UserNameLabel" runat="server" Text='<%# Eval("FromUser") == Eval("UserName") ? Eval("ToUser"):Eval("FromUser")%>'></asp:Label>
                </td>
                <td>
                    <asp:Label ID="MessageLabel" runat="server" Text='<%# Eval("Message") %>'></asp:Label>
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
    <br />
    <div style="width:100%" class="form-group">
        <asp:TextBox ID="MessageTextBox" class="form-control" style="text-align:center" runat="server" MaxLength="50"></asp:TextBox>
        <asp:Button ID="SendMessageButton" class="btn btn-primary btn-block" runat="server" Text="Send"  OnClick="SendMessageButton_Click"/>
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT [FromUser], [ToUser], @UserName UserName, [Message], [Messages].[Date] FROM [Messages] WHERE ( ([FromUser] = @UserName) AND ([ToUser] = @Friend) ) OR ( ([FromUser] = @Friend) AND ([ToUser] = @UserName) ) ORDER BY Date" OnSelecting="SqlDbSelecting">
                <SelectParameters>
                    <asp:Parameter Name="UserName" Type="String" />
                    <asp:Parameter Name="Friend" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
</asp:Content>

