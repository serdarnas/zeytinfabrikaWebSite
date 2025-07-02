<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="YeniUrun.aspx.cs" Inherits="fabrika_Urunler_YeniUrun" %>

<%@ Register Src="~/fabrika/Urunler/YeniUrunVaryantWebUserControl.ascx" TagName="VaryantControl" TagPrefix="uc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <!-- Modern CSS -->
    <link href="/App_Themes/serdarnas_admin_flat/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/App_Themes/serdarnas_admin_flat/assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="/App_Themes/serdarnas_admin_flat/css/style.css" rel="stylesheet" />
    <link href="/App_Themes/serdarnas_admin_flat/css/style-responsive.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/serdarnas_admin_flat/assets/jquery-multi-select/css/multi-select.css" />

    <!-- jQuery Multi-Select CSS -->
    <link rel="stylesheet" type="text/css" href="/App_Themes/serdarnas_admin_flat/assets/jquery-multi-select/css/multi-select.css" />



    <!-- jQuery Multi-Select Dependencies -->
    <script src="/App_Themes/serdarnas_admin_flat/assets/jquery-multi-select/js/jquery.multi-select.js"></script>
    <script src="/App_Themes/serdarnas_admin_flat/assets/jquery-multi-select/js/jquery.quicksearch.js"></script>

    <!-- ScriptManager - UpdatePanel için gerekli -->
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true" />

    <div class="row">
        <div class="col-md-10 col-md-offset-1">
            <section class="panel">
                <header class="panel-heading tab-bg-dark-navy-blue">
                    <ul class="nav nav-tabs" id="myTab">
                        <li class="active"><a href="#temel" data-toggle="tab"><i class="fa fa-info-circle"></i>Temel Bilgiler</a></li>
                        <li><a href="#fiyat" data-toggle="tab"><i class="fa fa-money"></i>Fiyat Bilgileri</a></li>
                        <li><a href="#detay" data-toggle="tab"><i class="fa fa-list"></i>Detaylar</a></li>
                        <li id="tabResimler" style="display: none;"><a href="#resimler" data-toggle="tab"><i class="fa fa-picture-o"></i>Resimler</a></li>
                        <li id="tabVaryant" style="display: none;"><a href="#varyant" data-toggle="tab"><i class="fa fa-th"></i>Varyantlar</a></li>
                        <li id="tabPaketleme" style="display: none;"><a href="#paketleme" data-toggle="tab"><i class="fa fa-cube"></i>Paketleme & Lojistik</a></li>
                    </ul>
                </header>
                <div class="panel-body">

                    <div class="tab-content">
                        <!-- Temel Bilgiler -->
                        <div class="tab-pane fade in active" id="temel">
                            <div class="form-horizontal">

                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-cube"></i>Ürün Adı <span class="text-danger">*</span></label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-cube"></i></span>
                                            <asp:TextBox ID="txtUrunAdi" runat="server" CssClass="form-control" placeholder="Ürün Adı" />
                                        </div>
                                    </div>
                                    <label class="col-sm-2 control-label"><i class="fa fa-tags"></i>Kategori</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-tags"></i></span>
                                            <asp:DropDownList ID="ddlKategoriID" runat="server" CssClass="form-control"></asp:DropDownList>
                                            <span class="input-group-btn">
                                                <asp:Button ID="Button1" runat="server" CssClass="btn btn-success" Text="+ Yeni" data-toggle="modal" data-target="#modalYeniKategori" type="button" />
                                                --
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-balance-scale"></i>Birim</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-balance-scale"></i></span>
                                            <asp:DropDownList ID="ddlBirimID" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <label class="col-sm-2 control-label"><i class="fa fa-money"></i>Para Birimi</label>
                                    <div class="col-sm-4">
                                        <asp:DropDownList ID="ddlParaBirimiID" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="TL" Value="1" />
                                            <asp:ListItem Text="USD" Value="2" />
                                            <asp:ListItem Text="EUR" Value="3" />
                                        </asp:DropDownList>
                                    </div>
                                </div>


                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-warning"></i>Ürün Durumlari</label>
                                    <div class="col-lg-8">
                                        <div class="checkbox-inline">
                                            <asp:CheckBox ID="chkMamul" runat="server" Text="Manul" /><br />
                                            <asp:CheckBox ID="chkYariMamul" runat="server" Text="Yari Mamul" /><br />
                                            <asp:CheckBox ID="chkPerakendeSatisVarmi" runat="server" Text="Perekende Satiş Var" /><br />
                                            <asp:CheckBox ID="chkUrunTipiStoklu" runat="server" Checked="true" Text="Stoklu Ürün" /><br />
                                            <asp:CheckBox ID="chkDurum" runat="server" Text="Aktif" Checked="true" /><br />
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <!-- Fiyat Bilgileri -->
                        <div class="tab-pane fade" id="fiyat">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-shopping-cart"></i>Alış Fiyatı</label>
                                    <div class="col-sm-2">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-shopping-cart"></i></span>
                                            <asp:TextBox ID="txtAlisFiyati" runat="server" CssClass="form-control" TextMode="Number" placeholder="0.00" />
                                        </div>
                                    </div>
                                    <label class="col-sm-2 control-label"><i class="fa fa-percent"></i>Alış KDV</label>
                                    <div class="col-sm-2">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-percent"></i></span>
                                            <asp:TextBox ID="txtAlisKdv" runat="server" CssClass="form-control" Text="1" TextMode="Number" placeholder="1" />
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="checkboxes">
                                            <asp:CheckBox ID="chkAlisFiyatiKdvDahilmi" runat="server" Text="Alış Fiyatı KDV Dahil" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">

                                    <label class="col-sm-2 control-label"><i class="fa fa-money"></i>Satış Fiyatı</label>
                                    <div class="col-sm-2">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-money"></i></span>
                                            <asp:TextBox ID="txtSatisFiyati" runat="server" CssClass="form-control" TextMode="Number" placeholder="0.00" />
                                        </div>
                                    </div>

                                    <label class="col-sm-2 control-label"><i class="fa fa-percent"></i>Satış KDV</label>
                                    <div class="col-sm-2">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-percent"></i></span>
                                            <asp:TextBox ID="txtSatisKdv" runat="server" CssClass="form-control" Text="1" TextMode="Number" placeholder="1" />
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="checkboxes">
                                            <asp:CheckBox ID="chkSatisFiyatiKdvDahilmi" runat="server" Text="Satış Fiyatı KDV Dahil" />
                                        </div>
                                    </div>

                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-money"></i>Perakende Fiyatı</label>
                                    <div class="col-sm-2">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-money"></i></span>
                                            <asp:TextBox ID="txtPerakendeSatisFiyati" runat="server" CssClass="form-control" TextMode="Number" placeholder="0.00" />
                                        </div>
                                    </div>
                                    <label class="col-sm-2 control-label"><i class="fa fa-percent"></i>KDV Oranı</label>
                                    <div class="col-sm-2">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-percent"></i></span>
                                            <asp:TextBox ID="txtKDVOrani" runat="server" CssClass="form-control" Text="1" TextMode="Number" placeholder="1" />
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="checkboxes">
                                            <asp:CheckBox ID="chkPerakendeSatisKdvDahilmi" runat="server" Text="Perakende Fiyatı KDV Dahil" />
                                        </div>
                                    </div>


                                </div>
                            </div>
                        </div>
                        <!-- Detaylar -->
                        <div class="tab-pane fade" id="detay">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-trademark"></i>Marka</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-trademark"></i></span>
                                            <asp:DropDownList ID="ddlMarkaID" runat="server" CssClass="form-control"></asp:DropDownList>
                                            <span class="input-group-btn">
                                                <button type="button" class="btn btn-success" data-toggle="modal" data-target="#modalYeniMarka">+ Yeni</button>
                                            </span>
                                        </div>
                                    </div>
                                    <label class="col-sm-2 control-label"><i class="fa fa-barcode"></i>Ürün Kodu</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-barcode"></i></span>
                                            <asp:TextBox ID="txtUrunKodu" runat="server" CssClass="form-control" placeholder="Ürün Kodu" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-warning"></i>Minimum Stok</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-warning"></i></span>
                                            <asp:TextBox ID="txtMinimumStok" runat="server" CssClass="form-control" TextMode="Number" placeholder="0" />
                                        </div>
                                    </div>

                                    <label class="col-sm-2 control-label"><i class="fa fa-cubes"></i>Stok Miktarı</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-cubes"></i></span>
                                            <asp:TextBox ID="txtStokMiktari" runat="server" CssClass="form-control" TextMode="Number" placeholder="0" />
                                        </div>
                                    </div>

                                </div>

                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-barcode"></i>Barkod</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-barcode"></i></span>
                                            <asp:TextBox ID="txtBarkod" runat="server" CssClass="form-control" placeholder="Barkod" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-sticky-note"></i>Notlar</label>
                                    <div class="col-sm-10">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-sticky-note"></i></span>
                                            <asp:TextBox ID="txtNotlar" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Notlar" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Resimler -->
                        <div class="tab-pane fade" id="resimler">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-upload"></i>Resim Yükle</label>
                                    <div class="col-sm-8">
                                        <asp:FileUpload ID="fuResimler" runat="server" AllowMultiple="true" CssClass="form-control" />
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:Button ID="btnResimYukle" runat="server" Text="Yükle" CssClass="btn btn-primary btn-block" OnClick="btnResimYukle_Click" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <asp:Repeater ID="rptResimler" runat="server">
                                            <ItemTemplate>
                                                <div class="col-md-3" style="margin-bottom: 10px;">
                                                    <div class="thumbnail">
                                                        <asp:Image ID="imgUrunResim" runat="server" ImageUrl='<%# Eval("ImageUrl") %>' CssClass="img-responsive" />
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Varyantlar -->
                        <div class="tab-pane fade" id="varyant">
                            <uc:VaryantControl ID="VaryantControl1" runat="server" />
                        </div>
                        <!-- Paketleme & Lojistik -->
                        <div class="tab-pane fade" id="paketleme">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Ürün Net Ağırlık (Ürün gr)</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtUrunNetAgirlik_gr" runat="server" CssClass="form-control" TextMode="Number" placeholder="Ürün Net Ağırlık (gr)" />
                                    </div>
                                    <label class="col-sm-2 control-label">Ürün Brüt Ağırlık (Ürün + Ambalaj gr)</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtUrunBurutAgirlik_gr" runat="server" CssClass="form-control" TextMode="Number" placeholder="Ürün Brüt Ağırlık (gr)" />
                                    </div>
                                </div>
                                <!-- Koli Bilgileri -->
                                <div class="form-section-title"><i class="fa fa-dropbox"></i>Koli Bilgileri</div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Koli İçi Ürün Adedi</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtKoliIciAdet" runat="server" CssClass="form-control" TextMode="Number" placeholder="Koli İçi Adet" />
                                    </div>
                                    <label class="col-sm-2 control-label">Koli Boyutları (cm)</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtKoliUzunluk" runat="server" CssClass="form-control" TextMode="Number" placeholder="U" Style="min-width: 70px; max-width: 110px;" />
                                            <span class="input-group-addon">x</span>
                                            <asp:TextBox ID="txtKoliGenislik" runat="server" CssClass="form-control" TextMode="Number" placeholder="G" Style="min-width: 70px; max-width: 110px;" />
                                            <span class="input-group-addon">x</span>
                                            <asp:TextBox ID="txtKoliYukseklik" runat="server" CssClass="form-control" TextMode="Number" placeholder="Y" Style="min-width: 70px; max-width: 110px;" />

                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Koli Brüt Ağırlığı (Kg)</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtKoliBrutAgirlik" runat="server" CssClass="form-control" TextMode="Number" placeholder="Brüt Ağırlık" />
                                    </div>
                                    <label class="col-sm-2 control-label">Koli Net Ağırlığı (Kg)</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtKoliNetAgirlik" runat="server" CssClass="form-control" TextMode="Number" placeholder="Net Ağırlık" ReadOnly="true" />
                                        <span class="form-text">Ürün ağırlığı ve adedine göre otomatik hesaplanır.</span>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Koli Hacmi (m³)</label>
                                    <div class="col-sm-4">
                                        <div class="calculated-value" id="koliHacmiHesaplanan">-</div>
                                    </div>
                                    <label class="col-sm-2 control-label">Koli Malzemesi</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtKoliMalzemesi" runat="server" CssClass="form-control" placeholder="Örn: Oluklu Mukavva B Dalga" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Koli Barkodu</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtKoliBarkodu" runat="server" CssClass="form-control" placeholder="GTIN-14 / ITF-14" />
                                    </div>
                                </div>
                                <!-- Palet Bilgileri -->
                                <div class="form-section-title"><i class="fa fa-cubes"></i>Palet Bilgileri</div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Palet Tipi</label>
                                    <div class="col-sm-4">
                                        <asp:DropDownList ID="ddlPaletTipi" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Euro Palet (80x120cm)" Value="euro_80x120" Selected="True" />
                                            <asp:ListItem Text="Standart Palet (100x120cm)" Value="standart_100x120" />
                                            <asp:ListItem Text="Özel Boyut" Value="custom" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-6" id="customPaletBoyutlari" style="display: none;">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtPaletUzunlukCustom" runat="server" CssClass="form-control" TextMode="Number" placeholder="Uzunluk" />
                                            <span class="input-group-addon">x</span>
                                            <asp:TextBox ID="txtPaletGenislikCustom" runat="server" CssClass="form-control" TextMode="Number" placeholder="Genişlik" />
                                            <span class="input-group-addon">cm</span>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Paletteki Koli Sayısı</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtPalettekiKoliSayisi" runat="server" CssClass="form-control" TextMode="Number" placeholder="Paletteki Koli" />
                                    </div>
                                    <label class="col-sm-2 control-label">Paletteki Toplam Ürün</label>
                                    <div class="col-sm-4">
                                        <div class="calculated-value" id="palettekiUrunAdediHesaplanan">-</div>
                                        <span class="form-text">Koli içi adet ve koli sayısına göre otomatik hesaplanır.</span>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Yüklü Palet Yüksekliği (cm)</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtPaletYuksekligi" runat="server" CssClass="form-control" TextMode="Number" placeholder="Yüklü Palet Yüksekliği" />
                                    </div>
                                    <label class="col-sm-2 control-label">Yüklü Palet Brüt Ağırlığı (Kg)</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtPaletBrutAgirlik" runat="server" CssClass="form-control" TextMode="Number" placeholder="Brüt Ağırlık" ReadOnly="true" />
                                        <span class="form-text">Koli ağırlığı ve sayısına göre otomatik hesaplanır (palet dara ağırlığı eklenebilir).</span>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Boş Palet Dara Ağırlığı (Kg)</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtPaletDaraAgirligi" runat="server" CssClass="form-control" TextMode="Number" placeholder="Dara Ağırlığı" Text="25" />
                                    </div>
                                    <label class="col-sm-2 control-label">Paletleme Notları</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtPaletlemeNotlari" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Örn: Streç film ile sarılı, Köşe bentli, Maks. 5 kat istif vb." />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-group mt-4">
                        <div class="col-sm-12 text-right">
                            <asp:HyperLink ID="btnIptal" runat="server" CssClass="btn btn-default" NavigateUrl="~/fabrika/Urunler/UrunListesi.aspx">
                            <i class="fa fa-times"></i> İptal
                            </asp:HyperLink>
                            <asp:Button ID="btnKaydet" runat="server" CssClass="btn btn-success" Text="Ürünü Kaydet" OnClick="btnKaydet_Click" />
                        </div>
                    </div>

                </div>
            </section>
        </div>
    </div>


    <!-- Ana JavaScript Fonksiyonları -->
    <script type="text/javascript">
        $(document).ready(function () {
            setupCheckboxVisuals();
            setupKoliHesaplamalar();
            setupTabKontrol();
        });

        // Tab kontrol sistemi
        function setupTabKontrol() {
            // URL'den UrunID parametresini kontrol et
            var urlParams = new URLSearchParams(window.location.search);
            var urunID = urlParams.get('UrunID');

            if (urunID && urunID !== '0') {
                // Ürün var, tüm tabları aktif et
                aktifTablar();
            } else {
                // Yeni ürün, sadece temel bilgiler aktif
                disableTablar();
            }
        }

        // Tabları göster
        function aktifTablar() {
            $('#tabResimler, #tabVaryant, #tabPaketleme').show();
            console.log('Tablar gösterildi');
        }

        // Tabları gizle
        function disableTablar() {
            $('#tabResimler, #tabVaryant, #tabPaketleme').hide();
            console.log('Tablar gizlendi');
        }

        function setupCheckboxVisuals() {
            var d = document;
            var gebtn = function (parEl, child) { return parEl.getElementsByTagName(child); };
            var inputs = gebtn(d, 'input');
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == 'checkbox') {
                    var parent = inputs[i].parentNode;
                    if (parent.tagName.toLowerCase() == 'label' && parent.className.indexOf('label_check') != -1) {
                        parent.className = inputs[i].checked ? 'label_check c_on' : 'label_check c_off';
                        inputs[i].onclick = function () {
                            var parent = this.parentNode;
                            parent.className = this.checked ? 'label_check c_on' : 'label_check c_off';
                        };
                    }
                }
            }
        }

        function setupKoliHesaplamalar() {
            try {
                // Koli Net Ağırlık hesaplama
                var txtKoliIciAdet = $('#<%= txtKoliIciAdet.ClientID %>');
                var txtUrunNetAgirlik_gr = $('#<%= txtUrunNetAgirlik_gr.ClientID %>');
                var txtKoliNetAgirlik = $('#<%= txtKoliNetAgirlik.ClientID %>');

                function hesaplaKoliNetAgirlik() {
                    var koliAdet = parseInt(txtKoliIciAdet.val());
                    var urunNetAgirlik = parseFloat(txtUrunNetAgirlik_gr.val());
                    if (!isNaN(koliAdet) && !isNaN(urunNetAgirlik) && koliAdet > 0 && urunNetAgirlik > 0) {
                        var netAgirlikKg = (koliAdet * urunNetAgirlik) / 1000;
                        txtKoliNetAgirlik.val(netAgirlikKg.toFixed(2));
                    } else {
                        txtKoliNetAgirlik.val('');
                    }
                }

                // Koli hacmi hesaplama
                var txtKoliUzunluk = $('#<%= txtKoliUzunluk.ClientID %>');
                var txtKoliGenislik = $('#<%= txtKoliGenislik.ClientID %>');
                var txtKoliYukseklik = $('#<%= txtKoliYukseklik.ClientID %>');

                function hesaplaKoliHacmi() {
                    var uzunluk = parseFloat(txtKoliUzunluk.val());
                    var genislik = parseFloat(txtKoliGenislik.val());
                    var yukseklik = parseFloat(txtKoliYukseklik.val());

                    if (!isNaN(uzunluk) && !isNaN(genislik) && !isNaN(yukseklik) &&
                        uzunluk > 0 && genislik > 0 && yukseklik > 0) {
                        var hacim = (uzunluk * genislik * yukseklik) / 1000000; // cm³ to m³
                        $('#koliHacmiHesaplanan').text(hacim.toFixed(4) + ' m³');
                    } else {
                        $('#koliHacmiHesaplanan').text('-');
                    }
                }

                // Event listeners - önce kaldır, sonra tekrar bağla
                txtKoliIciAdet.off('input.koliHesap').on('input.koliHesap', hesaplaKoliNetAgirlik);
                txtUrunNetAgirlik_gr.off('input.koliHesap').on('input.koliHesap', hesaplaKoliNetAgirlik);

                txtKoliUzunluk.off('input.koliHacim').on('input.koliHacim', hesaplaKoliHacmi);
                txtKoliGenislik.off('input.koliHacim').on('input.koliHacim', hesaplaKoliHacmi);
                txtKoliYukseklik.off('input.koliHacim').on('input.koliHacim', hesaplaKoliHacmi);

                // Sayfa ilk açıldığında hesaplama yap
                hesaplaKoliNetAgirlik();
                hesaplaKoliHacmi();
            } catch (e) {
                console.error('Koli hesaplama setup hatası:', e);
            }
        }
    </script>

    <script>
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            if ($('#modalYeniKategori').length > 0 && $('#txtYeniKategoriAdi').val() === '') {
                $('#modalYeniKategori').modal('hide');
            }
            if ($('#modalYeniMarka').length > 0 && $('#txtYeniMarkaAdi').val() === '') {
                $('#modalYeniMarka').modal('hide');
            }
        });
    </script>

    <!-- Yeni Kategori Modal -->
    <div class="modal fade" id="modalYeniKategori" tabindex="-1" role="dialog" aria-labelledby="modalYeniKategoriLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div id="formYeniKategori">
                    <div class="modal-header">
                        <h4 class="modal-title" id="modalYeniKategoriLabel">Yeni Kategori Ekle</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Kapat"><span aria-hidden="true">&times;</span></button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label for="txtYeniKategoriAdi">Kategori Adı</label>
                            <asp:TextBox ID="txtYeniKategoriAdi" runat="server" CssClass="form-control" placeholder="Kategori Adı" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnKategoriKaydet" runat="server" CssClass="btn btn-success" Text="Kaydet" OnClick="btnKategoriKaydet_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">İptal</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Yeni Marka Modal -->
    <div class="modal fade" id="modalYeniMarka" tabindex="-1" role="dialog" aria-labelledby="modalYeniMarkaLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div id="formYeniMarka">
                    <div class="modal-header">
                        <h4 class="modal-title" id="modalYeniMarkaLabel"><i class="fa fa-trademark"></i>Yeni Marka Ekle</h4>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Kapat"><span aria-hidden="true">&times;</span></button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label for="txtYeniMarkaAdi"><i class="fa fa-trademark"></i>Marka Adı</label>
                            <asp:TextBox ID="txtYeniMarkaAdi" runat="server" CssClass="form-control" placeholder="Marka Adı" />
                        </div>

                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnMarkaKaydet" runat="server" CssClass="btn btn-success" Text="Kaydet" OnClick="btnMarkaKaydet_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">İptal</button>
                    </div>
                </div>
            </div>
        </div>
    </div>



</asp:Content>

