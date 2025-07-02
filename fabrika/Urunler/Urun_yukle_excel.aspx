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

    <script type="text/javascript">
        // Debug fonksiyonu
        function debugLog(mesaj) {
            if (console && console.log) {
                console.log('[Excel Yükleme Debug] ' + mesaj);
            }
        }

        // MessageHelper fonksiyonları - fallback olarak (eğer master page'de yüklenmemişse)
        function showSuccessMessage(title, message, sticky) {
            debugLog('showSuccessMessage çağrıldı: ' + title);
            
            // Önce master page'deki global fonksiyonu dene
            if (typeof window.showSuccessMessage !== 'undefined' && window.showSuccessMessage !== showSuccessMessage) {
                debugLog('Master page showSuccessMessage fonksiyonu kullanılıyor');
                window.showSuccessMessage(title, message, sticky);
                return;
            }
            
            // Sonra Gritter'ı doğrudan dene
            if (typeof $ !== 'undefined' && typeof $.gritter !== 'undefined') {
                debugLog('Doğrudan Gritter kullanılıyor');
                $.gritter.add({
                    title: '<i class="icon-ok-sign"></i> ' + title,
                    text: message,
                    sticky: sticky || false,
                    time: 6000,
                    class_name: 'gritter-success'
                });
            } else {
                debugLog('Fallback alert() kullanılıyor');
                // Fallback: basit alert
                alert('[BAŞARI] ' + title + ': ' + message);
            }
        }

        function showErrorMessage(title, message, sticky) {
            debugLog('showErrorMessage çağrıldı: ' + title);
            debugLog('jQuery durumu: ' + typeof $);
            debugLog('Gritter durumu: ' + (typeof $ !== 'undefined' ? typeof $.gritter : 'jQuery yok'));
            
            // Önce master page'deki fonksiyonu dene
            if (typeof $ !== 'undefined' && typeof $.gritter !== 'undefined') {
                debugLog('Gritter ile mesaj gönderiliyor...');
                try {
                    $.gritter.add({
                        title: '<i class="icon-warning-sign"></i> ' + title,
                        text: message,
                        sticky: sticky || false,
                        time: 8000,
                        class_name: 'gritter-error'
                    });
                    debugLog('Gritter mesajı başarıyla gönderildi');
                } catch (e) {
                    debugLog('Gritter hatası: ' + e.message);
                    alert('[HATA] ' + title + ': ' + message);
                }
            } else {
                debugLog('Fallback alert() kullanılıyor');
                // Fallback: basit alert
                alert('[HATA] ' + title + ': ' + message);
            }
        }

        function showWarningMessage(title, message, sticky) {
            // Önce master page'deki fonksiyonu dene
            if (typeof $ !== 'undefined' && typeof $.gritter !== 'undefined') {
                $.gritter.add({
                    title: '<i class="icon-exclamation-sign"></i> ' + title,
                    text: message,
                    sticky: sticky || false,
                    time: 7000,
                    class_name: 'gritter-warning'
                });
            } else {
                // Fallback: basit alert
                alert('[UYARI] ' + title + ': ' + message);
            }
        }

        function showInfoMessage(title, message, sticky) {
            // Önce master page'deki fonksiyonu dene
            if (typeof $ !== 'undefined' && typeof $.gritter !== 'undefined') {
                $.gritter.add({
                    title: '<i class="icon-info-sign"></i> ' + title,
                    text: message,
                    sticky: sticky || false,
                    time: 5000,
                    class_name: 'gritter-info'
                });
            } else {
                // Fallback: basit alert
                alert('[BİLGİ] ' + title + ': ' + message);
            }
        }

        // Kütüphane durumlarını kontrol et
        function checkLibraries() {
            debugLog('=== KÜTÜPHANE DURUM RAPORU ===');
            debugLog('jQuery: ' + (typeof $ !== 'undefined' ? 'MEVCUT (v' + ($.fn ? $.fn.jquery : '?') + ')' : 'YOK'));
            debugLog('jQuery.gritter: ' + (typeof $ !== 'undefined' && typeof $.gritter !== 'undefined' ? 'MEVCUT' : 'YOK'));
            
            // Gritter CSS kontrolü
            var gritterCSS = document.querySelector('link[href*="gritter"]');
            debugLog('Gritter CSS: ' + (gritterCSS ? 'MEVCUT (' + gritterCSS.href + ')' : 'YOK'));
            
            // Gritter JS kontrolü
            var gritterJS = document.querySelector('script[src*="gritter"]');
            debugLog('Gritter JS: ' + (gritterJS ? 'MEVCUT (' + gritterJS.src + ')' : 'YOK'));
            
            // Master page fonksiyonları kontrolü
            debugLog('window.showSuccessMessage: ' + typeof window.showSuccessMessage);
            debugLog('window.showErrorMessage: ' + typeof window.showErrorMessage);
            debugLog('=== RAPOR SONU ===');
        }

        // jQuery yüklenmesini bekle ve sonra çalıştır
        function initializeExcelDebug() {
            debugLog('Sayfa yüklendi');
            
            // Kütüphane durumunu kontrol et
            checkLibraries();
            
            // jQuery varsa kullan, yoksa vanilla JavaScript kullan
            if (typeof $ !== 'undefined') {
                debugLog('jQuery mevcut, jQuery kullanılıyor');
                
                // jQuery ile event listener'lar
                $('#<%= btnOnizle.ClientID %>').click(function() {
                    debugLog('Önizle butonuna tıklandı');
                });
                
                $('#<%= btnOnlaKaydet.ClientID %>').click(function() {
                    debugLog('Onayla Kaydet butonuna tıklandı');
                });
                
                $('#<%= fuExcel.ClientID %>').change(function() {
                    var fileName = $(this).val();
                    debugLog('Dosya seçildi: ' + fileName);
                });
            } else {
                debugLog('jQuery mevcut değil, vanilla JavaScript kullanılıyor');
                
                // Vanilla JavaScript ile event listener'lar
                var btnOnizle = document.getElementById('<%= btnOnizle.ClientID %>');
                if (btnOnizle) {
                    btnOnizle.addEventListener('click', function() {
                        debugLog('Önizle butonuna tıklandı');
                    });
                }
                
                var btnKaydet = document.getElementById('<%= btnOnlaKaydet.ClientID %>');
                if (btnKaydet) {
                    btnKaydet.addEventListener('click', function() {
                        debugLog('Onayla Kaydet butonuna tıklandı');
                    });
                }
                
                var fuExcel = document.getElementById('<%= fuExcel.ClientID %>');
                if (fuExcel) {
                    fuExcel.addEventListener('change', function() {
                        var fileName = this.value;
                        debugLog('Dosya seçildi: ' + fileName);
                    });
                }
            }
            
            // MessageHelper fonksiyonlarını kontrol et
            if (typeof showErrorMessage !== 'undefined') {
                debugLog('MessageHelper fonksiyonları mevcut');
            } else {
                debugLog('UYARI: MessageHelper fonksiyonları mevcut değil');
            }
            
            // jQuery ve Gritter kontrolü
            if (typeof $ !== 'undefined') {
                debugLog('jQuery sürümü: ' + ($.fn ? $.fn.jquery : 'bilinmiyor'));
                if (typeof $.gritter !== 'undefined') {
                    debugLog('Gritter kütüphanesi mevcut');
                    
                    // Test başarılı - Gritter çalışıyor
                } else {
                    debugLog('UYARI: Gritter kütüphanesi mevcut değil - alert() kullanılacak');
                    debugLog('jQuery.gritter objesi: ' + typeof $.gritter);
                }
            } else {
                debugLog('UYARI: jQuery mevcut değil');
            }
        }

        // Hem DOMContentLoaded hem de window.onload'da dene
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', initializeExcelDebug);
        } else {
            initializeExcelDebug();
        }

        // jQuery yüklendikten sonra da dene
        if (typeof $ !== 'undefined') {
            $(document).ready(initializeExcelDebug);
        } else {
            // jQuery henüz yüklenmemişse, biraz bekle ve tekrar dene
            setTimeout(function() {
                if (typeof $ !== 'undefined') {
                    $(document).ready(initializeExcelDebug);
                }
            }, 500);
        }

        // Gritter'ın yüklenmesini bekle (gecikmeli kontrol)
        function waitForGritter(attempts) {
            attempts = attempts || 0;
            if (attempts > 10) {
                debugLog('Gritter 5 saniye içinde yüklenemedi, vazgeçiliyor...');
                return;
            }

            if (typeof $ !== 'undefined' && typeof $.gritter !== 'undefined') {
                debugLog('Gritter hazır! Deneme sayısı: ' + attempts);
                checkLibraries();
            } else {
                debugLog('Gritter henüz hazır değil, bekleniyor... (Deneme ' + (attempts + 1) + '/10)');
                setTimeout(function() {
                    waitForGritter(attempts + 1);
                }, 500);
            }
        }

        // Gritter kontrolünü başlat
        setTimeout(function() {
            waitForGritter();
        }, 1000);
    </script>
</asp:Content>

