<%@ Page Title="Müşteri Satış" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="MusteriSatis.aspx.cs" Inherits="fabrika_Musteriler_MusteriSatis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- AJAX ScriptManager -->
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    
    <!-- jQuery ve jQuery UI -->
    <script src="https://code.jquery.com/jquery-1.12.4.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    
    <!-- SweetAlert2 for beautiful alerts -->
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    
    <script>
        // jQuery noConflict modunu kullanarak çakışmaları önle
        var jq = jQuery.noConflict();
        
        // SweetAlert2 ile özelleştirilmiş mesaj fonksiyonları
        function showSuccessMessage(title, message) {
            Swal.fire({
                icon: 'success',
                title: title,
                text: message,
                confirmButtonText: 'Tamam',
                confirmButtonColor: '#3085d6'
            });
        }

        function showErrorMessage(title, message) {
            Swal.fire({
                icon: 'error',
                title: title,
                text: message,
                confirmButtonText: 'Tamam',
                confirmButtonColor: '#d33'
            });
        }

        function showWarningMessage(title, message) {
            Swal.fire({
                icon: 'warning',
                title: title,
                text: message,
                confirmButtonText: 'Tamam',
                confirmButtonColor: '#f0ad4e'
            });
        }

        function showInfoMessage(title, message) {
            Swal.fire({
                icon: 'info',
                title: title,
                text: message,
                confirmButtonText: 'Tamam',
                confirmButtonColor: '#17a2b8'
            });
        }
    </script>
    
    <style type="text/css">
        .urun-oneri-item {
            padding: 8px 10px;
            cursor: pointer;
            border-bottom: 1px solid #f0f0f0;
        }
        .urun-oneri-item:hover, .urun-oneri-item.active {
            background-color: #f8f8f8;
        }
        .urun-oneri-item small {
            color: #999;
            margin-left: 5px;
        }
        #urunAramaOneriContainer {
            position: relative;
        }
        #urunAramaOneriContainer .list-group {
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            max-height: 300px;
            overflow-y: auto;
        }
    </style>

    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <h3>Müşteri Satış İşlemi</h3>
                </header>
                <!-- İşlem Butonları -->
                <div class="panel-body">
                    <div class="btn-group">
                        <asp:LinkButton ID="btnProformaSiparis" runat="server" CssClass="btn btn-shadow btn-success" Style="margin-right: 5px;">
                    <i class="icon-file-text"></i> Proforma/Sipariş Kaydet (Stok Hariketi olmaz)
                        </asp:LinkButton>
                                                <asp:LinkButton ID="btnIrsaliyeKaydet" runat="server" CssClass="btn btn-shadow btn-info" Style="margin-right: 5px;">                    <i class="icon-truck"></i> İrsaliye Kaydet (Stok Hariketi olmaz)                        </asp:LinkButton>
                        <asp:LinkButton ID="btnFaturaKaydet" runat="server" CssClass="btn btn-shadow btn-danger" Style="margin-right: 5px;" OnClick="btnFaturaKaydet_Click">
                    <i class="icon-file"></i> Fatura Kaydet (Stok Eksilir)
                        </asp:LinkButton>
                        <asp:HyperLink ID="hplbtnGeriDon" runat="server" CssClass="btn btn-shadow btn-warning"><i class="icon-arrow-left"></i>Geri Dön</asp:HyperLink>
                        
                            
                  
                    </div>
                </div>
            </section>
        </div>
    </div>

    <div class="row">
        <!-- Müşteri Bilgileri Bölümü -->
        <div class="col-md-4">
            <div class="panel panel-primary" style="border-color: #3a87ad;">
                <div class="panel-heading" style="background-color: #3a87ad;">
                    <h3 class="panel-title">
                        <asp:Label ID="lblMusteriAd" runat="server" Text="Label"></asp:Label></h3>
                    <span class="pull-right">
                        <asp:LinkButton ID="btnBilgiDuzenle" runat="server" CssClass="btn btn-xs btn-default">
                            <i class="icon-info-sign"></i>
                        </asp:LinkButton>
                    </span>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <!-- Belge No -->
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Belge No:</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtBelgeNo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Tarih -->
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Tarih:</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtTarih" runat="server" CssClass="form-control" placeholder="06.05.2025"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtSaat" runat="server" CssClass="form-control" placeholder="17:20"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Vadesi -->
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Vadesi:</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtVadesi" runat="server" CssClass="form-control" placeholder="06.05.2025"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Sipariş Tarih/No -->
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Sipariş Tarih/No:</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtSiparisTarih" runat="server" CssClass="form-control" placeholder="Sipariş tarihi"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtSiparisNo" runat="server" CssClass="form-control" placeholder="Sipariş no"></asp:TextBox>
                            </div>
                        </div>

                        <!-- İrsaliye No -->
                        <div class="form-group">
                            <label class="col-sm-4 control-label">İrsaliye No:</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtIrsaliyeNo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Sevk Tarihi -->
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Sevk Tarihi:</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtSevkTarihi" runat="server" CssClass="form-control" placeholder="06.05.2025"></asp:TextBox>
                            </div>
                        </div>
                        <!-- Proje -->
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Pazarlamaci:</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlPazarlama" runat="server" CssClass="form-control" DataTextField="Ad" DataValueField="PazarlamaciID">
                                </asp:DropDownList>
                                <small class="text-muted">(irsaliye için)</small>
                            </div>
                        </div>

                        <!-- Proje -->
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Proje:</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlProje" runat="server" CssClass="form-control" DataTextField="Ad" DataValueField="ProjeID">
                                </asp:DropDownList>
                                <small class="text-muted">(irsaliye için)</small>
                            </div>
                        </div>

                        <!-- Açıklama -->
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Açıklama:</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Ürün/Hizmetler Bölümü -->
        <div class="col-md-8">
            <div class="panel panel-success" style="border-color: #5cb85c;">
                <div class="panel-heading" style="background-color: #5cb85c;">
                    <h3 class="panel-title">ÜRÜN / HİZMETLER</h3>
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <div class="input-group">
                            <asp:TextBox ID="txtUrunAra" runat="server" CssClass="form-control" placeholder="Ürün isminden arayın veya barkod okutun"></asp:TextBox>
                            <span class="input-group-btn">
                                <asp:LinkButton ID="btnUrunAra" runat="server" CssClass="btn btn-primary" OnClientClick="araUrunler(); return false;" style="display: none;">
                                    <i class="icon-search"></i> Detaylı arama için tıklayın
                                </asp:LinkButton>
                            </span>
                        </div>
                        <div id="urunAramaOneriContainer" style="width:100%;"></div>
                    </div>

                    <div class="alert alert-info" id="urunYokUyari">
                        <p>Henüz ürün eklenmedi. Ürün eklemek için yukarıdaki arama kutusunu kullanın.</p>
                    </div>

                    <!-- İşlem mesajları -->
                    <div class="alert alert-info" id="urunYukleniyorMessage" style="display:none;">
                        <i class="icon-spinner icon-spin"></i> Ürün ekleniyor, lütfen bekleyin...
                    </div>

                    <!-- Ürün Tablosu -->
                    <div class="table-responsive" id="urunTabloContainer">
                        <asp:GridView ID="gvUrunler" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-hover table-bordered" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="UrunKodu" HeaderText="Ürün Kodu" />
                                <asp:BoundField DataField="UrunAdi" HeaderText="Ürün Adı" />
                                <asp:BoundField DataField="Miktar" HeaderText="Miktar" />
                                <asp:BoundField DataField="Birim" HeaderText="Birim" />
                                <asp:BoundField DataField="BirimFiyat" HeaderText="Birim Fiyat" />
                                <asp:BoundField DataField="Iskonto" HeaderText="İskonto" />
                                <asp:BoundField DataField="KDV" HeaderText="KDV" />
                                <asp:BoundField DataField="Tutar" HeaderText="Tutar" />
                                <asp:TemplateField HeaderText="İşlemler">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnDuzenle" runat="server" CssClass="btn btn-xs btn-primary">
                                            <i class="icon-pencil"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnSil" runat="server" CssClass="btn btn-xs btn-danger">
                                            <i class="icon-trash"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="text-center">Henüz ürün eklenmedi</div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Ürün Arama Modal Dialog -->
    <div class="modal fade" id="urunAramaModal" tabindex="-1" role="dialog" aria-labelledby="urunAramaModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title" id="urunAramaModalLabel">Ürün Arama</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="input-group">
                                <input type="text" id="txtModalUrunAra" class="form-control" placeholder="Ürün Kodu, Barkod veya Ürün Adı ile arama yapın">
                                <span class="input-group-btn">
                                    <button class="btn btn-primary" type="button" id="btnModalUrunAra">
                                        <i class="icon-search"></i> Ara
                                    </button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="row mt-3">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <table id="tblUrunSonuc" class="table table-striped table-bordered">
                                    <thead>
                                        <tr>
                                            <th>Ürün Kodu</th>
                                            <th>Stoklu</th>
                                            <th>Barkod</th>
                                            <th>Ürün Adı</th>
                                            <th>Birim Fiyat</th>
                                            <th>İşlem</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <!-- AJAX ile doldurulacak -->
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Kapat</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Ürün Detay Modal Dialog -->
    <div class="modal fade" id="urunDetayModal" tabindex="-1" role="dialog" aria-labelledby="urunDetayModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header" style="background-color: #00abc0; color: white;">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true" style="color: white;">&times;</button>
                    <h4 class="modal-title" id="urunDetayModalLabel">Ürün Satış Detayları</h4>
                </div>
                <div class="modal-body">
                    <div class="form-horizontal">
                        <input type="hidden" id="hdnUrunID" value="0" />
                        
                        <!-- Sadece stok miktarını göster, diğer alanları kaldır -->
                        <div class="form-group" style="display: none;">
                            <label class="col-sm-3 control-label">Stok Miktarı:</label>
                            <div class="col-sm-9">
                                <p class="form-control-static"><span id="lblModalStokMiktari"></span> <span id="lblModalBirimAdi"></span></p>
                            </div>
                        </div>
                        
                        <div class="row" style="margin-top: 20px;">
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">Miktar</label>
                                    <div class="col-sm-8">
                                        <asp:TextBox ID="txtModalMiktar" CssClass="form-control text-right" runat="server" Text="1"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">Depo</label>
                                    <div class="col-sm-8">
                                        <select id="ddlModalDepo" class="form-control">
                                            <option value="Ana Depo">Ana Depo (Stokta yok)</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <div class="row" style="margin-top: 20px;">
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">Fiyat</label>
                                    <div class="col-sm-8">
                                        <div class="input-group">
                                            <input type="text" id="txtModalFiyat" class="form-control text-right" />
                                            <div class="input-group-btn">
                                                <button type="button" class="btn btn-default dropdown-toggle" style="min-width: 60px;" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                                    TL <span class="caret"></span>
                                                </button>
                                                <ul class="dropdown-menu dropdown-menu-right">
                                                    <li><a href="#" onclick="return false;">önceki fiyatlar</a></li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">KDV(%)</label>
                                    <div class="col-sm-8">
                                        <select id="ddlModalKDV" class="form-control">
                                            <option value="1">1</option>
                                            <option value="8">8</option>
                                            <option value="18" selected>18</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <div class="row" style="margin-top: 20px;">
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">İndirim</label>
                                    <div class="col-sm-8">
                                        <div class="input-group">
                                            <input type="text" id="txtModalIndirim" class="form-control text-right" value="0" />
                                            <div class="input-group-btn">
                                                <select id="ddlModalIndirimTuru" class="form-control" style="width: 60px; border-left: none;">
                                                    <option value="%">%</option>
                                                    <option value="TL">TL</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group" style="margin-top: 10px;">
                                    <div class="col-sm-8 col-sm-offset-4">
                                        <a href="#" class="text-success" onclick="return false;" id="btnCokluIskonto">çoklu iskonto</a>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <div class="form-group">
                                    <label class="col-sm-4 control-label">TOPLAM</label>
                                    <div class="col-sm-8">
                                        <div class="input-group">
                                            <input type="text" id="lblModalToplamInput" class="form-control text-right" value="0.00" />
                                            <span class="input-group-addon" style="min-width: 60px;">TL</span>
                                        </div>
                                        <small class="text-muted text-right" style="display: block;" id="lblModalNetKdv">(0.00 + 0.00)</small>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <div class="form-group" style="margin-top: 20px;">
                            <label class="col-sm-2 control-label">Açıklama</label>
                            <div class="col-sm-10">
                                <textarea id="txtModalAciklama" class="form-control" rows="3" placeholder="isteğe bağlı açıklama girebilirsiniz"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">İptal</button>
                    <button type="button" class="btn btn-success" id="btnUrunEkle" onclick="urunEkle()"><i class="icon-plus"></i> Ekle</button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        // Test fonksiyonları
        function testJQ() {
            try {
                var version = jq.fn.jquery;
                alert("jQuery sürümü: " + version + " başarıyla yüklendi!");
            } catch (e) {
                alert("jQuery ÇALIŞMIYOR! Hata: " + e.message);
            }
        }
        
        // Depoları yükle
        function loadDepolar() {
            try {
                console.log("loadDepolar fonksiyonu çağrıldı");
                console.log("window.depoListesi:", window.depoListesi);
                console.log("window.depolarYuklendi:", window.depolarYuklendi);
                
                if (typeof window.depoListesi !== 'undefined' && window.depoListesi && window.depoListesi.length > 0) {
                    console.log("Depolar yükleniyor:", window.depoListesi);
                    
                    // Modal depo dropdown'ını temizle
                    jq("#ddlModalDepo").empty();
                    
                    // Depoları dropdown'a ekle
                    jq.each(window.depoListesi, function(index, depo) {
                        var stokBilgisi = "";
                        if (depo.Kapasite && depo.Kapasite > 0) {
                            var dolulukOrani = depo.DoluMiktar ? ((depo.DoluMiktar / depo.Kapasite) * 100).toFixed(1) : 0;
                            stokBilgisi = " - %" + dolulukOrani + " dolu";
                        }
                        
                        var depoMetni = depo.DepoAdi + " (" + depo.DepoKodu + ")" + stokBilgisi;
                        jq("#ddlModalDepo").append('<option value="' + depo.DepoID + '">' + depoMetni + '</option>');
                    });
                    console.log("Depolar başarıyla yüklendi:", window.depoListesi.length + " adet");
                } else {
                    console.warn("Depo listesi boş veya tanımsız");
                    jq("#ddlModalDepo").empty();
                    jq("#ddlModalDepo").append('<option value="1">Ana Depo (Varsayılan)</option>');
                    
                    // Depo listesi henüz yüklenmemişse, kısa süre sonra tekrar dene
                    if (typeof window.depolarYuklendi === 'undefined' || !window.depolarYuklendi) {
                        console.log("Depolar henüz yüklenmemiş, 1 saniye sonra tekrar denenecek");
                        setTimeout(function() {
                            loadDepolar();
                        }, 1000);
                    }
                }
            } catch (e) {
                console.error("Depo yükleme hatası:", e);
                jq("#ddlModalDepo").empty();
                jq("#ddlModalDepo").append('<option value="1">Ana Depo (Hata)</option>');
            }
        }
        
        // Test butonu kaldırıldı - gerçek ürünlere odaklanıyoruz
        
        // Sayfa yüklendikten sonra çalışır
        document.addEventListener("DOMContentLoaded", function() {
            // jQuery kullanılabilir mi kontrol et
            if (typeof jQuery === 'undefined') {
                console.error("jQuery yüklenemedi! Bazı işlevler çalışmayabilir.");
                alert("jQuery kütüphanesi yüklenemedi! Lütfen sayfayı yenileyin veya yöneticinize başvurun.");
                return;
            }
            
            console.log("Document ready - MusteriSatis.aspx");
            
            // Arama kutusuna odaklan
            setTimeout(function() {
                try {
                    var searchField = document.getElementById('<%= txtUrunAra.ClientID %>');
                    if (searchField) {
                        searchField.focus();
                        console.log("Sayfa yüklendiğinde arama kutusuna odaklanıldı");
                    } else {
                        console.warn("Arama kutusu bulunamadı (txtUrunAra)");
                    }
                } catch (err) {
                    console.error("Arama kutusuna odaklanırken hata:", err);
                }
            }, 500);
            
            // Global değişkenler
            window.listNavigationEnabled = false;
            window.selectedNavigationIndex = -1;
            window.keyboardEventsAttached = false;
            
            // Enter tuşu için doğrudan belge seviyesinde yakalayıcı
            document.addEventListener('keydown', function(e) {
                if (e.which === 13 || e.keyCode === 13) {
                    console.log("Document level - ENTER tuşu basıldı");
                    
                    if (!jq("#urunAramaOneriContainer").is(":empty") && jq(".urun-oneri-item").length > 0) {
                        console.log("Dropdown açık ve ürün önerileri var");
                        
                        // Enter tuşu işleminin devam etmesini engelle
                        e.preventDefault();
                        e.stopPropagation();
                        
                        // Seçili öğe indeksi geçerli mi kontrol et
                        if (window.selectedNavigationIndex < 0) {
                            window.selectedNavigationIndex = 0; // İlk öğeyi seç
                            jq(".urun-oneri-item").first().addClass("active");
                        }
                        
                        try {
                            var items = jq(".urun-oneri-item");
                            var selectedItem = jq(items[window.selectedNavigationIndex]);
                            var urunId = selectedItem.data("urun-id");
                            
                            console.log("Ürün seçiliyor - ENTER - ID: " + urunId);
                            
                            // Görsel geri bildirim ekle
                            selectedItem.css("background-color", "#dff0d8");
                            setTimeout(function() {
                                // secUrun fonksiyonunu çağır
                                secUrun(urunId);
                                
                                // Dropdown'ı temizle
                                jq("#urunAramaOneriContainer").empty();
                            }, 100);
                            
                            return false;
                        } catch (err) {
                            console.error("Enter işlenirken hata:", err);
                        }
                    }
                }
            }, false);
            
            // Sayfa genelinde ok tuşu olaylarını yakala (tüm diğerlerinden önce)
            document.addEventListener('keydown', function(e) {
                // Sadece aşağı/yukarı tuşları ve açık liste varsa işle
                if ((e.keyCode === 38 || e.keyCode === 40) && !jq("#urunAramaOneriContainer").is(":empty")) {
                    console.log("Global ok tuşu yakalandı: " + e.keyCode);
                    
                    // Yukarı ok
                    if (e.keyCode === 38) {
                        handleArrowUp();
                        e.preventDefault();
                        return false;
                    }
                    // Aşağı ok
                    else if (e.keyCode === 40) {
                        handleArrowDown();
                        e.preventDefault();
                        return false;
                    }
                }
                // Enter tuşu için global izleme
                else if (e.keyCode === 13 && !jq("#urunAramaOneriContainer").is(":empty")) {
                    console.log("Global keydown - Enter tuşu yakalandı");
                    
                    var items = jq(".urun-oneri-item");
                    if (items.length > 0 && window.selectedNavigationIndex >= 0) {
                        try {
                            var selectedItem = jq(items[window.selectedNavigationIndex]);
                            var urunId = selectedItem.data("urun-id");
                            
                            console.log("Capture phase - ürün seçiliyor - ID: " + urunId);
                            selectedItem.addClass("active").css("background-color", "#dff0d8");
                            
                            // Modal'ı açmak için ürünü seç
                            secUrun(urunId);
                            
                            // Öneri listesini temizle
                            jq("#urunAramaOneriContainer").empty();
                        } catch (err) {
                            console.error("Global Enter capture yakalanırken hata:", err);
                        }
                    }
                }
            }, true); // true = capturing phase, en önce çalışır
            
            // Yukarı ok tuşunu işle
            function handleArrowUp() {
                var items = jq(".urun-oneri-item");
                if (items.length === 0) return;
                
                if (window.selectedNavigationIndex > 0) {
                    window.selectedNavigationIndex--;
                } else {
                    window.selectedNavigationIndex = items.length - 1;
                }
                
                console.log("Yukarı ok - yeni indeks: " + window.selectedNavigationIndex);
                
                // Tüm öğeleri temizle ve seçileni vurgula
                items.removeClass("active");
                jq(items[window.selectedNavigationIndex]).addClass("active");
                
                // Görünür kısma kaydır
                var container = jq("#urunAramaOneriContainer .list-group");
                var selectedItem = jq(items[window.selectedNavigationIndex]);
                
                if (container.length && selectedItem.length) {
                    var itemTop = selectedItem.position().top;
                    var containerScrollTop = container.scrollTop();
                    var containerHeight = container.height();
                    
                    if (itemTop < 0) {
                        container.scrollTop(containerScrollTop + itemTop);
                    } else if (itemTop + selectedItem.outerHeight() > containerHeight) {
                        container.scrollTop(containerScrollTop + itemTop - containerHeight + selectedItem.outerHeight());
                    }
                }
            }
            
            // Aşağı ok tuşunu işle
            function handleArrowDown() {
                var items = jq(".urun-oneri-item");
                if (items.length === 0) return;
                
                if (window.selectedNavigationIndex < items.length - 1) {
                    window.selectedNavigationIndex++;
                } else {
                    window.selectedNavigationIndex = 0;
                }
                
                console.log("Aşağı ok - yeni indeks: " + window.selectedNavigationIndex);
                
                // Tüm öğeleri temizle ve seçileni vurgula
                items.removeClass("active");
                jq(items[window.selectedNavigationIndex]).addClass("active");
                
                // Görünür kısma kaydır
                var container = jq("#urunAramaOneriContainer .list-group");
                var selectedItem = jq(items[window.selectedNavigationIndex]);
                
                if (container.length && selectedItem.length) {
                    var itemTop = selectedItem.position().top;
                    var containerScrollTop = container.scrollTop();
                    var containerHeight = container.height();
                    
                    if (itemTop < 0) {
                        container.scrollTop(containerScrollTop + itemTop);
                    } else if (itemTop + selectedItem.outerHeight() > containerHeight) {
                        container.scrollTop(containerScrollTop + itemTop - containerHeight + selectedItem.outerHeight());
                    }
                }
            }
            
            // Klavye navigasyonu için izleme
            jq(document).on("keydown", function(e) {
                // Konsola tuş kodunu yazdır (hata ayıklama için)
                console.log("Global keydown event: " + e.which);
            });
            
            // Depoları yükle - birkaç deneme ile
            var depoYuklemeDenemesi = 0;
            var maksimumDeneme = 5;
            
            function depoYuklemeKontrol() {
                depoYuklemeDenemesi++;
                console.log("Depo yükleme denemesi: " + depoYuklemeDenemesi + "/" + maksimumDeneme);
                
                if (typeof window.depoListesi !== 'undefined' && window.depoListesi && window.depoListesi.length > 0) {
                    console.log("Depolar başarıyla yüklendi, loadDepolar çağrılıyor");
                    loadDepolar();
                } else if (depoYuklemeDenemesi < maksimumDeneme) {
                    console.log("Depolar henüz yüklenmemiş, " + (depoYuklemeDenemesi * 500) + "ms sonra tekrar denenecek");
                    setTimeout(depoYuklemeKontrol, 500);
                } else {
                    console.warn("Depolar " + maksimumDeneme + " denemede yüklenemedi, varsayılan depo kullanılacak");
                    loadDepolar(); // Varsayılan depo ile devam et
                }
            }
            
            // İlk kontrol
            setTimeout(depoYuklemeKontrol, 500);
            
            // Debug mesajı - otomatik olarak gösterilecek
            setTimeout(function() {
                jq("#urunAramaOneriContainer").html('<div class="alert alert-info">Arama kutusu hazır. En az 3 karakter girin.</div>');
                setTimeout(function() {
                    jq("#urunAramaOneriContainer").empty();
                }, 3000);
            }, 1000);
            
            // Tarih alanları için datepicker
            try {
                jq("#<%=txtTarih.ClientID %>").datepicker({
                    dateFormat: 'dd.mm.yy',
                    changeMonth: true,
                    changeYear: true,
                    showButtonPanel: true,
                    dayNamesMin: ["Pz", "Pt", "Sa", "Ça", "Pe", "Cu", "Ct"],
                    monthNamesShort: ["Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"]
                });

                jq("#<%=txtVadesi.ClientID %>").datepicker({
                    dateFormat: 'dd.mm.yy',
                    changeMonth: true,
                    changeYear: true,
                    showButtonPanel: true,
                    dayNamesMin: ["Pz", "Pt", "Sa", "Ça", "Pe", "Cu", "Ct"],
                    monthNamesShort: ["Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"]
                });

                jq("#<%=txtSevkTarihi.ClientID %>").datepicker({
                    dateFormat: 'dd.mm.yy',
                    changeMonth: true,
                    changeYear: true,
                    showButtonPanel: true,
                    dayNamesMin: ["Pz", "Pt", "Sa", "Ça", "Pe", "Cu", "Ct"],
                    monthNamesShort: ["Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"]
                });

                jq("#<%=txtSiparisTarih.ClientID %>").datepicker({
                    dateFormat: 'dd.mm.yy',
                    changeMonth: true,
                    changeYear: true,
                    showButtonPanel: true,
                    dayNamesMin: ["Pz", "Pt", "Sa", "Ça", "Pe", "Cu", "Ct"],
                    monthNamesShort: ["Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık"]
                });
            } catch (e) {
                console.error("Datepicker hatası:", e);
            }
            
            // Otomatik arama
            jq("#<%=txtUrunAra.ClientID %>").keyup(function(e) {
                // Ok tuşlarını ve Enter, Escape tuşlarını işlemden hariç tut
                if (e.which == 38 || e.which == 40 || e.which == 13 || e.which == 27) {
                    e.preventDefault();
                    e.stopPropagation();
                    return false; // Aramayı tetikleme
                }
                
                var val = jq(this).val().trim();
                console.log("Arama kutusuna girilen: ", val);
                
                if (val.length >= 3) {
                    // Gerçek AJAX araması yap
                    setTimeout(function() {
                        console.log("AJAX aramaya hazırlanıyor...");
                        araUrunlerDropdown(val);
                    }, 300);
                } else {
                    jq("#urunAramaOneriContainer").empty();
                }
            });
            
            // Arama kutusu için ayrı bir keydown olayı ekle, ok tuşlarının kontrolünü buraya taşı
            jq("#<%=txtUrunAra.ClientID %>").keydown(function(e) {
                // Konsola tuş kodunu yazdır (hata ayıklama için)
                console.log("Arama kutusu keydown: " + e.which);
                
                // Eğer öneri listesi varsa, enter ve ok tuşlarını yakala
                if (!jq("#urunAramaOneriContainer").is(":empty") && 
                    jq("#urunAramaOneriContainer .list-group").length > 0) {
                    
                    // Enter tuşu (13)
                    if (e.which == 13) {
                        console.log("Arama kutusunda Enter tuşu yakalandı");
                        
                        // Normal davranışı engelle
                        e.preventDefault();
                        e.stopPropagation();
                        
                        // Seçili öğe yoksa ilk öğeyi seç
                        if (window.selectedNavigationIndex < 0) {
                            window.selectedNavigationIndex = 0;
                            jq(".urun-oneri-item").first().addClass("active");
                        }
                        
                        // Seçili ürünü al
                        var items = jq(".urun-oneri-item");
                        if (items.length > 0) {
                            try {
                                var selectedItem = jq(items[window.selectedNavigationIndex]);
                                var urunId = selectedItem.data("urun-id");
                                
                                console.log("Arama kutusundan ürün seçiliyor - ID: " + urunId);
                                
                                // Görsel geri bildirim
                                selectedItem.addClass("active").css("background-color", "#dff0d8");
                                
                                // Küçük bir gecikme ile ürünü seç
                                setTimeout(function() {
                                    // secUrun fonksiyonunu çağır
                                    secUrun(urunId);
                                    
                                    // Dropdown'ı temizle
                                    jq("#urunAramaOneriContainer").empty();
                                }, 50);
                                
                                return false;
                            } catch (err) {
                                console.error("Arama kutusu Enter işlenirken hata:", err);
                            }
                        }
                        
                        return false;
                    }
                    // Ok tuşları
                    else if (e.which == 38 || e.which == 40) {
                        e.preventDefault();
                        e.stopPropagation();
                        return false;
                    }
                }
            });
            
            // Sayfa herhangi bir yerine tıklandığında öneri listesini gizle
            jq(document).on("click", function(e) {
                // Test modunda ise ve test butonlarına tıklanmadıysa, öneri listesini temizleme
                if (window.testMode && !jq(e.target).closest("button").length) {
                    return;
                }
                
                if (!jq(e.target).closest("#<%=txtUrunAra.ClientID %>").length && !jq(e.target).closest(".urun-oneri-item").length) {
                    jq("#urunAramaOneriContainer").empty();
                    window.testMode = false;
                }
            });
            
            // Ana sayfadaki arama kutusu enter tuşu olayı
            jq("#<%=txtUrunAra.ClientID %>").keypress(function (e) {
                if (e.which == 13) {
                    araUrunler();
                    return false;
                }
            });
            
            // Ürün arama işlemleri
            jq("#txtModalUrunAra").keypress(function (e) {
                if (e.which == 13) {
                    araUrunlerModal();
                    return false;
                }
            });
            
            jq("#btnModalUrunAra").click(function () {
                araUrunlerModal();
            });
            
            // Ürün Detayı Modal işlemleri
            jq("#txtModalMiktar, #txtModalFiyat, #txtModalIndirim, #ddlModalKDV, #ddlModalIndirimTuru").change(function () {
                hesaplaToplam();
            });
            
            jq("#btnUrunEkle").click(function () {
                urunEkle();
            });
            
            // Sayfa yüklendiğinde mevcut sepet ürünlerini göster
            setTimeout(function() {
                console.log("Sayfa yüklendi, sepet ürünleri güncelleniyor...");
                updateGridView();
                
                // GridView'ın görünürlüğünü kontrol et ve uyarı mesajını düzenle
                var sepetUrunleri = jq("#<%=gvUrunler.ClientID %> tbody tr").not(":has(td[colspan='9'])");
                if (sepetUrunleri.length > 0) {
                    jq("#urunYokUyari").hide();
                } else {
                    jq("#urunYokUyari").show();
                }
            }, 1000);
            
            // Modal kapatıldığında sepeti güncelle
            jq('#urunDetayModal').on('hidden.bs.modal', function () {
                setTimeout(function() {
                    updateGridView();
                }, 200);
            });
        });
        
        // Ürün Detay Modal Dialog için olay dinleyicisi
        jq(document).ready(function() {
            // Modal açıldığında otomatik olarak miktar alanına odaklan
            jq('#urunDetayModal').on('shown.bs.modal', function () {
                // Önceki modalın tamamen kapandığından emin ol
                jq("#urunAramaModal").modal('hide');
                jq('body').removeClass('modal-open');
                jq('.modal-backdrop').remove();
                
                // Depo listesini her modal açıldığında yeniden yükle
                console.log("Modal açıldı, depo listesi yeniden yükleniyor");
                loadDepolar();
                
                setTimeout(function() {
                    try {
                        var miktarElement = document.getElementById('<%= txtModalMiktar.ClientID %>');
                        if (miktarElement) {
                            miktarElement.focus();
                            miktarElement.select();
                            console.log("Modal açıldı, miktar alanına odaklanıldı");
                            
                            // Miktar alanına Enter tuşu eventi ekle
                            jq(miktarElement).off('keypress').on('keypress', function(e) {
                                if (e.which == 13) { // Enter tuşu
                                    e.preventDefault();
                                    console.log("Miktar alanında Enter tuşuna basıldı");
                                    jq("#btnUrunEkle").click(); // Ekle butonunu otomatik tıkla
                                    return false;
                                }
                            });
                        } else {
                            console.warn("Miktar alanı bulunamadı (txtModalMiktar)");
                        }
                    } catch (err) {
                        console.error("Odaklanma sırasında hata:", err);
                    }
                }, 300);
            });
            
            // Ürün detay modalı kapandığında temizlik yap
            jq('#urunDetayModal').on('hidden.bs.modal', function () {
                console.log("Ürün detay modalı kapandı, temizlik yapılıyor");
                // Modallar ve arka planlarını temizle
                jq('body').removeClass('modal-open');
                jq('.modal-backdrop').remove();
            });
            
            // Ürün ekle butonuna tıklandığında
            jq("#btnUrunEkle").click(function() {
                urunEkle();
            });
            
            // Belge düzeyinde ürün tablosunu ve uyarı mesajını kontrol et
            var gvTable = jq("#<%=gvUrunler.ClientID %>");
            if (gvTable.length === 0) {
                gvTable = jq("#ContentPlaceHolder1_gvUrunler");
            }
            
            if (gvTable.length > 0) {
                console.log("Ürün tablosu bulundu, stiller düzenleniyor");
                gvTable.addClass("table-bordered table-striped");
                
                // Tablo görünürlüğünü ve uyarı mesajını kontrol et
                setTimeout(function() {
                    var hasProducts = jq("#<%=gvUrunler.ClientID %> tbody tr").not(":has(td[colspan])").length > 0;
                    if (hasProducts) {
                        jq("#urunYokUyari").hide();
                    } else {
                        jq("#urunYokUyari").show();
                    }
                }, 1500);
            } else {
                console.warn("Ürün tablosu bulunamadı!");
            }
        });
        
        // Dropdown için arama
        function araUrunlerDropdown(aramaMetni) {
            console.log("araUrunlerDropdown çağrıldı: " + aramaMetni);
            
            // Test modunu devre dışı bırak
            window.testMode = false;
            
            // Arama metni kontrolü
            if (aramaMetni === undefined || aramaMetni === null) {
                aramaMetni = "";
                console.log("Arama metni tanımsız, boş bir sorgu yapılacak");
            }
            
            // En az 3 karakter kontrolü - test ediliyor
            if (aramaMetni.length > 0 && aramaMetni.length < 3) {
                jq("#urunAramaOneriContainer").html('<div class="alert alert-warning">En az 3 karakter girmelisiniz.</div>');
                return;
            }
            
            // Arama devam ediyor mesajı göster
            jq("#urunAramaOneriContainer").html('<div class="alert alert-info">Arama yapılıyor...</div>');
            
            // AJAX çağrısı ile ürünleri ara
            PageMethods.UrunAra(
                aramaMetni, 
                function (sonuclar) {
                    console.log("Sonuçlar geldi:", sonuclar);
                    
                    // Test ürünleri için özel durum kaldırıldı - gerçek ürünlere odaklanıyoruz
                    
                    var oneriHtml = "";
                    if (sonuclar && sonuclar.length > 0) {
                        oneriHtml = '<div class="list-group" style="position:absolute; z-index:1000; width:92%;">';
                        for (var i = 0; i < sonuclar.length; i++) {
                            var urun = sonuclar[i];
                            var stokDurumu = "";
                            
                            // Stok durumunu göster
                            if (urun.StokMiktari !== undefined) {
                                var stokClass = (urun.StokMiktari > 0) ? "text-success" : "text-danger";
                                stokDurumu = ' <span class="' + stokClass + '">(' + 
                                             (urun.StokMiktari > 0 ? urun.StokMiktari + ' ' + (urun.BirimAdi || 'Adet') + ' stokta' : 'Stokta yok') + 
                                             ')</span>';
                            }
                            
                            oneriHtml += '<a href="javascript:void(0);" class="list-group-item urun-oneri-item" data-urun-id="' + urun.UrunID + '">' +
                                         '<strong>' + urun.UrunAdi + '</strong>' + 
                                         (urun.UrunKodu ? ' <small>(' + urun.UrunKodu + ')</small>' : '') + 
                                         (urun.Barkod ? ' <small>Barkod: ' + urun.Barkod + '</small>' : '') +
                                         stokDurumu +
                                         '</a>';
                        }
                        oneriHtml += '</div>';
                    } else {
                        oneriHtml = '<div class="alert alert-warning">Sonuç bulunamadı</div>';
                    }
                    
                    jq("#urunAramaOneriContainer").html(oneriHtml);
                    
                    // Arama kutusuna odağı geri getir
                    jq("#<%=txtUrunAra.ClientID %>").focus();
                    
                    // Öneriye tıklandığında
                    jq(".urun-oneri-item").click(function() {
                        var urunId = jq(this).data("urun-id");
                        secUrun(urunId);
                        jq("#urunAramaOneriContainer").empty();
                    });
                    
                    // AJAX sonuçları geldiğinde yeniden klavye gezinme özelliğini etkinleştir
                    console.log("AJAX sonuçları için klavye navigasyonu etkinleştiriliyor");
                    setupArrowNavigation();
                }, 
                function (hata) {
                    console.error("Arama sırasında hata:", hata);
                    jq("#urunAramaOneriContainer").html('<div class="alert alert-danger">Arama sırasında bir hata oluştu: ' + hata.get_message() + '</div>');
                }
            );
        }
        
        // Yön tuşları ile gezinme
        function setupArrowNavigation() {
            // Global değişkeni sıfırla ve listeyi hazır et
            window.selectedNavigationIndex = 0;
            window.listNavigationEnabled = true;
            
            var items = jq(".urun-oneri-item");
            
            // Önce tüm öğelerin vurgusunu kaldır
            jq(".urun-oneri-item").removeClass("active");
            
            // İlk öğeyi seç
            if (items.length > 0) {
                jq(items[0]).addClass("active");
            }
            
            // Enter tuşu için olay dinleyici
            jq(document).off("keydown.enterKey").on("keydown.enterKey", function(e) {
                if (e.which == 13 && window.selectedNavigationIndex >= 0) {
                    console.log("Enter tuşu basıldı, seçili öğe tıklanacak");
                    
                    var items = jq(".urun-oneri-item");
                    if (items.length === 0) return;
                    
                    try {
                        var selectedItem = jq(items[window.selectedNavigationIndex]);
                        var urunId = selectedItem.data("urun-id");
                        
                        console.log("Seçilen ürün ID:", urunId);
                        
                        // Seçilen öğeye tıklama animasyonu ekle
                        selectedItem.addClass("active").css("background-color", "#dff0d8");
                        setTimeout(function() {
                            selectedItem.css("background-color", "");
                        }, 300);
                        
                        // Modal'ı açmak için ürünü seç
                        secUrun(urunId);
                        
                        // Öneri listesini temizle
                        jq("#urunAramaOneriContainer").empty();
                        
                        e.preventDefault();
                        return false;
                    } catch (err) {
                        console.error("Enter tuşu işlenirken hata:", err);
                    }
                }
                // Escape tuşu (27)
                else if (e.which == 27) {
                    console.log("Escape tuşu basıldı, öneri listesi temizlenecek");
                    jq("#urunAramaOneriContainer").empty();
                    e.preventDefault();
                }
            });
        }
        
        // Tüm dokümanda Enter tuşunu izle
        jq(document).on("keyup", function(e) {
            // Enter tuşuna basıldı ve ürün önerileri varsa
            if (e.which == 13 && jq(".urun-oneri-item").length > 0 && window.selectedNavigationIndex >= 0) {
                console.log("Global Enter tuşu yakalandı - seçili indeks: " + window.selectedNavigationIndex);
                
                var items = jq(".urun-oneri-item");
                if (items.length === 0) return;
                
                try {
                    var selectedItem = jq(items[window.selectedNavigationIndex]);
                    var urunId = selectedItem.data("urun-id");
                    
                    // secUrun fonksiyonunu çağırarak ürün detaylarını göster
                    console.log("Ürün seçiliyor (global Enter) - ID: " + urunId);
                    secUrun(urunId);
                    
                    // Öneri listesini temizle
                    jq("#urunAramaOneriContainer").empty();
                } catch (err) {
                    console.error("Global Enter işlenirken hata:", err);
                }
            }
        });
        
        // Ürün arama modalını aç
        function araUrunler() {
            // İşlevi devre dışı bırak - kullanılmayan özellik
            console.log("araUrunler işlevi devre dışı bırakıldı");
            return false;
        }
        
        // Modal içinde ürün ara
        function araUrunlerModal() {
            var aramaMetni = jq("#txtModalUrunAra").val().trim();
            
            // Arama başlıyor mesajı göster
            var tbody = jq("#tblUrunSonuc tbody");
            tbody.empty();
            
            // Metin boşsa tüm ürünleri göster
            if (aramaMetni.length === 0) {
                tbody.append("<tr><td colspan='6' class='text-center'>Tüm ürünler gösteriliyor...</td></tr>");
                aramaMetni = ""; // Boş sorgu - tüm ürünleri getirecek
            } else if (aramaMetni.length < 2) {
                tbody.append("<tr><td colspan='6' class='text-center'>En az 2 karakter girmelisiniz</td></tr>");
                return;
            } else {
                tbody.append("<tr><td colspan='6' class='text-center'>Arama yapılıyor...</td></tr>");
            }
            
            // AJAX çağrısı ile ürünleri ara
            PageMethods.UrunAra(aramaMetni, function (sonuclar) {
                tbody.empty();
                
                console.log("Dönen sonuç:", sonuclar); // Konsola sonuçları yaz
                
                // Test ürünleri için özel durum kaldırıldı - gerçek ürünlere odaklanıyoruz
                
                if (!sonuclar || sonuclar.length === 0) {
                    tbody.append("<tr><td colspan='6' class='text-center'>Sonuç bulunamadı.</td></tr>");
                } else {
                    $.each(sonuclar, function (i, urun) {
                        // Stok bilgisi
                        var stokDurumu = urun.StokMiktari !== undefined ? urun.StokMiktari.toFixed(2) + " " + (urun.BirimAdi || "Adet") : "-";
                        var stokClass = urun.StokMiktari > 0 ? "text-success" : "text-danger";
                        
                        var row = "<tr>" +
                                  "<td>" + (urun.UrunKodu || "-") + "</td>" +
                                  "<td class='" + stokClass + "'>" + stokDurumu + "</td>" +
                                  "<td>" + (urun.Barkod || "-") + "</td>" +
                                  "<td>" + urun.UrunAdi + "</td>" +
                                  "<td class='text-right'>" + (urun.SatisFiyati || 0).toFixed(2) + " TL</td>" +
                                  "<td class='text-center'>" +
                                  "<button type='button' class='btn btn-xs btn-success' onclick='secUrun(" + urun.UrunID + ")'><i class='icon-plus'></i> Seç</button>" +
                                  "</td>" +
                                  "</tr>";
                        tbody.append(row);
                    });
                }
            }, function (hata) {
                console.error("Hata oluştu:", hata); // Hatayı konsola yazdır
                tbody.empty();
                tbody.append("<tr><td colspan='6' class='text-center text-danger'>Hata: " + hata.get_message() + "</td></tr>");
                alert("Ürün arama sırasında bir hata oluştu: " + hata.get_message());
            });
        }
        
        // Ürün seç ve detay modalını aç
        function secUrun(urunID) {
            console.log("secUrun fonksiyonu çağrıldı - Ürün ID:", urunID);
            
            // Önce tüm açık modalları kapat
            jq("#urunAramaModal").modal('hide');
            jq('body').removeClass('modal-open');
            jq('.modal-backdrop').remove();
            
            // ID kontrolü
            if (!urunID || isNaN(parseInt(urunID))) {
                console.error("Geçersiz ürün ID:", urunID);
                showErrorMessage("Hata", "Geçersiz ürün ID. Lütfen tekrar deneyin.");
                return;
            }
            
            // ID'yi tam sayıya çevir
            urunID = parseInt(urunID);
            
            // Form alanlarını sıfırla
            try {
                // Miktar alanını 1'e resetle
                var miktarElement = document.getElementById('<%= txtModalMiktar.ClientID %>');
                if (miktarElement) {
                    miktarElement.value = "1";
                    console.log("Miktar alanı resetlendi: 1");
                }
                
                // Diğer alanları da sıfırla
                jq("#txtModalAciklama").val("");
                jq("#txtModalIndirim").val("0");
                jq("#ddlModalIndirimTuru").val("%");
            } catch (err) {
                console.error("Form sıfırlama sırasında hata:", err);
            }
            
            // Normal ürünler için AJAX ile veri çek
            console.log("Ürün detayları için AJAX çağrısı - ID:", urunID);
            PageMethods.GetUrunDetay(urunID, function (urun) {
                if (urun) {
                    console.log("Ürün detayları başarıyla alındı:", urun);
                    
                    // Form değerlerini doldur
                    jq("#hdnUrunID").val(urun.UrunID);
                    
                    // Modal başlığını ürün adıyla değiştir
                    jq("#urunDetayModalLabel").text(urun.UrunAdi);
                    
                    // Stok miktarı
                    jq("#lblModalStokMiktari").text(urun.StokMiktari !== undefined ? urun.StokMiktari.toFixed(2) : "0.00");
                    jq("#lblModalBirimAdi").text(urun.BirimAdi || "Adet");
                    
                    // Depo dropdown'unu güncelle - loadDepolar ile güncel listeyi kullan
                    console.log("secUrun - Depo dropdown güncelleniyor");
                    loadDepolar();
                    
                    // Fiyat 
                    jq("#txtModalFiyat").val(urun.BirimFiyat !== undefined ? urun.BirimFiyat.toFixed(2) : (urun.SatisFiyati ? urun.SatisFiyati.toFixed(2) : "0.00"));
                    
                    // KDV seçeneğini ayarla
                    var kdvValue = urun.KDVOrani || 18;
                    jq("#ddlModalKDV option").each(function() {
                        if (parseInt(jq(this).val()) === kdvValue) {
                            jq(this).prop("selected", true);
                            return false;
                        }
                    });
                    
                    // Toplamı hesapla ve modalı göster
                    hesaplaToplam();
                    jq("#urunAramaModal").modal("hide");
                    jq("#urunDetayModal").modal("show");
                    
                    // İmleç Miktar alanına odaklansın
                    setTimeout(function() {
                        try {
                            var miktarElement = document.getElementById('<%= txtModalMiktar.ClientID %>');
                            if (miktarElement) {
                                miktarElement.focus();
                                miktarElement.select();
                                console.log("Normal ürün için miktar alanına odaklanıldı");
                            } else {
                                console.warn("Miktar alanı bulunamadı (txtModalMiktar)");
                            }
                        } catch (err) {
                            console.error("Odaklanma sırasında hata:", err);
                        }
                    }, 500);
                } else {
                    showErrorMessage("Hata", "Ürün detayları alınamadı.");
                }
            }, function (hata) {
                console.error("AJAX hatası:", hata);
                showErrorMessage("Hata", "Ürün detayları alınırken bir hata oluştu: " + hata.get_message());
            });
        }
        
        // Toplam tutarı hesapla
        function hesaplaToplam(source) {
            try {
                console.log("hesaplaToplam fonksiyonu çağrıldı, kaynak:", source);
                
                // Miktar değerini al (ASP.NET kontrolünden)
                var miktarElement = document.getElementById('<%= txtModalMiktar.ClientID %>');
                console.log("Miktar elementi:", miktarElement);
                
                var miktar = 0;
                if (miktarElement) {
                    miktar = parseFloat(miktarElement.value.replace(",", ".")) || 0;
                } else {
                    console.warn("txtModalMiktar elementi bulunamadı, varsayılan değer 1 kullanılıyor");
                    miktar = 1;
                }
                
                var birimFiyat = parseFloat(jq("#txtModalFiyat").val().replace(",", ".")) || 0;
                var indirim = parseFloat(jq("#txtModalIndirim").val().replace(",", ".")) || 0;
                var indirimTuru = jq("#ddlModalIndirimTuru").val();
                var kdvOrani = parseInt(jq("#ddlModalKDV").val()) || 0;
                
                // Eğer kaynak TOPLAM ise, fiyatı geri hesapla
                if (source === 'toplam') {
                    var genelToplam = parseFloat(jq("#lblModalToplamInput").val().replace(",", ".")) || 0;
                    
                    // Toplam değerinden geri hesaplama
                    // Net toplam = Genel toplam / (1 + KDV oranı / 100)
                    var netToplam = genelToplam / (1 + (kdvOrani / 100));
                    
                    // İndirim tutarını hesapla
                    var indirimTutari = 0;
                    var brutToplam = 0;
                    
                    if (indirimTuru === "%") {
                        // Brüt toplam = Net toplam / (1 - indirim yüzdesi / 100)
                        brutToplam = netToplam / (1 - (indirim / 100));
                        indirimTutari = brutToplam * indirim / 100;
                    } else {
                        // Brüt toplam = Net toplam + sabit indirim tutarı
                        brutToplam = netToplam + indirim;
                        indirimTutari = indirim;
                    }
                    
                    // Birim fiyatı hesapla (Brüt toplam / miktar)
                    birimFiyat = miktar > 0 ? brutToplam / miktar : 0;
                    
                    // Birim fiyatı güncelle
                    jq("#txtModalFiyat").val(birimFiyat.toFixed(2));
                    
                    // KDV tutarını hesapla
                    var kdvTutari = netToplam * kdvOrani / 100;
                    
                    // KDV ayrıntısını göster
                    jq("#lblModalNetKdv").text("(" + netToplam.toFixed(2) + " + " + kdvTutari.toFixed(2) + ")");
                    
                    console.log("Toplam değişti - Yeni hesaplanan fiyat:", birimFiyat.toFixed(2));
                } else {
                    console.log("Hesaplama değerleri - Miktar:", miktar, "Birim Fiyat:", birimFiyat, "İndirim:", indirim);
                    
                    var araToplam = miktar * birimFiyat;
                    
                    // İndirim hesapla
                    var indirimTutari = 0;
                    if (indirimTuru === "%") {
                        indirimTutari = araToplam * indirim / 100;
                    } else {
                        indirimTutari = indirim;
                    }
                    
                    var indirimsizToplam = araToplam - indirimTutari;
                    
                    // KDV hesapla
                    var kdvTutari = indirimsizToplam * kdvOrani / 100;
                    
                    // Genel toplam
                    var genelToplam = indirimsizToplam + kdvTutari;
                    
                    // Ekranda göster
                    jq("#lblModalToplamInput").val(genelToplam.toFixed(2));
                    jq("#lblModalNetKdv").text("(" + indirimsizToplam.toFixed(2) + " + " + kdvTutari.toFixed(2) + ")");
                }
            } catch (err) {
                console.error("hesaplaToplam fonksiyonunda hata:", err);
            }
        }
        
        // Miktar ve Fiyat değiştiğinde toplam hesabını güncelle
        jq(document).ready(function() {
            // Miktar, fiyat, indirim veya KDV değiştiğinde toplam hesaplanır
            jq("#txtModalFiyat, #txtModalIndirim, #ddlModalKDV, #ddlModalIndirimTuru").on("input change", function() {
                hesaplaToplam('normal');
            });
            
            // ASP.NET tarafından oluşturulan kontrol için ID'yi doğru şekilde al
            var miktarID = '<%= txtModalMiktar.ClientID %>';
            console.log("Miktar alanı ID:", miktarID);
            
            // Miktar alanı için özel event listener
            jq(document).on("input change", "#" + miktarID, function() {
                console.log("Miktar değişti:", jq(this).val());
                hesaplaToplam('normal');
            });
            
            // Toplam değiştiğinde fiyat hesaplanır (miktar sabit kalır)
            jq("#lblModalToplamInput").on("input change", function() {
                hesaplaToplam('toplam');
            });
            
            // Modal açıldığında da hesaplama yap
            jq('#urunDetayModal').on('shown.bs.modal', function() {
                setTimeout(function() {
                    hesaplaToplam('normal');
                }, 300);
            });
        });
        
        // Ürünü sepete ekle
        function urunEkle() {
            try {
                var urunID = parseInt(jq("#hdnUrunID").val());
                
                // Miktar değerini al (ASP.NET kontrolünden)
                var miktarElement = document.getElementById('<%= txtModalMiktar.ClientID %>');
                var miktar = 0;
                if (miktarElement) {
                    miktar = parseFloat(miktarElement.value.replace(",", ".")) || 0;
                } else {
                    console.warn("txtModalMiktar elementi bulunamadı");
                    miktar = 0;
                }
                
                var birimFiyat = parseFloat(jq("#txtModalFiyat").val().replace(",", ".")) || 0;
                var kdvOrani = parseInt(jq("#ddlModalKDV").val()) || 0;
                var indirim = parseFloat(jq("#txtModalIndirim").val().replace(",", ".")) || 0;
                var indirimTuru = jq("#ddlModalIndirimTuru").val();
                var aciklama = jq("#txtModalAciklama").val();
                var depoID = parseInt(jq("#ddlModalDepo").val()) || 1; // Varsayılan Ana Depo
                
                // Validasyon
                if (miktar <= 0) {
                    showWarningMessage("Uyarı", "Miktar sıfırdan büyük olmalıdır.");
                    return;
                }
                
                if (birimFiyat <= 0) {
                    showWarningMessage("Uyarı", "Birim fiyat sıfırdan büyük olmalıdır.");
                    return;
                }
                
                if (depoID <= 0) {
                    showWarningMessage("Uyarı", "Geçerli bir depo seçmelisiniz.");
                    return;
                }
                
                // Ürün bilgilerini al
                var urunAdi = jq("#urunDetayModalLabel").text();
                var urunKodu = ""; // jq("#lblModalUrunKodu").text();
                var birimAdi = jq("#lblModalBirimAdi").text();
                
                // Ürün detaylarını oluştur
                var urunDetay = {
                    UrunID: urunID,
                    UrunKodu: urunKodu,
                    UrunAdi: urunAdi,
                    Miktar: miktar,
                    Birim: birimAdi,
                    BirimFiyat: birimFiyat,
                    Iskonto: indirim,
                    IskontoTuru: indirimTuru,
                    KDV: kdvOrani,
                    Aciklama: aciklama,
                    DepoID: depoID
                };
                
                console.log("Eklenecek ürün detayları:", urunDetay);
                
                // AJAX ile ürünü GridView'a ekle
                jq("#urunYukleniyorMessage").show();
                
                PageMethods.UrunSepeteEkle(
                    urunID, 
                    miktar, 
                    birimFiyat, 
                    kdvOrani, 
                    indirim, 
                    indirimTuru, 
                    aciklama,
                    depoID, 
                    function(basarili) {
                        jq("#urunYukleniyorMessage").hide();
                        
                        if (basarili) {
                            // Başarılı ise GridView'u güncelle
                            console.log("Ürün başarıyla eklendi");
                            
                            // Modal'ı kapat
                            jq("#urunDetayModal").modal("hide");
                            
                            // GridView'u güncelle (sayfa yenilemeden)
                            updateGridView();
                            
                            // Arama kutusunu temizle ve odakla
                            setTimeout(function() {
                                jq("#<%=txtUrunAra.ClientID %>").val("").focus();
                            }, 500);
                        } else {
                            console.error("Ürün eklenemedi: Sunucu false döndü");
                            showErrorMessage("Hata", "Ürün eklenirken bir hata oluştu. Sunucu işlemi reddetti.");
                        }
                    }, 
                    function(hata) {
                        jq("#urunYukleniyorMessage").hide();
                        console.error("Ürün eklenirken hata:", hata);
                        console.error("Hata detayları:", hata.get_message(), hata.get_stackTrace());
                        showErrorMessage("Hata", "Ürün eklenirken bir hata oluştu: " + hata.get_message());
                    }
                );
            } catch (err) {
                console.error("urunEkle fonksiyonunda hata:", err);
                showErrorMessage("Hata", "Ürün ekleme işlemi sırasında bir hata oluştu: " + err.message);
            }
        }
        
        // GridView'u güncelle (sayfa yenilemeden)
        function updateGridView() {
            try {
                console.log("updateGridView fonksiyonu çağrıldı");
                
                PageMethods.GetSepetUrunleri(
                    function(urunler) {
                        console.log("Güncel sepet ürünleri:", urunler);
                        
                        // Uyarı mesajını gizle/göster
                        if (urunler && urunler.length > 0) {
                            jq("#urunYokUyari").hide();
                            jq("#urunTabloContainer").show();
                        } else {
                            jq("#urunYokUyari").show();
                            jq("#urunTabloContainer").hide();
                        }
                        
                        // GridView'u güncelle - daha fazla seçici dene
                        var gvTable = null;
                        
                        // Farklı seçici stratejileri dene
                        if (jq("#<%=gvUrunler.ClientID %>").length > 0) {
                            gvTable = jq("#<%=gvUrunler.ClientID %>");
                            console.log("GridView bulundu: ClientID");
                        } 
                        else if (jq("#ContentPlaceHolder1_gvUrunler").length > 0) {
                            gvTable = jq("#ContentPlaceHolder1_gvUrunler");
                            console.log("GridView bulundu: ContentPlaceHolder1_gvUrunler");
                        }
                        else if (jq("[id$='gvUrunler']").length > 0) {
                            gvTable = jq("[id$='gvUrunler']");
                            console.log("GridView bulundu: id son eki gvUrunler");
                        }
                        else if (jq(".table.table-striped.table-hover.table-bordered").length > 0) {
                            gvTable = jq(".table.table-striped.table-hover.table-bordered");
                            console.log("GridView bulundu: CSS sınıfları ile");
                        }
                        else if (jq("#urunTabloContainer table").length > 0) {
                            gvTable = jq("#urunTabloContainer table");
                            console.log("GridView bulundu: tablo container içinde");
                        }
                        
                        if (!gvTable || gvTable.length === 0) {
                            console.error("GridView tablosu bulunamadı! DOM yapısı inceleniyor...");
                            var containers = jq("#urunTabloContainer");
                            console.log("urunTabloContainer bulundu mu:", containers.length > 0);
                            
                            if (containers.length > 0) {
                                // Tablo yok ama container var, yeni tablo oluştur
                                console.log("Tablo container var ama tablo yok, oluşturuluyor");
                                containers.html('<table id="dynamicGridView" class="table table-striped table-hover table-bordered" width="100%"><thead><tr><th>Ürün Kodu</th><th>Ürün Adı</th><th>Miktar</th><th>Birim</th><th>Birim Fiyat</th><th>İskonto</th><th>KDV</th><th>Tutar</th><th>İşlemler</th></tr></thead><tbody></tbody></table>');
                                gvTable = jq("#dynamicGridView");
                            } else {
                                return; // Container bile yoksa çık
                            }
                        }
                        
                        console.log("GridView tablosu bulundu:", gvTable);
                        var gvBody = gvTable.find("tbody");
                        
                        if (gvBody.length === 0) {
                            console.log("tbody bulunamadı, oluşturuluyor...");
                            gvBody = jq("<tbody></tbody>");
                            gvTable.append(gvBody);
                        }
                        
                        console.log("GridView body bulundu:", gvBody);
                        gvBody.empty();
                        
                        if (urunler && urunler.length > 0) {
                            jq.each(urunler, function(index, urun) {
                                // Tutar hesapla
                                var araToplam = urun.Miktar * urun.BirimFiyat;
                                var indirimTutari = 0;
                                
                                if (urun.IskontoTuru === "%") {
                                    indirimTutari = araToplam * urun.Iskonto / 100;
                                } else {
                                    indirimTutari = urun.Iskonto;
                                }
                                
                                var netTutar = araToplam - indirimTutari;
                                var kdvTutari = netTutar * urun.KDV / 100;
                                var toplamTutar = netTutar + kdvTutari;
                                
                                var row = "<tr>" +
                                        "<td>" + (urun.UrunKodu || "-") + "</td>" +
                                        "<td>" + urun.UrunAdi + "</td>" +
                                        "<td class='text-right'>" + urun.Miktar.toFixed(2) + "</td>" +
                                        "<td>" + (urun.Birim || "-") + "</td>" +
                                        "<td class='text-right'>" + urun.BirimFiyat.toFixed(2) + " TL</td>" +
                                        "<td class='text-right'>" + urun.Iskonto + (urun.IskontoTuru === "%" ? "%" : " TL") + "</td>" +
                                        "<td class='text-right'>" + urun.KDV + "%</td>" +
                                        "<td class='text-right'>" + toplamTutar.toFixed(2) + " TL</td>" +
                                        "<td class='text-center'>" +
                                        "<button type='button' class='btn btn-xs btn-primary' onclick='duzenleUrun(" + urun.UrunID + ")'><i class='icon-pencil'></i></button> " +
                                        "<button type='button' class='btn btn-xs btn-danger' onclick='silUrun(" + urun.UrunID + ")'><i class='icon-trash'></i></button>" +
                                        "</td>" +
                                        "</tr>";
                                gvBody.append(row);
                            });
                            
                            // Tabloyu görünür hale getir
                            gvTable.show();
                            jq("#urunYokUyari").hide();
                            
                            // Özet bilgiler ve toplamlara ait tablo ekle
                            var toplamMiktar = 0;
                            var brutToplam = 0;
                            var indirimToplam = 0;
                            var netToplam = 0;
                            var kdvToplam = 0;
                            var genelToplam = 0;
                            
                            // Tüm ürünlerin toplamlarını hesapla
                            jq.each(urunler, function(index, urun) {
                                var araToplam = urun.Miktar * urun.BirimFiyat;
                                var indirimTutari = 0;
                                
                                if (urun.IskontoTuru === "%") {
                                    indirimTutari = araToplam * urun.Iskonto / 100;
                                } else {
                                    indirimTutari = urun.Iskonto;
                                }
                                
                                toplamMiktar += urun.Miktar;
                                brutToplam += araToplam;
                                indirimToplam += indirimTutari;
                                
                                var urunNetToplam = araToplam - indirimTutari;
                                netToplam += urunNetToplam;
                                
                                var urunKdvTutari = urunNetToplam * urun.KDV / 100;
                                kdvToplam += urunKdvTutari;
                                
                                genelToplam += (urunNetToplam + urunKdvTutari);
                            });
                            
                            // Özet tablosu HTML'i
                            var ozetHTML = '<div class="panel panel-default" style="margin-top:15px;">' +
                                '<div class="panel-heading"><h4 class="panel-title">Özet</h4></div>' +
                                '<div class="panel-body">' +
                                '<table class="table table-condensed" style="width: 50%; margin-left: auto;">' +
                                '<tr><td style="text-align:right;font-weight:bold;width:50%;">Toplam Miktar</td><td style="text-align:right">' + toplamMiktar.toFixed(0) + ' Ad</td></tr>' +
                                '<tr><td style="text-align:right;font-weight:bold;">Brüt Toplam</td><td style="text-align:right">' + brutToplam.toFixed(2) + ' TL</td></tr>' +
                                '<tr><td style="text-align:right;font-weight:bold;">İndirim</td><td style="text-align:right">' + indirimToplam.toFixed(2) + ' TL</td></tr>' +
                                '<tr><td style="text-align:right;font-weight:bold;">Net Toplam</td><td style="text-align:right">' + netToplam.toFixed(2) + ' TL</td></tr>' +
                                '<tr><td style="text-align:right;font-weight:bold;">KDV (%1)</td><td style="text-align:right">' + kdvToplam.toFixed(2) + ' TL</td></tr>' +
                                '<tr><td style="text-align:right;font-weight:bold;font-size:16px;">TOPLAM</td><td style="text-align:right;font-weight:bold;font-size:16px;">' + genelToplam.toFixed(2) + ' TL</td></tr>' +
                                '</table>' +
                                '</div>' +
                                '</div>';
                            
                            // Özet tablosunu ekle
                            if (jq("#siparisToplam").length > 0) {
                                jq("#siparisToplam").remove(); // Varsa mevcut özeti kaldır
                            }
                            
                            jq("#urunTabloContainer").after('<div id="siparisToplam">' + ozetHTML + '</div>');
                        } else {
                            gvBody.append("<tr><td colspan='9' class='text-center'>Henüz ürün eklenmedi</td></tr>");
                            jq("#urunYokUyari").show();
                            
                            // Özet kısmını temizle
                            if (jq("#siparisToplam").length > 0) {
                                jq("#siparisToplam").remove();
                            }
                        }
                        
                        // Basit bir animasyon efekti
                        gvTable.fadeIn(100).fadeOut(100).fadeIn(100);
                    },
                    function(hata) {
                        console.error("Sepet ürünleri alınırken hata:", hata);
                    }
                );
            } catch (err) {
                console.error("updateGridView fonksiyonunda hata:", err);
            }
        }
        
        // Ürün düzenle
        function duzenleUrun(urunID) {
            console.log("Ürün düzenleniyor:", urunID);
            // İlgili ürünü getir ve detay modalını aç
            secUrun(urunID);
        }
        
        // Ürün sil
        function silUrun(urunID) {
            if (confirm("Bu ürünü sepetten çıkarmak istediğinize emin misiniz?")) {
                console.log("Ürün siliniyor:", urunID);
                
                PageMethods.UrunSepettenCikar(
                    urunID,
                    function(basarili) {
                        if (basarili) {
                            console.log("Ürün başarıyla silindi");
                            // GridView'u güncelle
                            updateGridView();
                        } else {
                            showErrorMessage("Hata", "Ürün silinirken bir hata oluştu.");
                        }
                    },
                    function(hata) {
                        console.error("Ürün silinirken hata:", hata);
                        showErrorMessage("Hata", "Ürün silinirken bir hata oluştu: " + hata.get_message());
                    }
                );
            }
        }
        
        // Sayfa genelinde ürünleri göster
        jq(window).on('load', function() {
            console.log("Window load - Sepet ürünleri yeniden yükleniyor");
            setTimeout(function() {
                updateGridView();
            }, 800);
        });
    </script>
</asp:Content>

