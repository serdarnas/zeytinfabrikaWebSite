<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Tedarikci_yukle_excel.aspx.cs" Inherits="fabrika_Tedarikciler_Tedarikci_yukle_excel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">

                <div class="panel-body">
                    <!-- Üst Menü Butonları -->
                    <div class="btn-group">
                        <asp:Button ID="btnKaydet" runat="server" Text="Kaydet" CssClass="icon-plus btn btn-shadow  btn-success" Style="margin-right: 5px;"/>
                         
                        <asp:HyperLink ID="btnExcelIndir" runat="server" NavigateUrl="Default.aspx" CssClass="btn btn-shadow btn-warning" Style="margin-right: 5px;">
                            <i class="icon-truck"></i> Geri
                        </asp:HyperLink>
                        <!-- Hata mesajı için panel -->
                        <asp:Panel ID="pnlHata" runat="server" CssClass="alert alert-danger mb-3" Visible="false">
                            <asp:Label ID="lblHata" runat="server" Text=""></asp:Label>
                        </asp:Panel>

                        <!-- Başarı mesajı için panel -->
                        <asp:Panel ID="pnlBasari" runat="server" CssClass="alert alert-success mb-3" Visible="false">
                            <asp:Label ID="lblBasari" runat="server" Text=""></asp:Label>
                        </asp:Panel>
                    </div>
                </div>
            </section>
        </div>
    </div>   
    
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">Excel ile Toplu Tedarikci Yükleme</header>
                <div class="panel-body">
                    <div class="row mb-3">
                        <div class="col-md-12">
                            <div class="alert alert-info">
                                <p>
                                    <strong>Bilgi:</strong> Excel dosyası ile toplu Tedarikci yüklemesi yapabilirsiniz.
                                    Lütfen aşağıdaki şablonu kullanarak Tedarikci bilgilerini hazırlayınız.
                               
                                </p>
                                <p>
                                    <strong>Gerekli Kolonlar:</strong> Tedarikci Ünvanı, Tedarikci Kodu, Adres, Vergi Dairesi, Vergi No, E-Posta, Cep Telefonu, Yetkili Kişi Adı, Bakiyesi, Para Birimi
                               
                                </p>
                                <p>
                                    <a href="Sablonlar/zeytinfabrika_tedarikci_sablon.xlsx" class="btn btn-sm btn-primary" download>
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
                                    <asp:BoundField DataField="FirmaAdi" HeaderText="Tedarikci Ünvanı" />
                                    <asp:BoundField DataField="TedarikciKodu" HeaderText="Tedarikci Kodu" />
                                    <asp:BoundField DataField="Adres" HeaderText="Adres" />
                                    <asp:BoundField DataField="VergiDairesi" HeaderText="Vergi Dairesi" />
                                    <asp:BoundField DataField="VergiNo" HeaderText="Vergi No" /> 
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

