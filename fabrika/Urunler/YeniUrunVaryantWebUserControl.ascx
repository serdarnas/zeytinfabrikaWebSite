<%@ Control Language="C#" AutoEventWireup="true" CodeFile="YeniUrunVaryantWebUserControl.ascx.cs" Inherits="fabrika_Urunler_YeniUrunVaryantWebUserControl" %>

<!-- Varyant UserControl CSS ve JS bağımlılıkları -->
<link rel="stylesheet" type="text/css" href="/App_Themes/serdarnas_admin_flat/assets/jquery-multi-select/css/multi-select.css" />

<!-- Multi-Select için gerekli JavaScript dosyaları -->
<script type="text/javascript">
    // jQuery yüklü mü kontrol et
    if (typeof jQuery === 'undefined') {
        document.write('<script src="https://code.jquery.com/jquery-3.6.0.min.js"><\/script>');
    }
</script>

<!-- Multi-Select plugin'lerini yükle -->
<script type="text/javascript">
    // Plugin dosyalarını yükle
    function loadMultiSelectPlugins() {
        console.log('Multi-select plugin\'leri yükleniyor...');
        
        // jQuery MultiSelect plugin
        $.getScript('/App_Themes/serdarnas_admin_flat/assets/jquery-multi-select/js/jquery.multi-select.js')
            .done(function() {
                console.log('MultiSelect plugin yüklendi');
                
                // QuickSearch plugin
                $.getScript('/App_Themes/serdarnas_admin_flat/assets/jquery-multi-select/js/jquery.quicksearch.js')
                    .done(function() {
                        console.log('QuickSearch plugin yüklendi');
                        console.log('MultiSelect plugin kontrol:', typeof $.fn.multiSelect);
                        console.log('QuickSearch plugin kontrol:', typeof $.fn.quicksearch);
                    })
                    .fail(function() {
                        console.error('QuickSearch plugin yüklenemedi');
                    });
            })
            .fail(function() {
                console.error('MultiSelect plugin yüklenemedi');
                
                // CDN'den dene
                console.log('CDN\'den MultiSelect yükleniyor...');
                $.getScript('https://cdnjs.cloudflare.com/ajax/libs/jquery-multi-select/0.9.12/js/jquery.multi-select.min.js')
                    .done(function() {
                        console.log('CDN MultiSelect yüklendi');
                    })
                    .fail(function() {
                        console.error('CDN MultiSelect de yüklenemedi');
                    });
            });
    }
    
    // jQuery hazır olduğunda plugin'leri yükle
    $(document).ready(function() {
        loadMultiSelectPlugins();
    });
</script>

<!-- Multi-Select CSS düzeltmeleri -->
<style type="text/css">
    /* Multi-Select için özel CSS düzeltmeleri */
    .ms-container {
        width: 100% !important;
    }
    
    .ms-container .ms-selectable, .ms-container .ms-selection {
        width: 48% !important;
        min-height: 200px !important;
    }
    
    .ms-container .ms-selectable {
        margin-right: 2% !important;
    }
    
    .ms-container .ms-selection {
        margin-left: 2% !important;
    }
    
    .ms-container .ms-list {
        border: 1px solid #ccc !important;
        border-radius: 4px !important;
        background: white !important;
    }
    
    .ms-container .ms-list .ms-elem-selectable,
    .ms-container .ms-list .ms-elem-selection {
        border: none !important;
        padding: 8px 12px !important;
        margin: 0 !important;
        line-height: 1.4 !important;
        cursor: pointer !important;
    }
    
    .ms-container .ms-list .ms-elem-selectable:hover,
    .ms-container .ms-list .ms-elem-selection:hover {
        background-color: #f5f5f5 !important;
    }
    
    .ms-container .ms-list .ms-elem-selectable.ms-selected {
        background-color: #5cb85c !important;
        color: white !important;
    }
    
    .ms-container .ms-list .ms-elem-selection.ms-selected {
        background-color: #337ab7 !important;
        color: white !important;
    }
</style>

<!-- Varyant Paneli -->
<div class="form-horizontal">
    <div class="form-group">
        <div class="col-sm-12">
            <div class="checkbox">
                <label>
                    <input type="checkbox" id="chkVaryantKullan" onchange="toggleVaryantPanel()"> Bu ürün için varyant kullan
                </label>
            </div>
        </div>
    </div>
    
    <div id="pnlVaryantlar" style="display: none;">
        <!-- Varyant Türleri Seçimi - Advanced Form Components Tarzı -->
        <div class="form-group">
            <label class="col-sm-3 control-label"><i class="fa fa-tags"></i> Varyant Türü Seçimi</label>
            <div class="col-sm-9">
                <div class="input-group">
                    <span class="input-group-addon"><i class="fa fa-tags"></i></span>
                    <select id="ddlVaryantTurleri" class="form-control">
                        <option value="">Varyant türü seçin...</option>
                    </select>
                    <span class="input-group-btn">
                        <button type="button" id="btnVaryantTuruEkle" class="btn btn-primary" onclick="varyantTuruEkle()">
                            <i class="fa fa-plus"></i> Ekle
                        </button>
                    </span>
                </div>
                <span class="help-block">Dropdown'dan varyant türü seçin ve "Ekle" butonuna tıklayın</span>
                
                <!-- Seçilen Varyant Türlerinin Gösterimi -->
                <div id="secilenVaryantTurleriContainer" style="margin-top: 15px;">
                    <div id="secilenVaryantTurleri">
                        <!-- Seçilen varyant türleri burada gösterilecek -->
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Seçilen Varyant Türleri için Multi-Select'ler - Advanced Form Components Tarzı -->
        <div id="varyantDegerleriContainer">
            <div class="alert alert-info" style="margin: 15px 0;">
                <i class="fa fa-info-circle"></i> 
                <strong>Varyant Değerleri Seçimi:</strong>
                <ol style="margin: 5px 0 0 20px;">
                    <li>Yukarıdan bir varyant türü seçin ve "Ekle" butonuna tıklayın</li>
                    <li>Eklenen varyant türü için multi-select ile değerleri seçin</li>
                    <li>Tüm seçimler tamamlandıktan sonra kombinasyonları oluşturun</li>
                </ol>
            </div>
        </div>
        
        <!-- Varyant Kombinasyonları Oluştur - Advanced Form Components Tarzı -->
        <div class="form-group" style="margin-top: 30px;">
            <div class="col-md-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h4><i class="fa fa-magic"></i> Varyant Kombinasyonları</h4>
                    </div>
                    <div class="panel-body">
                        <p class="help-block">
                            <i class="fa fa-info-circle"></i> 
                            Seçtiğiniz varyant türleri ve değerlerinden tüm olası kombinasyonları otomatik oluşturacaktır.
                        </p>
                        <button type="button" class="btn btn-primary btn-lg" onclick="olusturVaryantKombinasyonlari()">
                            <i class="fa fa-magic"></i> Kombinasyonları Oluştur
                        </button>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Oluşturulan Varyant Kombinasyonları - Advanced Form Components Tarzı -->
        <div class="form-group">
            <div class="col-md-12">
                <div class="panel panel-success">
                    <div class="panel-heading">
                        <h4><i class="fa fa-th"></i> Oluşturulan Varyant Kombinasyonları</h4>
                        <span class="tools pull-right">
                            <a href="javascript:;" class="icon-chevron-down"></a>
                        </span>
                    </div>
                    <div class="panel-body">
                        <div class="alert alert-warning" style="margin-bottom: 15px;">
                            <i class="fa fa-exclamation-triangle"></i> 
                            <strong>Dikkat:</strong> Her varyant kombinasyonu için fiyat ve stok bilgilerini girin.
                        </div>
                        <div class="table-responsive varyant-kombinasyon-tablo">
                            <table class="table table-striped table-bordered table-hover" id="tblVaryantKombinasyonlari">
                                <thead class="bg-primary">
                                    <tr>
                                        <th><i class="fa fa-tags"></i> Varyant</th>
                                        <th><i class="fa fa-barcode"></i> Barkod</th>
                                        <th><i class="fa fa-cubes"></i> Stok</th>
                                        <th><i class="fa fa-shopping-cart"></i> Alış Fiyatı</th>
                                        <th><i class="fa fa-money"></i> Satış Fiyatı</th>
                                        <th><i class="fa fa-money"></i> Perakende Fiyatı</th>
                                        <th><i class="fa fa-toggle-on"></i> Durum</th>
                                        <th><i class="fa fa-image"></i> Görsel</th>
                                        <th><i class="fa fa-cogs"></i> İşlem</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <!-- Kombinasyonlar buraya eklenir -->
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Varyantları Kaydet - Advanced Form Components Tarzı -->
        <div class="form-group" style="margin-top: 30px;">
            <div class="col-md-12">
                <div class="panel panel-info">
                    <div class="panel-heading">
                        <h4><i class="fa fa-save"></i> Varyant Kaydetme</h4>
                    </div>
                    <div class="panel-body text-center">
                        <p class="help-block">
                            <i class="fa fa-info-circle"></i> 
                            Tüm varyant kombinasyonları ve bilgileri kontrol edildikten sonra kaydedin.
                        </p>
                        <button type="button" class="btn btn-success btn-lg" onclick="kaydetVaryantlar()" style="min-width: 200px;">
                            <i class="fa fa-save"></i> Varyantları Kaydet
                        </button>
                        <br/><br/>
                        <button type="button" class="btn btn-warning" onclick="temizleVaryantVerileri()" style="min-width: 150px;">
                            <i class="fa fa-refresh"></i> Temizle
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<!-- Varyant Özel Stilleri - Advanced Form Components Tarzı -->
<style type="text/css">
    /* Ana Container */
    .varyant-container {
        background: #ffffff;
        border: 1px solid #e5e5e5;
        border-radius: 6px;
        padding: 20px;
        margin-bottom: 25px;
        box-shadow: 0 1px 3px rgba(0,0,0,0.1);
    }
    
    /* Seçilen Varyant Türleri */
    .varyant-tur-item {
        margin-bottom: 8px;
        border-radius: 4px;
        position: relative;
        transition: all 0.3s ease;
    }
    
    .varyant-tur-item:hover {
        box-shadow: 0 2px 5px rgba(0,0,0,0.1);
    }
    
    .varyant-tur-item .close {
        opacity: 0.6;
        cursor: pointer;
        transition: opacity 0.3s ease;
        font-size: 18px;
    }
    
    .varyant-tur-item .close:hover {
        opacity: 1;
        color: #d9534f;
    }
    
    /* Multi-Select Container */
    .varyant-deger-container {
        margin-top: 15px;
        padding: 15px;
        background: #f8f9fa;
        border: 1px solid #dee2e6;
        border-radius: 6px;
        transition: all 0.3s ease;
    }
    
    .varyant-deger-container:hover {
        border-color: #007bff;
        box-shadow: 0 0 0 0.2rem rgba(0,123,255,.25);
    }
    
    /* Multi-Select Styling */
    .ms-container {
        width: 100%;
    }
    
    .ms-container .ms-list {
        border: 1px solid #ced4da;
        border-radius: 4px;
        height: 200px;
    }
    
    .ms-container .ms-selectable {
        margin-right: 10px;
    }
    
    .ms-container .ms-selection {
        margin-left: 10px;
    }
    
    .ms-container .ms-list .ms-elem-selectable,
    .ms-container .ms-list .ms-elem-selection {
        padding: 8px 12px;
        border-bottom: 1px solid #f1f1f1;
        cursor: pointer;
        transition: background-color 0.2s ease;
    }
    
    .ms-container .ms-list .ms-elem-selectable:hover,
    .ms-container .ms-list .ms-elem-selection:hover {
        background-color: #e9ecef;
    }
    
    .ms-container .ms-list .ms-elem-selectable.ms-hover,
    .ms-container .ms-list .ms-elem-selection.ms-hover {
        background-color: #007bff;
        color: white;
    }
    
    .ms-container .ms-selectable li.ms-elem-selectable.ms-selected {
        background-color: #28a745;
        color: white;
    }
    
    .ms-container .ms-selection li.ms-elem-selection.ms-selected {
        background-color: #17a2b8;
        color: white;
    }
    
    /* Search Input Styling */
    .ms-container .search-input {
        margin-bottom: 5px;
        border-radius: 4px;
        border: 1px solid #ced4da;
        font-size: 12px;
    }
    
    /* Panel Styling */
    .panel-primary > .panel-heading {
        background: linear-gradient(to bottom, #337ab7 0%, #2e6da4 100%);
        border-color: #2e6da4;
    }
    
    .panel-success > .panel-heading {
        background: linear-gradient(to bottom, #5cb85c 0%, #449d44 100%);
        border-color: #449d44;
    }
    
    /* Tablo Styling */
    .varyant-kombinasyon-tablo {
        margin-top: 15px;
    }
    
    .varyant-kombinasyon-tablo table {
        font-size: 13px;
    }
    
    .varyant-kombinasyon-tablo thead.bg-primary th {
        background: linear-gradient(to bottom, #337ab7 0%, #2e6da4 100%);
        color: white;
        font-weight: bold;
        text-align: center;
        padding: 12px 8px;
    }
    
    .varyant-kombinasyon-tablo td {
        vertical-align: middle;
        text-align: center;
        padding: 8px;
    }
    
    .varyant-kombinasyon-tablo input.form-control {
        font-size: 12px;
        padding: 6px 8px;
        height: 32px;
        border-radius: 4px;
    }
    
    .varyant-kombinasyon-tablo .btn {
        padding: 4px 8px;
        font-size: 11px;
    }
    
    /* Custom Header for Multi-Select */
    .ms-container .ms-selectable .ms-header,
    .ms-container .ms-selection .ms-header {
        background: linear-gradient(to bottom, #f8f9fa 0%, #e9ecef 100%);
        border-bottom: 1px solid #dee2e6;
        padding: 10px;
        font-weight: bold;
        text-align: center;
        color: #495057;
    }
    
    /* Advanced Multi-Select Container */
    .advanced-multiselect-container {
        display: flex;
        gap: 15px;
        margin-top: 10px;
        width: 100%;
    }
    
    .multiselect-panel {
        flex: 1;
        border: 1px solid #dee2e6;
        border-radius: 6px;
        background: #ffffff;
        box-shadow: 0 1px 3px rgba(0,0,0,0.1);
    }
    
    .multiselect-panel .panel-header {
        background: linear-gradient(to bottom, #f8f9fa 0%, #e9ecef 100%);
        border-bottom: 1px solid #dee2e6;
        padding: 10px;
        border-radius: 6px 6px 0 0;
    }
    
    .multiselect-panel .panel-body {
        padding: 0;
        height: 250px;
        overflow-y: auto;
    }
    
    .multiselect-panel .panel-title {
        background: linear-gradient(to bottom, #007bff 0%, #0056b3 100%);
        color: white;
        padding: 8px 12px;
        margin: 0;
        font-weight: bold;
        text-align: center;
        font-size: 13px;
    }
    
    .multiselect-list {
        list-style: none;
        padding: 0;
        margin: 0;
    }
    
    .multiselect-item {
        padding: 10px 12px;
        border-bottom: 1px solid #f1f1f1;
        cursor: pointer;
        transition: all 0.2s ease;
        background: #ffffff;
    }
    
    .multiselect-item:hover {
        background: linear-gradient(to right, #e3f2fd 0%, #bbdefb 100%);
        color: #1976d2;
        font-weight: 500;
    }
    
    .multiselect-item:last-child {
        border-bottom: none;
    }
    
    .available-list .multiselect-item:hover {
        background: linear-gradient(to right, #e8f5e8 0%, #c8e6c9 100%);
        color: #388e3c;
    }
    
    .selected-list .multiselect-item:hover {
        background: linear-gradient(to right, #fff3e0 0%, #ffcc80 100%);
        color: #f57c00;
    }
    
    .multiselect-panel .search-input {
        width: 100%;
        border: none;
        border-radius: 0;
        font-size: 12px;
        padding: 8px 12px;
    }
    
    .multiselect-panel .search-input:focus {
        outline: none;
        box-shadow: inset 0 1px 3px rgba(0,0,0,0.1);
        border-color: #007bff;
    }
    
    /* Responsive Multi-Select */
    @media (max-width: 768px) {
        .advanced-multiselect-container {
            flex-direction: column;
        }
        
        .multiselect-panel .panel-body {
            height: 180px;
        }
    }
    
    /* Alert Improvements */
    .alert {
        border-radius: 6px;
    }
    
    .alert-info {
        background-color: #e3f2fd;
        border-color: #bbdefb;
        color: #0277bd;
    }
    
    .alert-success {
        background-color: #e8f5e8;
        border-color: #c3e6c3;
        color: #2e7d2e;
    }
    
    .alert-warning {
        background-color: #fff3cd;
        border-color: #ffeaa7;
        color: #856404;
    }
    
    /* Button Improvements */
    .btn-primary {
        background: linear-gradient(to bottom, #337ab7 0%, #2e6da4 100%);
        border-color: #2e6da4;
        transition: all 0.3s ease;
    }
    
    .btn-primary:hover {
        background: linear-gradient(to bottom, #2e6da4 0%, #265a88 100%);
        border-color: #265a88;
        transform: translateY(-1px);
        box-shadow: 0 4px 8px rgba(0,0,0,0.2);
    }
    
    .btn-lg {
        padding: 12px 24px;
        font-size: 16px;
        font-weight: bold;
    }
    
    /* Responsive Improvements */
    @media (max-width: 768px) {
        .varyant-kombinasyon-tablo {
            font-size: 11px;
        }
        
        .varyant-kombinasyon-tablo input.form-control {
            font-size: 10px;
            padding: 4px 6px;
            height: 28px;
        }
        
        .ms-container .ms-list {
            height: 150px;
        }
    }
</style>

<!-- Varyant JavaScript Fonksiyonları -->
<script type="text/javascript">
    // Global değişkenler
    var varyantTurleri = [];
    var varyantDegerleri = {};
    var varyantKombinasyonlari = [];
    var multiSelectInstances = {};

    $(document).ready(function () {
        console.log('UserControl document ready');
        
        // jQuery ve plugin'lerin yüklendiğini kontrol et
        console.log('jQuery version:', $.fn.jquery);
        console.log('MultiSelect plugin:', typeof $.fn.multiSelect);
        console.log('QuickSearch plugin:', typeof $.fn.quicksearch);
        
        // UserControl yüklendiğinde varyant türlerini yükle
        yukleVaryantTurleri();
        
        // Dropdown için Enter tuş desteği
        $('#ddlVaryantTurleri').on('keypress', function(e) {
            if (e.which === 13) { // Enter tuşu
                e.preventDefault();
                varyantTuruEkle();
            }
        });
        
        // Dropdown değiştiğinde otomatik ekleme seçeneği (isteğe bağlı)
        $('#ddlVaryantTurleri').on('change', function() {
            if ($(this).val()) {
                // Otomatik ekleme yapmak istemiyorsanız bu satırı kaldırın
                // varyantTuruEkle();
            }
        });
        
        // Multi-select CSS'lerinin yüklendiğini kontrol et
        setTimeout(function() {
            var cssLoaded = false;
            $('link[rel="stylesheet"]').each(function() {
                if (this.href.indexOf('multi-select.css') > -1) {
                    cssLoaded = true;
                }
            });
            console.log('Multi-select CSS yüklendi:', cssLoaded);
        }, 1000);
    });

    // Varyant panelini göster/gizle
    function toggleVaryantPanel() {
        var checkbox = document.getElementById('chkVaryantKullan');
        var panel = document.getElementById('pnlVaryantlar');
        
        if (checkbox.checked) {
            panel.style.display = 'block';
            if (varyantTurleri.length === 0) {
                yukleVaryantTurleri();
            }
        } else {
            panel.style.display = 'none';
            // Varyant verilerini temizle
            temizleVaryantVerileri();
        }
    }

    // Varyant türlerini yükle
    function yukleVaryantTurleri() {
        console.log('Varyant türleri yükleniyor...');
        
        $.ajax({
            type: "POST",
            url: "YeniUrun.aspx/GetVaryantTurleri",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                console.log('Varyant türleri yüklendi:', response);
                if (response.d && response.d.length > 0) {
                    varyantTurleri = response.d;
                    gosterVaryantTurleri();
                } else {
                    $('#ddlVaryantTurleri').html('<option value="">Varyant türü bulunamadı</option>');
                    showWarningMessage('Varyant Sistemi', 'Varyant türleri yükleniyor...<br/><br/>' +
                        '<strong>Bilgi:</strong><br/>' +
                        '• İlk defa varyant kullanıyorsanız, sistem otomatik olarak default varyant türlerini oluşturacak<br/>' +
                        '• Bu işlem birkaç saniye sürebilir<br/>' +
                        '• Sayfayı yenileyin veya biraz bekleyip tekrar deneyin<br/><br/>' +
                        '<strong>Default Varyant Türleri:</strong> Renk, Boyut, Malzeme, Ağırlık, Hacim');
                    
                    // 3 saniye sonra otomatik olarak tekrar dene
                    setTimeout(function() {
                        console.log('Varyant türleri tekrar yükleniyor...');
                        yukleVaryantTurleri();
                    }, 3000);
                }
            },
            error: function (xhr, status, error) {
                console.error('Varyant türleri yüklenirken hata:', error);
                console.error('Response:', xhr.responseText);
                showErrorMessage('Hata', 'Varyant türleri yüklenirken hata oluştu: ' + error);
            }
        });
    }

    // Varyant türlerini dropdown'a yükle
    function gosterVaryantTurleri() {
        console.log('gosterVaryantTurleri çağrıldı, varyantTurleri.length:', varyantTurleri.length);
        
        var dropdown = $('#ddlVaryantTurleri');
        dropdown.empty();
        dropdown.append('<option value="">Varyant türü seçin...</option>');
        
        for (var i = 0; i < varyantTurleri.length; i++) {
            var tur = varyantTurleri[i];
            console.log('Varyant türü ekleniyor:', tur.VaryantTurID, tur.TurAdi);
            dropdown.append('<option value="' + tur.VaryantTurID + '">' + tur.TurAdi + '</option>');
        }
        
        console.log('Dropdown options:', dropdown.find('option').length);
    }

    // Seçilen varyant türlerini takip etmek için
    var secilenVaryantTurleri = [];

    // Varyant türü ekle
    function varyantTuruEkle() {
        var dropdown = $('#ddlVaryantTurleri');
        var turID = parseInt(dropdown.val());
        var turAdi = dropdown.find('option:selected').text();
        
        if (!turID || turID <= 0) {
            showWarningMessage('Uyarı', 'Lütfen bir varyant türü seçin.');
            return;
        }
        
        // Daha önce eklenmiş mi kontrol et
        if (secilenVaryantTurleri.some(function(t) { return t.VaryantTurID === turID; })) {
            showWarningMessage('Uyarı', 'Bu varyant türü zaten eklenmiş.');
            return;
        }
        
        // Seçilen listeye ekle
        secilenVaryantTurleri.push({ VaryantTurID: turID, TurAdi: turAdi });
        
        // Varyant değerlerini yükle
        yukleVaryantDegerleri(turID, turAdi);
        
        // Seçilen türleri göster
        gosterSecilenVaryantTurleri();
        
        // Dropdown'ı sıfırla
        dropdown.val('');
    }

    // Seçilen varyant türlerini göster - Advanced Form Components Tarzı
    function gosterSecilenVaryantTurleri() {
        var html = '';
        if (secilenVaryantTurleri.length > 0) {
            html += '<div class="alert alert-success" style="margin-bottom: 10px;">';
            html += '<i class="fa fa-check-circle"></i> <strong>Seçilen Varyant Türleri:</strong>';
            html += '</div>';
            
            for (var i = 0; i < secilenVaryantTurleri.length; i++) {
                var tur = secilenVaryantTurleri[i];
                html += '<div class="varyant-tur-item alert alert-info" style="position: relative; margin-bottom: 8px; padding-right: 35px;">';
                html += '<button type="button" class="close" onclick="varyantTuruKaldir(' + tur.VaryantTurID + ')" style="position: absolute; right: 10px; top: 8px;">';
                html += '<span aria-hidden="true">&times;</span>';
                html += '</button>';
                html += '<i class="fa fa-tag"></i> <strong>' + tur.TurAdi + '</strong>';
                html += '<span class="help-block" style="margin: 0; font-size: 11px;">Bu varyant türü için değerler aşağıda seçilebilir</span>';
                html += '</div>';
            }
        }
        $('#secilenVaryantTurleri').html(html);
    }

    // Varyant türünü kaldır
    function varyantTuruKaldir(turID) {
        // Seçilen listeden kaldır
        secilenVaryantTurleri = secilenVaryantTurleri.filter(function(t) { return t.VaryantTurID !== turID; });
        
        // Multi-select'i kaldır
        kaldirVaryantDegerleri(turID);
        
        // Seçilen türleri güncelle
        gosterSecilenVaryantTurleri();
    }

    // Varyant değerlerini yükle
    function yukleVaryantDegerleri(turID, turAdi) {
        console.log('Varyant değerleri yükleniyor, TurID:', turID);
        
        $.ajax({
            type: "POST",
            url: "YeniUrun.aspx/GetVaryantDegerleri",
            data: JSON.stringify({ varyantTurID: turID }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                console.log('Varyant değerleri yüklendi:', response);
                if (response.d && response.d.length > 0) {
                    varyantDegerleri[turID] = response.d;
                    ekleVaryantDegerleriMultiSelect(turID, turAdi, response.d);
                } else {
                    showWarningMessage('Uyarı', turAdi + ' için varyant değeri bulunamadı.');
                }
            },
            error: function (xhr, status, error) {
                console.error('Varyant değerleri yüklenirken hata:', error);
                console.error('Response:', xhr.responseText);
                showErrorMessage('Hata', 'Varyant değerleri yüklenirken hata oluştu: ' + error);
            }
        });
    }

    // Varyant değerleri için multi-select ekle - Advanced Form Components Tarzı
    function ekleVaryantDegerleriMultiSelect(turID, turAdi, degerler) {
        var html = '<div class="form-group varyant-deger-container" id="varyantDeger_' + turID + '">';
        html += '<label class="control-label col-md-3"><i class="fa fa-tags"></i> ' + turAdi + '</label>';
        html += '<div class="col-md-9">';
        html += '<select multiple="multiple" class="multi-select" id="multiSelect_' + turID + '" name="varyant_' + turID + '[]" style="display: none;">';
        
        for (var i = 0; i < degerler.length; i++) {
            html += '<option value="' + degerler[i].VaryantDegerID + '">' + degerler[i].DegerAdi + '</option>';
        }
        
        html += '</select>';
        html += '<span class="help-block">Çoklu seçim yapabilirsiniz. Sol taraftaki değerlere tıklayarak sağ tarafa taşıyın.</span>';
        html += '</div>';
        html += '</div>';
        
        // İlk varyant türü ekleniyorsa yardım mesajını gizle
        if ($('#varyantDegerleriContainer .form-group').length === 0) {
            $('#varyantDegerleriContainer .alert-info').hide();
        }
        
        $('#varyantDegerleriContainer').append(html);
        
        // Doğrudan manuel multi-select oluştur
        setTimeout(function() {
            console.log('Manuel multi-select oluşturuluyor, elementId:', 'multiSelect_' + turID);
            createAdvancedMultiSelect('multiSelect_' + turID, degerler);
        }, 100);
    }

    // Gelişmiş manuel multi-select oluştur
    function createAdvancedMultiSelect(elementId, degerler) {
        try {
            console.log('Gelişmiş multi-select oluşturuluyor:', elementId);
            
            var $select = $('#' + elementId);
            if ($select.length === 0) {
                console.error('Select element bulunamadı:', elementId);
                return;
            }
            
            // Multi-select container oluştur
            var containerHtml = '<div class="advanced-multiselect-container" id="container_' + elementId + '">';
            
            // Sol taraf - Seçilebilir değerler
            containerHtml += '<div class="multiselect-panel">';
            containerHtml += '<div class="panel-header">';
            containerHtml += '<input type="text" class="form-control search-input" placeholder="Seçilebilir değerlerde ara..." id="search_available_' + elementId + '">';
            containerHtml += '</div>';
            containerHtml += '<div class="panel-body">';
            containerHtml += '<div class="panel-title">Seçilebilir Değerler</div>';
            containerHtml += '<ul class="multiselect-list available-list" id="available_' + elementId + '">';
            
            // Tüm değerleri sol tarafa ekle
            for (var i = 0; i < degerler.length; i++) {
                var deger = degerler[i];
                containerHtml += '<li class="multiselect-item" data-value="' + deger.VaryantDegerID + '">';
                containerHtml += '<span>' + deger.DegerAdi + '</span>';
                containerHtml += '</li>';
            }
            
            containerHtml += '</ul>';
            containerHtml += '</div>';
            containerHtml += '</div>';
            
            // Sağ taraf - Seçili değerler
            containerHtml += '<div class="multiselect-panel">';
            containerHtml += '<div class="panel-header">';
            containerHtml += '<input type="text" class="form-control search-input" placeholder="Seçili değerlerde ara..." id="search_selected_' + elementId + '">';
            containerHtml += '</div>';
            containerHtml += '<div class="panel-body">';
            containerHtml += '<div class="panel-title">Seçili Değerler</div>';
            containerHtml += '<ul class="multiselect-list selected-list" id="selected_' + elementId + '">';
            containerHtml += '</ul>';
            containerHtml += '</div>';
            containerHtml += '</div>';
            
            containerHtml += '</div>';
            
            // Container'ı ekle
            $select.after(containerHtml);
            
            // Event handler'ları ekle
            setupMultiSelectEvents(elementId);
            
            console.log('Gelişmiş multi-select oluşturuldu:', elementId);
            
        } catch (e) {
            console.error('Gelişmiş multi-select oluşturma hatası:', e);
        }
    }

    // Multi-select event handler'larını kur
    function setupMultiSelectEvents(elementId) {
        var $available = $('#available_' + elementId);
        var $selected = $('#selected_' + elementId);
        var $select = $('#' + elementId);
        
        // Seçilebilir değerlere tıklama
        $available.on('click', '.multiselect-item', function() {
            var $item = $(this);
            var value = $item.data('value');
            var text = $item.find('span').text();
            
            // Seçili tarafa taşı
            $item.remove();
            $selected.append($item);
            
            // Select element'ini güncelle
            $select.find('option[value="' + value + '"]').prop('selected', true);
            
            console.log('Seçildi:', text);
        });
        
        // Seçili değerlere tıklama
        $selected.on('click', '.multiselect-item', function() {
            var $item = $(this);
            var value = $item.data('value');
            var text = $item.find('span').text();
            
            // Seçilebilir tarafa taşı
            $item.remove();
            $available.append($item);
            
            // Select element'ini güncelle
            $select.find('option[value="' + value + '"]').prop('selected', false);
            
            console.log('Seçim kaldırıldı:', text);
        });
        
        // Arama fonksiyonları
        $('#search_available_' + elementId).on('keyup', function() {
            var searchText = $(this).val().toLowerCase();
            $available.find('.multiselect-item').each(function() {
                var itemText = $(this).find('span').text().toLowerCase();
                if (itemText.indexOf(searchText) > -1) {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            });
        });
        
        $('#search_selected_' + elementId).on('keyup', function() {
            var searchText = $(this).val().toLowerCase();
            $selected.find('.multiselect-item').each(function() {
                var itemText = $(this).find('span').text().toLowerCase();
                if (itemText.indexOf(searchText) > -1) {
                    $(this).show();
                } else {
                    $(this).hide();
                }
            });
        });
    }

    // Varyant değerlerini kaldır
    function kaldirVaryantDegerleri(turID) {
        var elementId = 'multiSelect_' + turID;
        if (multiSelectInstances[elementId]) {
            try {
                $('#' + elementId).multiSelect('destroy');
            } catch (e) {
                console.log('MultiSelect destroy hatası (normal):', e);
            }
            delete multiSelectInstances[elementId];
        }
        $('#varyantDeger_' + turID).remove();
        delete varyantDegerleri[turID];
    }

    // Varyant kombinasyonlarını oluştur
    function olusturVaryantKombinasyonlari() {
        console.log('Varyant kombinasyonları oluşturuluyor...');
        
        // Seçilen varyant değerlerini topla
        var secilenDegerler = {};
        var secilenTurSayisi = 0;
        
        for (var i = 0; i < secilenVaryantTurleri.length; i++) {
            var turID = secilenVaryantTurleri[i].VaryantTurID;
            var secilenler = $('#multiSelect_' + turID).val();
            if (secilenler && secilenler.length > 0) {
                secilenDegerler[turID] = [];
                for (var j = 0; j < secilenler.length; j++) {
                    var degerID = parseInt(secilenler[j]);
                    var deger = varyantDegerleri[turID].find(function(d) { return d.VaryantDegerID === degerID; });
                    if (deger) {
                        secilenDegerler[turID].push(deger);
                    }
                }
                secilenTurSayisi++;
            }
        }
        
        if (secilenTurSayisi === 0) {
            showWarningMessage('Uyarı', 'Lütfen en az bir varyant türü için değer seçin.');
            return;
        }
        
        // Kombinasyonları oluştur
        varyantKombinasyonlari = olusturKombinasyonlar(secilenDegerler);
        console.log('Oluşturulan kombinasyonlar:', varyantKombinasyonlari);
        
        // Tabloyu güncelle
        gosterVaryantKombinasyonlari();
        
        showSuccessMessage('Başarılı', varyantKombinasyonlari.length + ' adet varyant kombinasyonu oluşturuldu.');
    }

    // Kombinasyonları oluştur (recursive)
    function olusturKombinasyonlar(secilenDegerler) {
        var turKeys = Object.keys(secilenDegerler);
        var kombinasyonlar = [];
        
        function recursive(index, mevcutKombinasyon) {
            if (index === turKeys.length) {
                kombinasyonlar.push(mevcutKombinasyon.slice());
                return;
            }
            
            var turID = turKeys[index];
            var degerler = secilenDegerler[turID];
            
            for (var i = 0; i < degerler.length; i++) {
                mevcutKombinasyon.push(degerler[i]);
                recursive(index + 1, mevcutKombinasyon);
                mevcutKombinasyon.pop();
            }
        }
        
        recursive(0, []);
        return kombinasyonlar;
    }

    // Varyant kombinasyonlarını tabloda göster
    function gosterVaryantKombinasyonlari() {
        var tbody = $('#tblVaryantKombinasyonlari tbody');
        tbody.empty();
        
        for (var i = 0; i < varyantKombinasyonlari.length; i++) {
            var kombinasyon = varyantKombinasyonlari[i];
            var varyantAdi = kombinasyon.map(function(d) { return d.DegerAdi; }).join(' / ');
            
            var row = '<tr data-index="' + i + '">';
            row += '<td>' + varyantAdi + '</td>';
            row += '<td><input type="text" class="form-control input-sm" placeholder="Barkod" /></td>';
            row += '<td><input type="number" class="form-control input-sm" placeholder="0" min="0" /></td>';
            row += '<td><input type="number" class="form-control input-sm" placeholder="0.00" min="0" step="0.01" /></td>';
            row += '<td><input type="number" class="form-control input-sm" placeholder="0.00" min="0" step="0.01" /></td>';
            row += '<td><input type="number" class="form-control input-sm" placeholder="0.00" min="0" step="0.01" /></td>';
            row += '<td><input type="checkbox" checked /></td>';
            row += '<td><button type="button" class="btn btn-sm btn-info" onclick="gorselYukle(' + i + ')"><i class="fa fa-image"></i></button></td>';
            row += '<td><button type="button" class="btn btn-sm btn-danger" onclick="kombinasyonSil(' + i + ')"><i class="fa fa-trash"></i></button></td>';
            row += '</tr>';
            
            tbody.append(row);
        }
    }

    // Kombinasyon sil
    function kombinasyonSil(index) {
        if (confirm('Bu varyant kombinasyonunu silmek istediğinizden emin misiniz?')) {
            varyantKombinasyonlari.splice(index, 1);
            gosterVaryantKombinasyonlari();
        }
    }

    // Görsel yükle
    function gorselYukle(index) {
        var kombinasyon = varyantKombinasyonlari[index];
        var varyantAdi = kombinasyon.map(function(d) { return d.DegerAdi; }).join(' / ');
        
        showInfoMessage('Görsel Yükleme', varyantAdi + ' için görsel yükleme özelliği yakında eklenecek.');
    }

    // Varyantları kaydet
    function kaydetVaryantlar() {
        if (varyantKombinasyonlari.length === 0) {
            showWarningMessage('Uyarı', 'Kaydedilecek varyant kombinasyonu bulunamadı.');
            return;
        }
        
        // Tablo verilerini topla
        var varyantVerileri = [];
        $('#tblVaryantKombinasyonlari tbody tr').each(function(index) {
            var $row = $(this);
            var kombinasyon = varyantKombinasyonlari[index];
            
            var varyantData = {
                VaryantDegerleri: kombinasyon.map(function(d) { return d.VaryantDegerID; }),
                Barkod: $row.find('input[type="text"]').val(),
                StokMiktari: parseInt($row.find('input[type="number"]').eq(0).val()) || 0,
                AlisFiyati: parseFloat($row.find('input[type="number"]').eq(1).val()) || 0,
                SatisFiyati: parseFloat($row.find('input[type="number"]').eq(2).val()) || 0,
                PerakendeSatisFiyati: parseFloat($row.find('input[type="number"]').eq(3).val()) || 0,
                Durum: $row.find('input[type="checkbox"]').is(':checked')
            };
            
            varyantVerileri.push(varyantData);
        });
        
        console.log('Kaydedilecek varyant verileri:', varyantVerileri);
        
        // AJAX ile kaydet
        $.ajax({
            type: "POST",
            url: "YeniUrun.aspx/KaydetVaryantlar",
            data: JSON.stringify({ varyantlar: varyantVerileri }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function() {
                $('button[onclick="kaydetVaryantlar()"]').prop('disabled', true).html('<i class="fa fa-spinner fa-spin"></i> Kaydediliyor...');
            },
            success: function (response) {
                console.log('Varyantlar kaydedildi:', response);
                if (response.d && response.d.success) {
                    showSuccessMessage('Başarılı', 'Varyantlar başarıyla kaydedildi.');
                } else {
                    showErrorMessage('Hata', response.d.message || 'Varyantlar kaydedilirken hata oluştu.');
                }
            },
            error: function (xhr, status, error) {
                console.error('Varyant kaydetme hatası:', error);
                console.error('Response:', xhr.responseText);
                showErrorMessage('Hata', 'Varyantlar kaydedilirken hata oluştu: ' + error);
            },
            complete: function() {
                $('button[onclick="kaydetVaryantlar()"]').prop('disabled', false).html('<i class="fa fa-save"></i> Varyantları Kaydet');
            }
        });
    }

    // Varyant verilerini temizle
    function temizleVaryantVerileri() {
        // Multi-select'leri temizle
        for (var elementId in multiSelectInstances) {
            $('#' + elementId).multiSelect('destroy');
        }
        multiSelectInstances = {};
        
        // Containerları temizle
        $('#varyantDegerleriContainer').html('<div class="alert alert-info" style="margin: 10px 0;"><i class="fa fa-info-circle"></i> <strong>Varyant değerlerini seçmek için:</strong><ol style="margin: 5px 0 0 20px;"><li>Yukarıdan bir varyant türü seçin ve "Ekle" butonuna tıklayın</li><li>Eklenen varyant türü için değerleri seçin</li><li>Kombinasyonları oluşturun</li></ol></div>');
        $('#tblVaryantKombinasyonlari tbody').empty();
        $('#secilenVaryantTurleri').empty();
        
        // Verileri sıfırla
        varyantDegerleri = {};
        varyantKombinasyonlari = [];
        secilenVaryantTurleri = [];
        
        // Dropdown'ı sıfırla
        $('#ddlVaryantTurleri').val('');
    }

    // Public metodlar - Ana sayfadan çağrılabilir
    window.VaryantUserControl = {
        temizle: temizleVaryantVerileri,
        yukle: yukleVaryantTurleri,
        getVaryantlar: function() { return varyantKombinasyonlari; }
    };
</script>
