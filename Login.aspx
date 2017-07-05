<%@ Page Title="" Language="C#" MasterPageFile="~/GeneralItems.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <div style="top:100px;height:500px">
    <asp:LoginView ID="LoginView1" runat="server">
        
        <AnonymousTemplate>
            <div style="position:absolute;top: 40%;left: 50%; margin-right: -50%; transform: translate(-50%, -50%); text-align:center; ">
            <asp:Login ID="Login1" runat="server" DestinationPageUrl="~/Home.aspx">
            </asp:Login>
            <hr style="color: #677E98" />
            <asp:Label ID="OrLabel" runat="server" Text="Or"></asp:Label>
            <hr style="color: #677E98" />
            <asp:CreateUserWizard ID="CreateUserWizard1" runat="server">
                <WizardSteps>
                    <asp:CreateUserWizardStep runat="server" />
                    <asp:CompleteWizardStep runat="server" />
                </WizardSteps>
            </asp:CreateUserWizard>
            </div>
        </AnonymousTemplate>
    </asp:LoginView>
   </div>
</asp:Content>

