<%@ Page Title="Mal Kabul" Language="C#" MasterPageFile="~/Mobil/MobilMasterPage.master" AutoEventWireup="true" CodeFile="MalKabul.aspx.cs" Inherits="Mobil_MalKabul" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .form-container {
            background: white;
            border-radius: 15px;
            padding: 25px;
            margin-bottom: 20px;
            box-shadow: 0 4px 20px rgba(0,0,0,0.08);
        }
        
        .form-header {
            display: flex;
            align-items: center;
            margin-bottom: 25px;
            padding-bottom: 15px;
            border-bottom: 2px solid #f0f0f0;
        }
        
        .form-header i {
            font-size: 24px;
            color: #4CAF50;
            margin-right: 10px;
        }
        
        .form-header h4 {
            margin: 0;
            color: #333;
            font-weight: 600;
        }
        
        .form-group {
            margin-bottom: 20px;
        }
        
        .form-label {
            font-weight: 600;
            color: #555;
            margin-bottom: 8px;
            display: block;
        }
        
        .form-control, .form-select {
            width: 100%;
            padding: 12px 15px;
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            font-size: 16px;
            transition: border-color 0.3s ease;
        }
        
        .form-control:focus, .form-select:focus {
            border-color: #4CAF50;
            outline: none;
            box-shadow: 0 0 10px rgba(76, 175, 80, 0.1);
        }
        
        .parti-display {
            background: #f8f9fa;
            border: 2px dashed #4CAF50;
            border-radius: 8px;
            padding: 15px;
            text-align: center;
            font-weight: 600;
            color: #4CAF50;
            font-size: 18px;
        }
        
        .zeytin-box-container {
            border: 1px solid #e0e0e0;
            border-radius: 8px;
            padding: 15px;
            margin-bottom: 15px;
            background: #fafafa;
        }
        
        .box-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 10px;
        }
        
        .box-title {
            font-weight: 600;
            color: #333;
        }
        
        .btn-remove-box {
            background: #f44336;
            color: white;
            border: none;
            border-radius: 50%;
            width: 30px;
            height: 30px;
            font-size: 14px;
            cursor: pointer;
        }
        
        .btn-add-box {
            background: #4CAF50;
            color: white;
            border: none;
            border-radius: 8px;
            padding: 10px 20px;
            font-size: 14px;
            margin-bottom: 20px;
            cursor: pointer;
        }
        
        .btn-save {
            background: linear-gradient(135deg, #4CAF50, #45a049);
            color: white;
            border: none;
            border-radius: 12px;
            padding: 15px 30px;
            font-size: 16px;
            font-weight: 600;
            width: 100%;
            margin-top: 20px;
            cursor: pointer;
            box-shadow: 0 4px 15px rgba(76, 175, 80, 0.3);
        }
        
        .btn-save:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(76, 175, 80, 0.4);
        }
        
        .alert {
            padding: 12px 15px;
            border-radius: 8px;
            margin-bottom: 20px;
        }
        
        .alert-success {
            background: #d4edda;
            border: 1px solid #c3e6cb;
            color: #155724;
        }
        
        .alert-danger {
            background: #f8d7da;
            border: 1px solid #f5c6cb;
            color: #721c24;
        }
        
        .required {
            color: #f44336;
        }
        
        .info-box {
            background: #e3f2fd;
            border-left: 4px solid #2196F3;
            padding: 12px 15px;
            margin-bottom: 20px;
            border-radius: 0 8px 8px 0;
        }
        
        .info-box i {
            color: #2196F3;
            margin-right: 8px;
        }
        
        .mustahsil-search-container {
            position: relative;
        }
        
        .search-results {
            position: absolute;
            top: 100%;
            left: 0;
            right: 0;
            max-height: 300px;
            overflow-y: auto;
            background: white;
            border: 1px solid #e0e0e0;
            border-top: none;
            border-radius: 0 0 8px 8px;
            box-shadow: 0 4px 15px rgba(0,0,0,0.1);
            z-index: 1000;
        }
        
        .search-result-item {
            padding: 8px 12px;
            border-bottom: 1px solid #f0f0f0;
            cursor: pointer;
            transition: all 0.2s ease;
        }
        
        .search-result-item:hover {
            background-color: #e3f2fd;
            transform: translateX(2px);
        }
        
        .search-result-item:last-child {
            border-bottom: none;
        }
        
        .badge {
            font-size: 0.75em;
            padding: 4px 8px;
        }

        /* ZeytinBox Grid Sistemi */
        .zeytinbox-search-section {
            margin-bottom: 20px;
        }

        .selected-zeytinboxes-section {
            margin-bottom: 20px;
        }

        .zeytinbox-grid {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 12px;
            margin-top: 10px;
            min-height: 60px;
        }

        .zeytinbox-card {
            background: linear-gradient(135deg, #e8f5e8 0%, #f0f8f0 100%);
            border: 2px solid #28a745;
            border-radius: 12px;
            padding: 12px;
            display: flex;
            align-items: center;
            justify-content: space-between;
            transition: all 0.3s ease;
            position: relative;
            overflow: hidden;
        }

        .zeytinbox-card:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(40, 167, 69, 0.3);
        }

        .zeytinbox-card .card-content {
            flex: 1;
        }

        .zeytinbox-card .card-title {
            font-weight: 600;
            color: #155724;
            margin: 0;
            font-size: 14px;
        }

        .zeytinbox-card .card-id {
            font-size: 12px;
            color: #28a745;
            margin: 2px 0 0 0;
        }

        .zeytinbox-card .remove-btn {
            background: #dc3545;
            color: white;
            border: none;
            border-radius: 50%;
            width: 24px;
            height: 24px;
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
            font-size: 12px;
            transition: all 0.2s ease;
            margin-left: 8px;
        }

        .zeytinbox-card .remove-btn:hover {
            background: #c82333;
            transform: scale(1.1);
        }

        .empty-state {
            grid-column: 1 / -1;
            text-align: center;
            padding: 30px;
            border: 2px dashed #dee2e6;
            border-radius: 12px;
            background: #f8f9fa;
        }

        /* Responsive - Mobile */
        @media (max-width: 768px) {
            .zeytinbox-grid {
                grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
                gap: 8px;
            }
            
            .zeytinbox-card {
                padding: 8px;
            }
            
            .zeytinbox-card .card-title {
                font-size: 13px;
            }
        }
        
        .selected-mustahsil, .selected-zeytinbox {
            margin-top: 10px;
        }
        
        .selected-item {
            background: #e8f5e8;
            border: 1px solid #4CAF50;
            border-radius: 8px;
            padding: 12px 15px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        
        .zeytinbox-search-container {
            position: relative;
        }
        
        .zeytinbox-results {
            position: absolute;
            top: 100%;
            left: 0;
            right: 0;
            max-height: 200px;
            overflow-y: auto;
            background: white;
            border: 1px solid #e0e0e0;
            border-top: none;
            border-radius: 0 0 8px 8px;
            box-shadow: 0 4px 15px rgba(0,0,0,0.1);
            z-index: 1000;
        }
        
        .btn-clear {
            background: #f44336;
            color: white;
            border: none;
            border-radius: 50%;
            width: 25px;
            height: 25px;
            font-size: 12px;
            cursor: pointer;
        }
        
        .no-results {
            padding: 12px 15px;
            text-align: center;
            color: #666;
            font-style: italic;
        }
        
        .badge {
            display: inline-block;
            padding: 0.25em 0.6em;
            font-size: 0.75em;
            font-weight: 700;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            border-radius: 0.375rem;
        }
        
        .bg-success {
            background-color: #198754 !important;
        }
        
        .bg-danger {
            background-color: #dc3545 !important;
        }
        
        .ms-2 {
            margin-left: 0.5rem !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid">
        <!-- Başarı/Hata Mesajları -->
        <asp:Panel ID="pnlMesaj" runat="server" Visible="false">
            <div class="alert" id="divMesaj">
                <asp:Label ID="lblMesaj" runat="server"></asp:Label>
            </div>
        </asp:Panel>

        <!-- Ana Form -->
        <div class="form-container">
            <div class="form-header">
                <i class="fas fa-truck-loading"></i>
                <h4>Yeni Mal Kabul</h4>
            </div>

            <!-- Bilgi Kutusu -->
            <div class="info-box">
                <i class="fas fa-info-circle"></i>
                <strong>Bilgi:</strong> Parti numarası otomatik oluşturulacak. ZeytinBox'ları seçerek mal kabulü yapın.
            </div>

            <!-- Müştahsil Arama -->
            <div class="form-group">
                <label class="form-label">
                    <i class="fas fa-user me-2"></i>Müştahsil Ara <span class="required">*</span>
                </label>
                <div class="mustahsil-search-container">
                    <asp:TextBox ID="txtMustahsilAra" runat="server" CssClass="form-control" 
                        placeholder="Ad, soyad, telefon veya email ile arama yapın..." 
                        AutoComplete="off"></asp:TextBox>
                    <asp:HiddenField ID="hdnSelectedMustahsilID" runat="server" />
                    <div id="mustahsilSonuclar" class="search-results" style="display: none;"></div>
                </div>
                <div id="secilenMustahsil" class="selected-mustahsil" style="display: none;">
                    <div class="selected-item">
                        <span id="secilenMustahsilBilgi"></span>
                        <button type="button" onclick="clearMustahsilSecimi()" class="btn-clear">
                            <i class="fas fa-times"></i>
                        </button>
                    </div>
                </div>
            </div>

            <!-- Ürün Seçimi -->
            <div class="form-group">
                <label class="form-label">
                    <i class="fas fa-leaf me-2"></i>Ürün <span class="required">*</span>
                </label>
                <asp:DropDownList ID="ddlUrun" runat="server" CssClass="form-select">
                    <asp:ListItem Value="">-- Ürün Seçin --</asp:ListItem>
                </asp:DropDownList>
            </div>

            <!-- Parti No (Otomatik) -->
            <div class="form-group">
                <label class="form-label">
                    <i class="fas fa-barcode me-2"></i>Parti Numarası (Otomatik)
                </label>
                <div class="parti-display">
                    <asp:Label ID="lblPartiNo" runat="server" Text="Müştahsil seçildikten sonra oluşturulacak"></asp:Label>
                </div>
            </div>

            <!-- Plaka No -->
            <div class="form-group">
                <label class="form-label">
                    <i class="fas fa-truck me-2"></i>Plaka No (Opsiyonel)
                </label>
                <asp:TextBox ID="txtPlaka" runat="server" CssClass="form-control" 
                    placeholder="Örn: 34ABC123 (Boş bırakılabilir)" MaxLength="15"></asp:TextBox>
            </div>

            <!-- Geliş Kg (Opsiyonel) -->
            <div class="form-group">
                <label class="form-label">
                    <i class="fas fa-weight me-2"></i>Geliş Kg (Opsiyonel)
                </label>
                <asp:TextBox ID="txtGelisKg" runat="server" CssClass="form-control" 
                    placeholder="Boş bırakılabilir" TextMode="Number"></asp:TextBox>
            </div>
        </div>

        <!-- ZeytinBox Yönetimi -->
        <div class="form-container">
            <div class="form-header">
                <i class="fas fa-boxes"></i>
                <h4>ZeytinBox Seçimi</h4>
            </div>

            <!-- Tek ZeytinBox Arama Alanı -->
            <div class="zeytinbox-search-section">
                <label class="form-label">ZeytinBox Ara:</label>
                <div class="zeytinbox-search-container">
                    <input type="text" id="zeytinBoxArama" class="form-control" 
                        placeholder="ZeytinBox numarası ile arama yapın..." autocomplete="off">
                    <div class="search-results zeytinbox-results" id="zeytinBoxSonuclar" style="display: none;"></div>
                </div>
            </div>

            <!-- Seçilen ZeytinBox'lar -->
            <div class="selected-zeytinboxes-section">
                <label class="form-label">Seçilen ZeytinBox'lar:</label>
                <div id="secilenZeytinBoxlar" class="zeytinbox-grid">
                    <div class="empty-state text-muted">
                        <i class="fas fa-search me-2"></i>
                        Henüz ZeytinBox seçilmedi. Yukarıdan arama yaparak ekleyin.
                    </div>
                </div>
                <asp:HiddenField ID="hdnSecilenZeytinBoxlar" runat="server" />
            </div>

            <!-- Test ve Kaydet Butonları -->
            <div class="d-flex gap-2 mb-3">
                <button type="button" class="btn btn-info flex-fill" onclick="zeytinBoxDurumKontrol()">
                    <i class="fas fa-info-circle me-2"></i>ZeytinBox Durum Kontrol
                </button>
            </div>
            
            <asp:Button ID="btnKaydet" runat="server" CssClass="btn-save" 
                Text="Mal Kabulü Kaydet" OnClick="btnKaydet_Click" />
        </div>
    </div>

    <script>
        // Seçilen ZeytinBox'ları tutacak array
        let secilenZeytinBoxlar = [];
        let zeytinBoxAramaTimer;

        function setupZeytinBoxArama() {
            const aramaInput = document.getElementById('zeytinBoxArama');
            const sonuclarDiv = document.getElementById('zeytinBoxSonuclar');

            // Arama input'una event listener ekle
            aramaInput.addEventListener('input', function() {
                const aramaMetni = this.value.trim();
                
                clearTimeout(zeytinBoxAramaTimer);
                
                if (aramaMetni.length >= 1) {
                    zeytinBoxAramaTimer = setTimeout(() => {
                        zeytinBoxAra(aramaMetni);
                    }, 200);
                } else {
                    sonuclarDiv.style.display = 'none';
                }
            });

            // Dış tıklama ile sonuçları gizle
            document.addEventListener('click', function(e) {
                if (!e.target.closest('.zeytinbox-search-container')) {
                    sonuclarDiv.style.display = 'none';
                }
            });
        }

        function zeytinBoxAra(aramaMetni) {
            const sonuclarDiv = document.getElementById('zeytinBoxSonuclar');
            
            console.log('ZeytinBox arama başlatıldı:', aramaMetni);
            
            // PageMethods var mı kontrol et
            if (typeof PageMethods === 'undefined') {
                console.error('PageMethods tanımlı değil!');
                sonuclarDiv.innerHTML = '<div class="no-results">PageMethods yüklenemiyor</div>';
                sonuclarDiv.style.display = 'block';
                return;
            }
            
            if (typeof PageMethods.ZeytinBoxAra === 'undefined') {
                console.error('PageMethods.ZeytinBoxAra tanımlı değil!');
                sonuclarDiv.innerHTML = '<div class="no-results">ZeytinBoxAra metodu bulunamadı</div>';
                sonuclarDiv.style.display = 'block';
                return;
            }
            
            PageMethods.ZeytinBoxAra(aramaMetni, function(result) {
                console.log('ZeytinBox arama sonucu:', result);
                
                if (result && result.length > 0) {
                    let html = '';
                    result.forEach(function(box) {
                        // Zaten seçilmiş olanları filtrele
                        const mevcutMu = secilenZeytinBoxlar.some(secilen => secilen.ZeytinBoxKasaID === box.ZeytinBoxKasaID);
                        if (mevcutMu) return;
                        
                        const statusBadge = box.Status === 'Müsait' ? 
                            '<span class="badge bg-success">Müsait</span>' : 
                            '<span class="badge bg-danger">Dolu</span>';
                            
                        html += `
                            <div class="search-result-item" onclick="selectZeytinBox(${box.ZeytinBoxKasaID}, '${box.ZeytinBoxNo}', '${box.Status}')">
                                <div class="d-flex justify-content-between align-items-center">
                                    <span><strong>ZeytinBox ${box.ZeytinBoxNo}</strong></span>
                                    ${statusBadge}
                                </div>
                            </div>
                        `;
                    });
                    
                    if (html) {
                        sonuclarDiv.innerHTML = html;
                        sonuclarDiv.style.display = 'block';
                        console.log('Sonuçlar gösterildi');
                    } else {
                        sonuclarDiv.innerHTML = '<div class="no-results">Tüm sonuçlar zaten seçilmiş</div>';
                        sonuclarDiv.style.display = 'block';
                    }
                } else {
                    console.log('Sonuç bulunamadı');
                    sonuclarDiv.innerHTML = '<div class="no-results">ZeytinBox bulunamadı (Arama: "' + aramaMetni + '")</div>';
                    sonuclarDiv.style.display = 'block';
                }
            }, function(error) {
                console.error('ZeytinBox arama hatası:', error);
                sonuclarDiv.innerHTML = '<div class="no-results">Arama hatası: ' + (error.get_message ? error.get_message() : error) + '</div>';
                sonuclarDiv.style.display = 'block';
            });
        }

        function selectZeytinBox(zeytinBoxID, zeytinBoxNo, status) {
            // Duplicate kontrolü
            const mevcutMu = secilenZeytinBoxlar.some(box => box.ZeytinBoxKasaID === zeytinBoxID);
            if (mevcutMu) {
                console.log('Bu ZeytinBox zaten seçilmiş:', zeytinBoxNo);
                return;
            }
            
            // Array'e ekle
            secilenZeytinBoxlar.push({
                ZeytinBoxKasaID: zeytinBoxID,
                ZeytinBoxNo: zeytinBoxNo,
                Status: status
            });
            
            // Arama kutusunu temizle
            document.getElementById('zeytinBoxArama').value = '';
            
            // Sonuçları gizle
            document.getElementById('zeytinBoxSonuclar').style.display = 'none';
            
            // Grid'i güncelle
            updateZeytinBoxGrid();
            
            // Hidden field'ı güncelle
            updateHiddenField();
            
            console.log('ZeytinBox eklendi:', zeytinBoxNo, 'Toplam:', secilenZeytinBoxlar.length);
        }

        function removeZeytinBox(zeytinBoxID) {
            // Array'den çıkar
            secilenZeytinBoxlar = secilenZeytinBoxlar.filter(box => box.ZeytinBoxKasaID !== zeytinBoxID);
            
            // Grid'i güncelle
            updateZeytinBoxGrid();
            
            // Hidden field'ı güncelle
            updateHiddenField();
            
            console.log('ZeytinBox kaldırıldı. Kalan:', secilenZeytinBoxlar.length);
        }

        function updateZeytinBoxGrid() {
            const gridContainer = document.getElementById('secilenZeytinBoxlar');
            
            if (secilenZeytinBoxlar.length === 0) {
                gridContainer.innerHTML = `
                    <div class="empty-state text-muted">
                        <i class="fas fa-search me-2"></i>
                        Henüz ZeytinBox seçilmedi. Yukarıdan arama yaparak ekleyin.
                    </div>
                `;
                return;
            }
            
            let html = '';
            secilenZeytinBoxlar.forEach(function(box) {
                html += `
                    <div class="zeytinbox-card">
                        <div class="card-content">
                            <div class="card-title">ZeytinBox ${box.ZeytinBoxNo}</div>
                            <div class="card-id">ID: ${box.ZeytinBoxKasaID}</div>
                        </div>
                        <button type="button" class="remove-btn" onclick="removeZeytinBox(${box.ZeytinBoxKasaID})" title="Kaldır">
                            <i class="fas fa-times"></i>
                        </button>
                    </div>
                `;
            });
            
            gridContainer.innerHTML = html;
        }

        function updateHiddenField() {
            const hiddenField = document.getElementById('<%= hdnSecilenZeytinBoxlar.ClientID %>');
            hiddenField.value = JSON.stringify(secilenZeytinBoxlar.map(box => box.ZeytinBoxKasaID));
        }

        // ZeytinBox durum kontrol fonksiyonu
        function zeytinBoxDurumKontrol() {
            console.log('ZeytinBox durum kontrolü başlatıldı');
            
            if (typeof PageMethods === 'undefined' || typeof PageMethods.ZeytinBoxDurum === 'undefined') {
                alert('PageMethods.ZeytinBoxDurum metodu bulunamadı');
                return;
            }
            
            PageMethods.ZeytinBoxDurum(function(result) {
                console.log('ZeytinBox durum sonucu:', result);
                
                if (result.Hata) {
                    alert('Hata: ' + result.Hata);
                } else {
                    const mesaj = `ZeytinBox Durum Raporu:
Şirket ID: ${result.SirketID}
Toplam ZeytinBox: ${result.ToplamBox}
Aktif ZeytinBox: ${result.AktifBox}
Pasif ZeytinBox: ${result.PasifBox}
Müsait ZeytinBox: ${result.MusaitBox}  
Dolu ZeytinBox: ${result.DoluBox}`;
                    
                    alert(mesaj);
                }
            }, function(error) {
                console.error('ZeytinBox durum hatası:', error);
                alert('Durum kontrolü hatası: ' + (error.get_message ? error.get_message() : error));
            });
        }

        // Müştahsil arama fonksiyonları
        let aramaTimer;
        let selectedMustahsilID = null;

        function setupMustahsilArama() {
            const txtArama = document.getElementById('<%= txtMustahsilAra.ClientID %>');
            const sonuclarDiv = document.getElementById('mustahsilSonuclar');
            
            txtArama.addEventListener('input', function() {
                clearTimeout(aramaTimer);
                const aramaMetni = this.value.trim();
                
                if (aramaMetni.length >= 2) {
                    aramaTimer = setTimeout(() => {
                        mustahsilAra(aramaMetni);
                    }, 300);
                } else {
                    sonuclarDiv.style.display = 'none';
                }
            });

            // Input dışına tıklandığında sonuçları gizle
            document.addEventListener('click', function(e) {
                if (!e.target.closest('.mustahsil-search-container')) {
                    sonuclarDiv.style.display = 'none';
                }
            });
        }

        function mustahsilAra(aramaMetni) {
            const sonuclarDiv = document.getElementById('mustahsilSonuclar');
            
            // AJAX ile arama yap
            PageMethods.MustahsilAra(aramaMetni, function(result) {
                if (result && result.length > 0) {
                    let html = '';
                    result.forEach(function(mustahsil) {
                        html += `
                            <div class="search-result-item" onclick="selectMustahsil(${mustahsil.MustahsilID}, '${mustahsil.AdSoyad}', '${mustahsil.Telefon}', '${mustahsil.Email}')">
                                <div class="result-name">${mustahsil.AdSoyad}</div>
                                <div class="result-details">${mustahsil.Telefon} | ${mustahsil.Email}</div>
                            </div>
                        `;
                    });
                    sonuclarDiv.innerHTML = html;
                    sonuclarDiv.style.display = 'block';
                } else {
                    sonuclarDiv.innerHTML = '<div class="no-results">Sonuç bulunamadı</div>';
                    sonuclarDiv.style.display = 'block';
                }
            }, function(error) {
                console.error('Arama hatası:', error);
                sonuclarDiv.style.display = 'none';
            });
        }

        function selectMustahsil(mustahsilID, adSoyad, telefon, email) {
            selectedMustahsilID = mustahsilID;
            
            // Hidden field'a ID'yi kaydet
            document.getElementById('<%= hdnSelectedMustahsilID.ClientID %>').value = mustahsilID;
            
            // Arama kutusunu temizle
            document.getElementById('<%= txtMustahsilAra.ClientID %>').value = '';
            
            // Sonuçları gizle
            document.getElementById('mustahsilSonuclar').style.display = 'none';
            
            // Seçilen müştahsili göster
            document.getElementById('secilenMustahsilBilgi').innerHTML = `
                <strong>${adSoyad}</strong><br>
                <small>${telefon} | ${email}</small>
            `;
            document.getElementById('secilenMustahsil').style.display = 'block';
            
            // Parti numarası oluştur
            partiNoOlustur();
        }

        function clearMustahsilSecimi() {
            selectedMustahsilID = null;
            document.getElementById('<%= hdnSelectedMustahsilID.ClientID %>').value = '';
            document.getElementById('secilenMustahsil').style.display = 'none';
            document.getElementById('<%= txtMustahsilAra.ClientID %>').value = '';
            
            // Parti numarasını temizle
            document.querySelector('.parti-display').innerHTML = 'Müştahsil seçildikten sonra oluşturulacak';
        }

        function partiNoOlustur() {
            if (selectedMustahsilID) {
                PageMethods.PartiNoOlustur(function(result) {
                    document.querySelector('.parti-display').innerHTML = result;
                }, function(error) {
                    console.error('Parti no oluşturma hatası:', error);
                });
            }
        }

        // Sayfa yüklendiğinde çalıştır
        document.addEventListener('DOMContentLoaded', function() {
            // Arama sistemlerini başlat
            setupMustahsilArama();
            setupZeytinBoxArama();
            
            // ZeytinBox grid'ini başlat
            updateZeytinBoxGrid();
        });
    </script>
</asp:Content>

