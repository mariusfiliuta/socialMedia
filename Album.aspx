<%@ Page Title="" Language="C#" MasterPageFile="~/GeneralItems.master" AutoEventWireup="true" CodeFile="Album.aspx.cs" Inherits="Album" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:FileUpload runat="server" ClientIDMode="Static" onchange="this.form.submit()" ID="PictureUpload" Visible="False"></asp:FileUpload>
    <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSource1" OnItemCommand="Repeater1_ItemCommand">
        <HeaderTemplate>
                <div class="container">
                <div class="list-group">
            </HeaderTemplate>
            <ItemTemplate>
                <div style="display:inline-block; position: relative;">
                    <asp:LinkButton runat="server" class="glyphicon glyphicon-remove" style="position: absolute; top:0; right:0; z-index:51;"  Visible='<%# HasPermission() %>' CommandName="RemoveImage" CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                    <asp:ImageButton class="list-group-item img-thumbnail"  Width="304" Height="236" ID="PhotoButton" runat="server" ImageUrl='<%# "Photos/" + Eval("PhotoName") %>' CommandName="ImageButton" CommandArgument='<%# Eval("Id") %>'/>
                </div>
            </ItemTemplate>
            <FooterTemplate>
                </div></div>
            </FooterTemplate>
    </asp:Repeater>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT [PhotoName],[Id] From [Photos] Where ([AlbumId] = @AlbumId)" OnSelecting="SqlDbSelecting">
        <SelectParameters>
                    <asp:Parameter Name="AlbumId" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

