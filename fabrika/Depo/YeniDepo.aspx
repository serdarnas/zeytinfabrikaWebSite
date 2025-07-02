<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="YeniDepo.aspx.cs" Inherits="fabrika_Depo_YeniDepo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <section class="panel">
        <div class="panel-body">
            <div class="col-lg-12">
                <div class="col-md-6">
                    <asp:Button ID="btnGeriDon" runat="server" CssClass="btn btn-shadow btn-default btn-block" Text="← Depo Listesine Dön" OnClick="btnGeriDon_Click" />
                </div>
                <div class="col-md-6">
                    <asp:Button ID="btnDepoListesi" runat="server" CssClass="btn btn-shadow btn-info btn-block" Text="📋 Depo Listesi" OnClick="btnDepoListesi_Click" />
                </div>
            </div>
        </div>
    </section>

    <div class="row">
        <!-- Sol Taraf - Yeni Depo Ekle Formu -->
        <div class="col-lg-6">
            <section class="panel">
                <header class="panel-heading">
                    <h3>Yeni Depo Ekle</h3>
                </header>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Depo Adı <span style="color: red;">*</span></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtDepoAdi" runat="server" CssClass="form-control" placeholder="Örnek: Ana Depo" MaxLength="50" />
                                <asp:RequiredFieldValidator ID="rfvDepoAdi" runat="server" 
                                    ControlToValidate="txtDepoAdi" 
                                    ErrorMessage="Depo adı zorunludur" 
                                    CssClass="text-danger" 
                                    Display="Dynamic" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-4 control-label">Depo Kodu <span style="color: red;">*</span></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtDepoKodu" runat="server" CssClass="form-control" placeholder="Örnek: DP001" MaxLength="20" />
                                <asp:RequiredFieldValidator ID="rfvDepoKodu" runat="server" 
                                    ControlToValidate="txtDepoKodu" 
                                    ErrorMessage="Depo kodu zorunludur" 
                                    CssClass="text-danger" 
                                    Display="Dynamic" />
                                <small class="text-muted">Benzersiz depo kodu giriniz</small>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-4 control-label">Kapasite (Kg)</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtKapasite" runat="server" CssClass="form-control" placeholder="Örnek: 10000" TextMode="Number" step="0.01" />
                                <asp:RangeValidator ID="rvKapasite" runat="server" 
                                    ControlToValidate="txtKapasite" 
                                    MinimumValue="0" 
                                    MaximumValue="999999999" 
                                    Type="Double" 
                                    ErrorMessage="Geçerli bir kapasite değeri giriniz" 
                                    CssClass="text-danger" 
                                    Display="Dynamic" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-4 control-label">Mevcut Dolu Miktar (Kg)</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtDoluMiktar" runat="server" CssClass="form-control" placeholder="Örnek: 0" TextMode="Number" step="0.01" Text="0" />
                                <asp:RangeValidator ID="rvDoluMiktar" runat="server" 
                                    ControlToValidate="txtDoluMiktar" 
                                    MinimumValue="0" 
                                    MaximumValue="999999999" 
                                    Type="Double" 
                                    ErrorMessage="Geçerli bir miktar değeri giriniz" 
                                    CssClass="text-danger" 
                                    Display="Dynamic" />
                                <small class="text-muted">Başlangıç stok miktarı</small>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-4 control-label">Durum</label>
                            <div class="col-sm-8">
                                <asp:CheckBox ID="chkDurum" runat="server" Checked="true" Text=" Depo aktif olarak oluşturulsun" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-sm-4 control-label">Açıklama</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Depo hakkında ek bilgiler..." />
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-sm-offset-4 col-sm-8">
                                <asp:Button ID="btnKaydet" runat="server" CssClass="btn btn-success btn-lg" Text="💾 Depo Kaydet" OnClick="btnKaydet_Click" />
                                <asp:Button ID="btnTemizle" runat="server" CssClass="btn btn-default" Text="🗑 Temizle" OnClick="btnTemizle_Click" CausesValidation="false" />
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-sm-offset-4 col-sm-8">
                                <asp:Label ID="lblMesaj" runat="server" CssClass="alert-dismissible" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>

        <!-- Sağ Taraf - Depo Ekleme Kuralları -->
        <div class="col-lg-6">
            <section class="panel">
                <header class="panel-heading">
                    <h3>Depo Ekleme Kuralları</h3>
                </header>
                <div class="panel-body">
                    <div class="alert alert-info">
                        <h4>📝 Önemli Bilgiler:</h4>
                        <ul class="list-unstyled">
                            <li><i class="fa fa-check-circle text-success"></i> <strong>Depo Adı:</strong> Depoyu tanımlayıcı benzersiz bir ad giriniz</li>
                            <li><i class="fa fa-check-circle text-success"></i> <strong>Depo Kodu:</strong> Kısa ve benzersiz bir kod kullanınız (örn: DP001, DEPO-A)</li>
                            <li><i class="fa fa-check-circle text-success"></i> <strong>Kapasite:</strong> Deponun maksimum saklama kapasitesi (Kg cinsinden)</li>
                            <li><i class="fa fa-check-circle text-success"></i> <strong>Mevcut Stok:</strong> Depo oluşturulurken mevcut stok miktarı</li>
                            <li><i class="fa fa-check-circle text-success"></i> <strong>Durum:</strong> Aktif depolar stok işlemlerinde kullanılabilir</li>
                        </ul>
                    </div>
                    
                    <div class="alert alert-warning">
                        <h4>⚠ Dikkat Edilecek Hususlar:</h4>
                        <ul class="list-unstyled">
                            <li><i class="fa fa-exclamation-triangle text-warning"></i> Depo kodu oluşturulduktan sonra değiştirilemez</li>
                            <li><i class="fa fa-exclamation-triangle text-warning"></i> Mevcut dolu miktar, kapasiteden büyük olamaz</li>
                            <li><i class="fa fa-exclamation-triangle text-warning"></i> Pasif depolar stok işlemlerinde görünmez</li>
                        </ul>
                    </div>

                    <div class="alert alert-success">
                        <h4>💡 İpuçları:</h4>
                        <ul class="list-unstyled">
                            <li><i class="fa fa-lightbulb-o text-success"></i> <strong>Depo Adlandırma:</strong> Lokasyon bazlı adlar kullanın (Ana Depo, Üst Depo, vs.)</li>
                            <li><i class="fa fa-lightbulb-o text-success"></i> <strong>Kod Sistemi:</strong> Tutarlı bir kodlama sistemi kullanın</li>
                            <li><i class="fa fa-lightbulb-o text-success"></i> <strong>Kapasite Planlaması:</strong> Gelecekteki ihtiyaçları da göz önünde bulundurun</li>
                        </ul>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>

