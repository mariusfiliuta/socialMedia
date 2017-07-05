<%@ Page Title="" Language="C#" MasterPageFile="~/GeneralItems.master" AutoEventWireup="true" CodeFile="Search.aspx.cs" Inherits="Search" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <link href="SearchStyle.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div style="position:absolute;left: 50%; margin-right: -50%;transform: translate(-50%, -50%);">
    <asp:TextBox ID="SearchTextBox" runat="server" AutoPostBack="True" OnTextChanged="SearchTextBox_TextChanged"></asp:TextBox>
    </div>
    <br />
    <br />

    <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1" OnItemCommand="ItemRepeaterClicked">
        <HeaderTemplate>
            <div style="left: 50%; margin-right: -50%;" >
            <div id="Repeater" class="container">
            <table class="table table-bordered">
        </HeaderTemplate>
        <ItemTemplate>
        <tr>
            <td id="ImageTd">
                <asp:Image ID="ProfilePicture" runat="server" ImageUrl='<%# "~/ProfilePics/" + Eval("ProfilePicture")%>' Width="50%"/>
            </td>
            <td style="text-align:center;"> 
                <asp:Label ID="Label1" CssClass="ProfileName" runat="server" style="text-align:center;vertical-align: middle;" Text='<%# Eval("UserName") %>'></asp:Label>
            </td>
            <td style="text-align:center;vertical-align:middle;">
                <asp:LinkButton ID="Button" CssClass="Buttons" Runat="server" CommandArgument='<%# Eval("UserName") %>' CommandName="AccessProfile">Access Profile</asp:LinkButton>
            </td>
         </tr>
        </ItemTemplate>

        <FooterTemplate>
            </table>
            </div>
            </div>
        </FooterTemplate>
    </asp:Repeater>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT [UserName], [ProfilePicture] FROM [Users] WHERE ([UserName] LIKE '%' + @UserName + '%') ORDER BY [UserName]">
        <SelectParameters>
            <asp:SessionParameter Name="UserName" SessionField="shearchedName" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
    </asp:Content>

