<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Mustahsil_yukle_Cks_pdf.aspx.cs" Inherits="fabrika_Mustahsil_Mustahsil_yukle_Cks_pdf" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-10">
            <section class="panel">
                <header class="panel-heading">
                    ÇKS PDF'den Bilgi Yükle ve Önizle
                </header>
                <div class="panel-body">
                    <div class="form-group">
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                        <asp:Button ID="BtnUpload" runat="server" Text="Yükle ve Önizle" OnClick="BtnUpload_Click" CssClass="btn btn-primary" />
                    </div>
                    <asp:Panel ID="PreviewPanel" runat="server" Visible="true">
                        <table class="table table-bordered">
                            <tr>
                                <td>T.C. Kimlik No :</td>
                                <td><asp:TextBox ID="txtTckimlikNo" runat="server" CssClass="form-control" /></td>
                            </tr>
                            <tr>
                                <td>Adı Soyadı:</td>
                                <td><asp:TextBox ID="txtAdSoyad" runat="server" CssClass="form-control" /></td>
                            </tr>
                            <tr>
                                <td>Doğum Tarihi :</td>
                                <td><asp:TextBox ID="txtdogumTarihi" runat="server" CssClass="form-control" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </section>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-10">
            <section class="panel">
                <header class="panel-heading">
                    Kayıtlı Arazi Bilgileri
                </header>
                <div class="panel-body">
                    <asp:GridView ID="GridViewMustahsilTarlalar" runat="server" Visible="true" AutoGenerateColumns="True">
                         
                    </asp:GridView>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="scripts" runat="Server">
</asp:Content>

