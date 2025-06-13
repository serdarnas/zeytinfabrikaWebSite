<%@ Page Title="Test Page" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="fabrika_Zeytinyagi_test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid py-4">
        <h1>Test Page</h1>
        <p>If you can see this text, the master page integration is working correctly.</p>
        <asp:Label runat="server" ID="lblTest" Text="This is a server control test."></asp:Label>
    </div>
</asp:Content>
