<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="musteri_yukle_excel.aspx.cs" Inherits="fabrika_Musteriler_musteri_yukle_excel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <div class="text-center">
                    <asp:Button ID="btnKaydet" runat="server" CssClass="btn btn-success" Text="Kaydet" OnClick="btnKaydet_Click" />
                    <a href="Default.aspx" class="btn btn-primary ms-2">Geri Dön</a>
                </div>
            </section>
        </div>
    </div>


    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">Excel ile Toplu Müşteri Yükleme</header>
                <div class="panel-body">
                    <div class="row mb-3">
                        <div class="col-md-12">
                            <div class="alert alert-info">
                                <p>
                                    <strong>Bilgi:</strong> Excel dosyası ile toplu müşteri yüklemesi yapabilirsiniz.
                                    Lütfen aşağıdaki şablonu kullanarak müşteri bilgilerini hazırlayınız.
                               
                                </p>
                                <p>
                                    <strong>Gerekli Kolonlar:</strong> Müşteri Ünvanı, Müşteri Kodu, Adres, Vergi Dairesi, Vergi No, Kategori, E-Posta, Cep Telefonu, Yetkili Kişi Adı, Bakiyesi, Para Birimi
                               
                                </p>
                                <p>
                                    <a href="Sablonlar/zeytinfabrika_musteri_sablon.xlsx" class="btn btn-sm btn-primary" download>
                                        <i class="fa fa-download"></i> Excel Şablonunu İndir
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
                                    <asp:BoundField DataField="FirmaAdi" HeaderText="Müşteri Ünvanı" />
                                    <asp:BoundField DataField="MusteriKodu" HeaderText="Müşteri Kodu" />
                                    <asp:BoundField DataField="Adres" HeaderText="Adres" />
                                    <asp:BoundField DataField="VergiDairesi" HeaderText="Vergi Dairesi" />
                                    <asp:BoundField DataField="VergiNo" HeaderText="Vergi No" />
                                    <asp:BoundField DataField="Kategori" HeaderText="Kategori" />
                                    <asp:BoundField DataField="Email" HeaderText="E-Posta" />
                                    <asp:BoundField DataField="CepTelefonu" HeaderText="Cep Telefonu" />
                                    <asp:BoundField DataField="YetkiliAdi" HeaderText="Yetkili Kişi Adı" />
                                    <asp:BoundField DataField="Bakiyesi" HeaderText="Bakiyesi" />
                                    <asp:BoundField DataField="ParaBirimi" HeaderText="Para Birimi" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
