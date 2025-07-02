<%@ Page Title="Bakiye Düzelt" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="BakiyeDuzelt.aspx.cs" Inherits="fabrika_Musteriler_BakiyeDuzelt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css">
    <style>
        .payment-form {
            background: linear-gradient(135deg, #ffecd2 0%, #fcb69f 100%);
            border-radius: 15px;
            padding: 2rem;
            color: #333;
            box-shadow: 0 10px 30px rgba(0,0,0,0.2);
        }
        .form-control, .form-select {
            border-radius: 10px;
            border: 2px solid rgba(0,0,0,0.1);
            background: rgba(255,255,255,0.8);
            color: #333;
            padding: 12px 15px;
        }
        .form-control::placeholder {
            color: rgba(0,0,0,0.5);
        }
        .form-control:focus, .form-select:focus {
            background: rgba(255,255,255,0.9);
            border-color: rgba(0,0,0,0.3);
            color: #333;
            box-shadow: 0 0 0 0.2rem rgba(252,182,159,0.25);
        }
        .btn-payment {
            background: rgba(0,0,0,0.1);
            border: 2px solid rgba(0,0,0,0.2);
            color: #333;
            border-radius: 10px;
            padding: 12px 30px;
            font-weight: 600;
            transition: all 0.3s ease;
        }
        .btn-payment:hover {
            background: rgba(0,0,0,0.2);
            border-color: rgba(0,0,0,0.3);
            color: #333;
            transform: translateY(-2px);
        }
        .adjustment-info {
            background: rgba(255,255,255,0.3);
            border-radius: 10px;
            padding: 1rem;
            margin: 1rem 0;
        }
        .customer-info {
            background: rgba(255,255,255,0.3);
            border-radius: 10px;
            padding: 1rem;
            margin-bottom: 1.5rem;
        }
        .balance-card {
            background: rgba(255,255,255,0.4);
            border-radius: 10px;
            padding: 1rem;
            text-align: center;
            margin-bottom: 1rem;
        }
        .balance-old {
            color: #dc3545;
        }
        .balance-new {
            color: #28a745;
        }
        @media (max-width: 768px) {
            .payment-form {
                padding: 1.5rem;
                margin: 1rem;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid py-4">
        <!-- Başlık -->
        <div class="d-flex justify-content-between align-items-center mb-4">
            <h2 class="mb-0"><i class="bi bi-calculator"></i> Bakiye Düzelt</h2>
            <asp:LinkButton ID="btnGeriDon" runat="server" CssClass="btn btn-outline-secondary" OnClick="btnGeriDon_Click">
                <i class="bi bi-arrow-left"></i> Geri Dön
            </asp:LinkButton>
        </div>

        <div class="row justify-content-center">
            <div class="col-lg-10">
                <div class="payment-form">
                    <!-- Müşteri Bilgileri -->
                    <div class="customer-info">
                        <div class="row">
                            <div class="col-md-8">
                                <h5><i class="bi bi-person-circle"></i> <asp:Label ID="lblMusteriAdi" runat="server"></asp:Label></h5>
                                <p class="mb-0"><i class="bi bi-geo-alt"></i> <asp:Label ID="lblMusteriAdres" runat="server"></asp:Label></p>
                            </div>
                            <div class="col-md-4">
                                <div class="balance-card">
                                    <small>Mevcut Bakiye</small>
                                    <h4 class="balance-old"><asp:Label ID="lblMevcutBakiye" runat="server"></asp:Label> ₺</h4>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Düzeltme Bilgileri -->
                    <div class="row g-3">
                        <div class="col-md-6">
                            <label class="form-label"><i class="bi bi-arrow-up-down"></i> Düzeltme Türü</label>
                            <asp:DropDownList ID="ddlDuzeltmeTuru" runat="server" CssClass="form-select">
                                <asp:ListItem Value="Artir" Text="Bakiye Artır (+)" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="Azalt" Text="Bakiye Azalt (-)"></asp:ListItem>
                                <asp:ListItem Value="Sifirla" Text="Bakiye Sıfırla"></asp:ListItem>
                                <asp:ListItem Value="Manuel" Text="Manuel Bakiye Belirle"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label"><i class="bi bi-cash"></i> Tutar</label>
                            <asp:TextBox ID="txtTutar" runat="server" CssClass="form-control" placeholder="0,00" TextMode="Number" step="0.01"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvTutar" runat="server" ControlToValidate="txtTutar" 
                                ErrorMessage="Tutar girilmelidir" CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label"><i class="bi bi-calendar"></i> İşlem Tarihi</label>
                            <asp:TextBox ID="txtIslemTarihi" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label"><i class="bi bi-list-check"></i> Düzeltme Nedeni</label>
                            <asp:DropDownList ID="ddlDuzeltmeNedeni" runat="server" CssClass="form-select">
                                <asp:ListItem Value="Hatalı Kayıt" Text="Hatalı Kayıt Düzeltmesi"></asp:ListItem>
                                <asp:ListItem Value="Eksik Ödeme" Text="Eksik Ödeme Kaydı"></asp:ListItem>
                                <asp:ListItem Value="Fazla Ödeme" Text="Fazla Ödeme Düzeltmesi"></asp:ListItem>
                                <asp:ListItem Value="Sistem Hatası" Text="Sistem Hatası Düzeltmesi"></asp:ListItem>
                                <asp:ListItem Value="Muhasebe Düzeltmesi" Text="Muhasebe Düzeltmesi"></asp:ListItem>
                                <asp:ListItem Value="Diğer" Text="Diğer" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-12">
                            <label class="form-label"><i class="bi bi-chat-text"></i> Açıklama</label>
                            <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control" TextMode="MultiLine" 
                                Rows="3" placeholder="Düzeltme açıklaması (zorunlu)"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvAciklama" runat="server" ControlToValidate="txtAciklama" 
                                ErrorMessage="Açıklama girilmelidir" CssClass="text-danger small" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <!-- Düzeltme Özeti -->
                    <div class="adjustment-info">
                        <div class="row text-center">
                            <div class="col-md-3">
                                <h5 class="balance-old"><asp:Label ID="lblEskiBakiye" runat="server" Text="0,00"></asp:Label> ₺</h5>
                                <small>Eski Bakiye</small>
                            </div>
                            <div class="col-md-3">
                                <h5><asp:Label ID="lblDuzeltmeMiktari" runat="server" Text="0,00"></asp:Label> ₺</h5>
                                <small>Düzeltme</small>
                            </div>
                            <div class="col-md-3">
                                <h5 class="balance-new"><asp:Label ID="lblYeniBakiye" runat="server" Text="0,00"></asp:Label> ₺</h5>
                                <small>Yeni Bakiye</small>
                            </div>
                            <div class="col-md-3">
                                <h5><asp:Label ID="lblDuzeltmeTuruGoster" runat="server" Text="Artır"></asp:Label></h5>
                                <small>İşlem Türü</small>
                            </div>
                        </div>
                    </div>

                    <!-- İşlem Butonları -->
                    <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                        <asp:Button ID="btnTemizle" runat="server" Text="Temizle" CssClass="btn btn-payment" 
                            OnClick="btnTemizle_Click" CausesValidation="false" />
                        <asp:Button ID="btnKaydet" runat="server" Text="Düzeltmeyi Kaydet" CssClass="btn btn-payment" 
                            OnClick="btnKaydet_Click" OnClientClick="return confirm('Bakiye düzeltmesi kalıcıdır. Emin misiniz?');" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Son Düzeltmeler -->
        <div class="row mt-4">
            <div class="col-12">
                <div class="card shadow-sm">
                    <div class="card-header bg-light">
                        <h5 class="mb-0"><i class="bi bi-clock-history"></i> Son Bakiye Düzeltmeleri</h5>
                    </div>
                    <div class="card-body">
                        <asp:Repeater ID="rptSonDuzeltmeler" runat="server">
                            <ItemTemplate>
                                <div class="d-flex justify-content-between align-items-center py-2 border-bottom">
                                    <div>
                                        <strong><%# String.Format("{0:N2}", Eval("Tutar")) %> ₺</strong>
                                        <small class="text-muted d-block"><%# Eval("DuzeltmeTuru") %> - <%# Eval("DuzeltmeNedeni") %></small>
                                        <small class="text-muted d-block"><%# Eval("Aciklama") %></small>
                                    </div>
                                    <div class="text-end">
                                        <span class="badge bg-secondary"><%# String.Format("{0:dd.MM.yyyy}", Eval("IslemTarihi")) %></span>
                                        <small class="text-muted d-block"><%# String.Format("{0:dd.MM.yyyy HH:mm}", Eval("KayitTarihi")) %></small>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Panel ID="pnlSonDuzeltmeYok" runat="server" Visible="false" CssClass="text-center py-4">
                            <i class="bi bi-info-circle text-muted" style="font-size: 2rem;"></i>
                            <p class="text-muted mt-2">Henüz bakiye düzeltmesi bulunmuyor.</p>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Bakiye Düzeltme Geçmişi -->
        <div class="col-12 mt-4">
            <div class="card">
                <div class="card-header">
                    <h5 class="card-title mb-0">
                        <i class="bi bi-clock-history me-2"></i>Bakiye Düzeltme Geçmişi
                    </h5>
                </div>
                <div class="card-body">
                    <div id="divBakiyeGecmisi">
                        <div class="text-center py-4">
                            <div class="spinner-border text-primary" role="status">
                                <span class="visually-hidden">Yükleniyor...</span>
                            </div>
                            <p class="text-muted mt-2">Bakiye geçmişi yükleniyor...</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var mevcutBakiye = 0;

        // Sayfa yüklendiğinde mevcut bakiyeyi al
        window.addEventListener('load', function() {
            var today = new Date().toISOString().split('T')[0];
            document.getElementById('<%= txtIslemTarihi.ClientID %>').value = today;
            
            var bakiyeText = document.getElementById('<%= lblMevcutBakiye.ClientID %>').innerText;
            mevcutBakiye = parseFloat(bakiyeText.replace(',', '.')) || 0;
            document.getElementById('<%= lblEskiBakiye.ClientID %>').innerText = mevcutBakiye.toLocaleString('tr-TR', {minimumFractionDigits: 2});
            
            hesaplaYeniBakiye();
        });

        // Tutar değiştiğinde hesapla
        document.getElementById('<%= txtTutar.ClientID %>').addEventListener('input', hesaplaYeniBakiye);
        
        // Düzeltme türü değiştiğinde hesapla
        document.getElementById('<%= ddlDuzeltmeTuru.ClientID %>').addEventListener('change', function() {
            document.getElementById('<%= lblDuzeltmeTuruGoster.ClientID %>').innerText = this.options[this.selectedIndex].text;
            hesaplaYeniBakiye();
        });

        function hesaplaYeniBakiye() {
            var tutar = parseFloat(document.getElementById('<%= txtTutar.ClientID %>').value) || 0;
            var tur = document.getElementById('<%= ddlDuzeltmeTuru.ClientID %>').value;
            var yeniBakiye = mevcutBakiye;
            var duzeltmeMiktari = tutar;

            switch(tur) {
                case 'Artir':
                    yeniBakiye = mevcutBakiye + tutar;
                    duzeltmeMiktari = '+' + tutar.toLocaleString('tr-TR', {minimumFractionDigits: 2});
                    break;
                case 'Azalt':
                    yeniBakiye = mevcutBakiye - tutar;
                    duzeltmeMiktari = '-' + tutar.toLocaleString('tr-TR', {minimumFractionDigits: 2});
                    break;
                case 'Sifirla':
                    yeniBakiye = 0;
                    duzeltmeMiktari = 'Sıfırla';
                    break;
                case 'Manuel':
                    yeniBakiye = tutar;
                    duzeltmeMiktari = 'Manuel: ' + tutar.toLocaleString('tr-TR', {minimumFractionDigits: 2});
                    break;
            }

            document.getElementById('<%= lblDuzeltmeMiktari.ClientID %>').innerText = duzeltmeMiktari;
            document.getElementById('<%= lblYeniBakiye.ClientID %>').innerText = yeniBakiye.toLocaleString('tr-TR', {minimumFractionDigits: 2});
        }
    </script>
</asp:Content>
