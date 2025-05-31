<%@ Page Title="Yeni Üretim Başlat: Parti ve Makine Seçimi" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="PartiMakineSecimi.aspx.cs" Inherits="fabrika_Zeytinyagi_PartiMakineSecimi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .list-group-item.selectable { cursor: pointer; transition: background-color 0.15s ease-in-out; }
        .list-group-item.selectable:hover { background-color: #e9ecef; }
        .list-group-item.selectable.active { background-color: #0d6efd; color: white; border-color: #0d6efd; }
        .list-group-item.selectable.active .text-muted { color: rgba(255,255,255,0.75) !important; }
        .machine-status-available { color: #198754; } /* Yeşil */
        .machine-status-busy { color: #dc3545; } /* Kırmızı */
        .machine-status-maintenance { color: #ffc107; } /* Sarı */
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Sayfa Başlığı -->
    <div class="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pt-3 pb-2 mb-3 border-bottom">
        <h1 class="h2"><i class="bi bi-play-circle-fill text-primary"></i> Yeni Üretim Başlat: Parti ve Makine Seçimi</h1>
    </div>

    <div class="row g-4">
        <!-- Sol Sütun: İşlenecek Zeytin Partileri -->
        <div class="col-lg-6">
            <div class="card h-100">
                <div class="card-header">
                    <i class="bi bi-card-list"></i> İşlenecek Zeytin Partisi Seçin
                    <asp:TextBox ID="txtPartiArama" runat="server" CssClass="form-control form-control-sm float-end mt-n1" 
                        Style="width: 200px;" placeholder="Parti No veya Müstahsil Ara..." 
                        onkeyup="filtrelePartiListesi()"></asp:TextBox>
                </div>
                <div class="card-body p-0" style="max-height: 400px; overflow-y: auto;">
                    <asp:RadioButtonList ID="rblPartiListesi" runat="server" CssClass="list-group list-group-flush" 
                        RepeatLayout="Flow" 
                        onchange="partiSecildiginde(this)">
                    </asp:RadioButtonList>
                    <div class="invalid-feedback p-3" id="partiSecimHata" style="display:none;">Lütfen işlenecek bir zeytin partisi seçin.</div>
                </div>
                <div class="card-footer text-muted small">
                    Sadece "Kabul Edildi" veya "Kalite Onaylı" durumundaki partiler üretime alınabilir.
                </div>
            </div>
        </div>

        <!-- Sağ Sütun: Üretim Makinesi Seçimi ve Detaylar -->
        <div class="col-lg-6">
            <div class="card h-100">
                <div class="card-header">
                    <i class="bi bi-motherboard-fill"></i> Kullanılacak Makine/Hat Seçin
                </div>
                <div class="card-body p-0" style="max-height: 400px; overflow-y: auto;">
                    <asp:RadioButtonList ID="rblMakineListesi" runat="server" CssClass="list-group list-group-flush" 
                        RepeatLayout="Flow" 
                        onchange="makineSecildiginde(this)">
                    </asp:RadioButtonList>
                    <div class="invalid-feedback p-3" id="makineSecimHata" style="display:none;">Lütfen üretim yapılacak bir makine seçin.</div>
                </div>
                <div class="card-footer text-muted small">
                    Makinelerin anlık durumlarını ve kapasitelerini kontrol edin.
                </div>
            </div>
        </div>

        <!-- Ek Üretim Bilgileri -->
        <div class="col-12">
            <div class="card">
                <div class="card-header"><i class="bi bi-info-circle"></i> Ek Üretim Bilgileri</div>
                <div class="card-body row g-3">
                    <div class="col-md-4">
                        <label for="txtIslemMiktari" class="form-label">İşlenecek Miktar (Kg)</label>
                        <asp:TextBox ID="txtIslemMiktari" runat="server" CssClass="form-control" TextMode="Number" 
                            placeholder="Partinin tamamı veya bir kısmı" required></asp:TextBox>
                        <div class="form-text">Seçilen partideki max. miktar: <span id="maxPartiMiktar">-</span> Kg</div>
                        <asp:RequiredFieldValidator ID="rfvIslemMiktari" runat="server" 
                            ControlToValidate="txtIslemMiktari" CssClass="invalid-feedback" 
                            ErrorMessage="Lütfen işlenecek miktarı girin." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-4">
                        <label for="txtOperator" class="form-label">Sorumlu Operatör</label>
                        <asp:TextBox ID="txtOperator" runat="server" CssClass="form-control" required></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvOperator" runat="server" 
                            ControlToValidate="txtOperator" CssClass="invalid-feedback" 
                            ErrorMessage="Lütfen operatör adını girin." Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-4">
                        <label for="ddlBeklenenYagTipi" class="form-label">Beklenen Yağ Tipi (Opsiyonel)</label>
                        <asp:DropDownList ID="ddlBeklenenYagTipi" runat="server" CssClass="form-select">
                            <asp:ListItem Value="">Otomatik (Zeytin Türüne Göre)</asp:ListItem>
                            <asp:ListItem Value="NS_ERKEN">Natürel Sızma - Erken Hasat</asp:ListItem>
                            <asp:ListItem Value="NS_OLGUN">Natürel Sızma - Olgun Hasat</asp:ListItem>
                            <asp:ListItem Value="NBIRINCI">Natürel Birinci</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-12">
                        <label for="txtUretimNotlari" class="form-label">Üretim Notları (Opsiyonel)</label>
                        <asp:TextBox ID="txtUretimNotlari" runat="server" CssClass="form-control" TextMode="MultiLine" 
                            Rows="2" placeholder="Özel sıkım koşulları, müşteri talebi vb."></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-12 text-center mt-4">
            <asp:Button ID="btnUretimiBaslat" runat="server" Text="Üretimi Başlat" 
                CssClass="btn btn-primary btn-lg px-5" OnClick="btnUretimiBaslat_Click" 
                OnClientClick="return formDogrula();" />
            <asp:HiddenField ID="hfSelectedPartiID" runat="server" />
            <asp:HiddenField ID="hfSelectedMakineID" runat="server" />
        </div>
    </div>

    <script type="text/javascript">
        // Parti seçildiğinde çalışacak fonksiyon
        function partiSecildiginde(sender) {
            // Tüm parti satırlarındaki active sınıfını kaldır
            var partiSatirlari = document.querySelectorAll('#<%=rblPartiListesi.ClientID%> label');
            partiSatirlari.forEach(function (satir) {
                satir.classList.remove('active');
            });

            // Seçilen parti satırına active sınıfı ekle
            var secilenInput = document.querySelector('input[name="<%=rblPartiListesi.UniqueID%>"]:checked');
            if (secilenInput) {
                secilenInput.closest('label').classList.add('active');
                
                // Parti ID'sini hidden field'a kaydet
                document.getElementById('<%=hfSelectedPartiID.ClientID%>').value = secilenInput.value;
                
                // Parti miktarını göster (varsa)
                var partiText = secilenInput.closest('label').querySelector('small').textContent;
                var miktarMatch = partiText.match(/(\d+(\.\d+)?)\s*Kg/i);
                var maxMiktarSpan = document.getElementById('maxPartiMiktar');
                
                if (miktarMatch && maxMiktarSpan) {
                    maxMiktarSpan.textContent = miktarMatch[1];
                    document.getElementById('<%=txtIslemMiktari.ClientID%>').setAttribute('max', parseFloat(miktarMatch[1]));
                    document.getElementById('<%=txtIslemMiktari.ClientID%>').placeholder = `Max: ${miktarMatch[1]} Kg`;
                } else if (maxMiktarSpan) {
                    maxMiktarSpan.textContent = '-';
                    document.getElementById('<%=txtIslemMiktari.ClientID%>').placeholder = `Partinin tamamı veya bir kısmı`;
                    document.getElementById('<%=txtIslemMiktari.ClientID%>').removeAttribute('max');
                }
            }
        }

        // Makine seçildiğinde çalışacak fonksiyon
        function makineSecildiginde(sender) {
            // Tüm makine satırlarındaki active sınıfını kaldır
            var makineSatirlari = document.querySelectorAll('#<%=rblMakineListesi.ClientID%> label');
            makineSatirlari.forEach(function (satir) {
                satir.classList.remove('active');
            });

            // Seçilen makine satırına active sınıfı ekle
            var secilenInput = document.querySelector('input[name="<%=rblMakineListesi.UniqueID%>"]:checked');
            if (secilenInput) {
                secilenInput.closest('label').classList.add('active');
                
                // Makine ID'sini hidden field'a kaydet
                document.getElementById('<%=hfSelectedMakineID.ClientID%>').value = secilenInput.value;
            }
        }

        // Parti listesini filtrele
        function filtrelePartiListesi() {
            var aramaTerimi = document.getElementById('<%=txtPartiArama.ClientID%>').value.toLowerCase();
            var partiSatirlari = document.querySelectorAll('#<%=rblPartiListesi.ClientID%> label');
            
            partiSatirlari.forEach(function (satir) {
                var satirText = satir.textContent.toLowerCase();
                if (satirText.includes(aramaTerimi)) {
                    satir.style.display = 'flex';
                } else {
                    satir.style.display = 'none';
                }
            });
        }

        // Form doğrulama
        function formDogrula() {
            var isValid = true;
            
            // Parti seçimi kontrolü
            var partiSeciliMi = document.querySelector('input[name="<%=rblPartiListesi.UniqueID%>"]:checked');
            var partiHataMesaji = document.getElementById('partiSecimHata');
            
            if (!partiSeciliMi) {
                partiHataMesaji.style.display = 'block';
                isValid = false;
            } else {
                partiHataMesaji.style.display = 'none';
            }
            
            // Makine seçimi kontrolü
            var makineSeciliMi = document.querySelector('input[name="<%=rblMakineListesi.UniqueID%>"]:checked');
            var makineHataMesaji = document.getElementById('makineSecimHata');
            
            if (!makineSeciliMi) {
                makineHataMesaji.style.display = 'block';
                isValid = false;
            } else {
                makineHataMesaji.style.display = 'none';
                
                // Meşgul veya bakımdaki makine seçilmişse uyar
                var makineStatusBadge = makineSeciliMi.closest('label').querySelector('.badge');
                if (makineStatusBadge && (makineStatusBadge.classList.contains('machine-status-busy') || 
                    makineStatusBadge.classList.contains('machine-status-maintenance'))) {
                    if (!confirm(`${makineSeciliMi.closest('label').querySelector('strong').textContent} şu anda ${makineStatusBadge.textContent.toLowerCase()}. Yine de devam etmek istiyor musunuz?`)) {
                        return false;
                    }
                }
            }
            
            return isValid;
        }

        // Sayfa yüklendiğinde çalışacak kod
        document.addEventListener('DOMContentLoaded', function() {
            // RadioButtonList'e özel stil ve DOM yapısı ekleme
            initializePartiListesi();
            initializeMakineListesi();
        });
        
        function initializePartiListesi() {
            var partiItems = document.querySelectorAll('#<%=rblPartiListesi.ClientID%> input[type="radio"]');
            partiItems.forEach(function(radio) {
                var label = radio.parentElement;
                label.classList.add('list-group-item', 'list-group-item-action', 'selectable', 'd-flex', 'justify-content-between', 'align-items-center');
            });
        }
        
        function initializeMakineListesi() {
            var makineItems = document.querySelectorAll('#<%=rblMakineListesi.ClientID%> input[type="radio"]');
            makineItems.forEach(function(radio) {
                var label = radio.parentElement;
                label.classList.add('list-group-item', 'list-group-item-action', 'selectable', 'd-flex', 'justify-content-between', 'align-items-center');
            });
        }
    </script>
</asp:Content>

