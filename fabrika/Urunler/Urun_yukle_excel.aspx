<%@ Page Title="Excel ile Ürün Yükleme" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Urun_yukle_excel.aspx.cs" Inherits="fabrika_Urunler_Urun_yukle_excel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <div class="text-center">
                    <asp:Button ID="btnKaydet" runat="server" CssClass="btn btn-success" Text="Kaydet" OnClick="btnKaydet_Click" />
                    <a href="UrunListesi.aspx" class="btn btn-primary ms-2">Geri Dön</a>
                </div>
            </section>
        </div>
    </div>


    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">Excel ile Toplu Ürün Yükleme</header>
                <div class="panel-body">
                    <div class="row mb-3">
                        <div class="col-md-12">
                            <div class="alert alert-info">
                                <p>
                                    <strong>Bilgi:</strong> Excel dosyası ile toplu ürün yüklemesi yapabilirsiniz.
                                    Lütfen aşağıdaki şablonu kullanarak ürün bilgilerini hazırlayınız.
                               
                                </p>
                                <p>
                                    <strong>Gerekli Kolonlar:</strong> Ürün Adı, KDV Oranı, Ürün Tipi, Ürün Kodu, Barkodu, Markası, Kategorisi, Alış Fiyatı, Satış Fiyatı, KDV Dahil mi?, Para Birimi, Stok Miktarı, Birim
                               
                                </p>
                                <p>
                                    <a href="Sablonlar/fabrika_urun_sablon.xlsx" class="btn btn-sm btn-primary" download>
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
                                    <asp:BoundField DataField="UrunAdi" HeaderText="Ürün Adı" />
                                    <asp:BoundField DataField="KDVOrani" HeaderText="KDV Oranı" />
                                    <asp:BoundField DataField="UrunTipiStoklu" HeaderText="Ürün Tipi" />
                                    <asp:BoundField DataField="UrunKodu" HeaderText="Ürün Kodu" />
                                    <asp:BoundField DataField="Barkod" HeaderText="Barkodu" />
                                    <asp:BoundField DataField="Marka" HeaderText="Markası" />
                                    <asp:BoundField DataField="Kategori" HeaderText="Kategorisi" />
                                    <asp:BoundField DataField="AlisFiyati" HeaderText="Alış Fiyatı" />
                                    <asp:BoundField DataField="SatisFiyati" HeaderText="Satış Fiyatı" />
                                    <asp:BoundField DataField="SatisFiyatiKdvDahilmi" HeaderText="KDV Dahil mi?" />
                                    <asp:BoundField DataField="ParaBirimi" HeaderText="Para Birimi" />
                                    <asp:BoundField DataField="StokMiktari" HeaderText="Stok Miktarı" />
                                    <asp:BoundField DataField="Birim" HeaderText="Birim" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>

