<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Mustahsil_yukle_excel.aspx.cs" Inherits="fabrika_Mustahsil_Mustahsil_yukle_excel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">Excel ile Toplu Mustahsil Yükleme</header>
                <div class="panel-body">
                    <div class="row mb-3">
                        <div class="col-md-12">
                            <div class="alert alert-info">
                                <p>
                                    <strong>Bilgi:</strong> Excel dosyası ile toplu Mustahsil yüklemesi yapabilirsiniz.
                                    Lütfen aşağıdaki şablonu kullanarak Mustahsil bilgilerini hazırlayınız.
                                </p>
                                <p>
                                    <strong>Gerekli Kolonlar:</strong> Ad ,Soyad ,Telefon ,E-Posta ,Adres ,TC Kimlik No ,Bakiyesi ,Banka Bilgileri</p>
                                <p>
                                    <a href="Sablonlar/zeytinfabrika_mustahsil_sablon.xlsx" class="btn btn-sm btn-primary" download>
                                        <i class="fa fa-download"></i>Excel Şablonunu İndir
                                    </a>
                                </p>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-3">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label for="fuExcel">Excel Dosyası Seçin</label>
                                <asp:FileUpload ID="fuExcel" runat="server" CssClass="form-control" />
                                <small class="form-text text-muted">Sadece .xlsx formatında dosyalar kabul edilmektedir.</small>
                            </div>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <div class="form-group">
                                <asp:Button ID="btnOnizle" runat="server" CssClass="btn btn-warning" Text="Önizle" OnClick="btnOnizle_Click" />
                                <asp:Button ID="btnOnlaKaydet" runat="server" CssClass="btn btn-success" Text="Onayla Kaydet" OnClick="btnKaydet_Click" />
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <asp:GridView ID="gvOnizleme" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="false" Visible="false">
                                <Columns>
                                    <asp:BoundField DataField="Ad" HeaderText="Ad" />
                                    <asp:BoundField DataField="Soyad" HeaderText="Soyad" />
                                    <asp:BoundField DataField="Telefon" HeaderText="Telefon" />
                                    <asp:BoundField DataField="Email" HeaderText="E-Posta" />
                                    <asp:BoundField DataField="Adres" HeaderText="Adres" />
                                    <asp:BoundField DataField="TCKimlikNo" HeaderText="TC Kimlik No" />
                                    <asp:BoundField DataField="Bakiyesi" HeaderText="Bakiyesi" />
                                    <asp:BoundField DataField="BankaBilgileri" HeaderText="Banka Bilgileri" /> 
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>

</asp:Content>

