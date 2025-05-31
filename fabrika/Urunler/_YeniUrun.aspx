<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="YeniUrun.aspx.cs" Inherits="fabrika_Urunler_YeniUrun" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hfActiveTab" runat="server" Value="tanim" />

    <!-- Modern CSS -->
    <link href="/App_Themes/serdarnas_admin_flat/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/App_Themes/serdarnas_admin_flat/assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="/App_Themes/serdarnas_admin_flat/css/style.css" rel="stylesheet" />
    <link href="/App_Themes/serdarnas_admin_flat/css/style-responsive.css" rel="stylesheet" />
    <link href="/App_Themes/serdarnas_admin_flat/assets/bootstrap-colorpicker/css/colorpicker.css" rel="stylesheet" />
    <link href="/App_Themes/serdarnas_admin_flat/assets/bootstrap-fileupload/bootstrap-fileupload.css" rel="stylesheet" />

    <!-- Custom Checkbox styles -->


    <div class="row">
        <div class="col-md-10 col-md-offset-1">
            <section class="panel">
                <header class="panel-heading tab-bg-dark-navy-blue">
                    <ul class="nav nav-tabs" id="myTab">
                        <li class="active"><a href="#tanim" data-toggle="tab"><i class="fa fa-tag"></i>Temel Bilgiler</a></li>
                        <li><a href="#fiyat" data-toggle="tab"><i class="fa fa-money"></i>Fiyat Bilgileri</a></li>
                        <li><a href="#detaylar" data-toggle="tab"><i class="fa fa-info-circle"></i>Detaylar</a></li>
                        <li><a href="#resimler" data-toggle="tab"><i class="fa fa-image"></i>Resimler</a></li>
                        <li><a href="#varyant" data-toggle="tab"><i class="fa fa-cubes"></i>Varyantlar</a></li>
                        <li><a href="#paketleme" data-toggle="tab"><i class="fa fa-truck"></i> Paketleme & Lojistik</a></li>
                    </ul>
                </header>
                <div class="panel-body">
                    <!-- Hata ve Başarı Mesajları -->
                    
                    
                    
                    <!-- Tablar -->
                    <div class="tab-content">
                        <div class="tab-pane fade in active" id="tanim">
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
                                                <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#modalYeniKategori">
                                                    <i class="fa fa-plus"></i>
                                                </button>
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
                                    <div class="col-sm-12">
                                        <div class="checkbox-inline">
                                            <label class="label_check" for="chkUrunTipiStoklu">
                                                <asp:CheckBox ID="chkUrunTipiStoklu" runat="server" />
                                                <span class="checkmark"></span>
                                                Stoklu Ürün
                                            </label>
                                        </div>
                                        <div class="checkbox-inline">
                                            <label class="label_check" for="chkDurum">
                                                <asp:CheckBox ID="chkDurum" runat="server" />
                                                <span class="checkmark"></span>
                                                Aktif
                                            </label>
                                        </div>
                                        <div class="checkbox-inline">
                                            <label class="label_check" for="chkMamul">
                                                <asp:CheckBox ID="chkMamul" runat="server" />
                                                <span class="checkmark"></span>
                                                Mamul
                                            </label>
                                        </div>
                                        <div class="checkbox-inline">
                                            <label class="label_check" for="chkYariMamul">
                                                <asp:CheckBox ID="chkYariMamul" runat="server" />
                                                <span class="checkmark"></span>
                                                Yarı Mamul
                                            </label>
                                        </div>
                                        <div class="checkbox-inline">
                                            <label class="label_check" for="chkPerakendeSatisVarmi">
                                                <asp:CheckBox ID="chkPerakendeSatisVarmi" runat="server" />
                                                <span class="checkmark"></span>
                                                Perakende Satış Var
                                            </label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

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
                                            <asp:TextBox ID="txtAlisKdv" runat="server" CssClass="form-control" TextMode="Number" placeholder="0" />
                                        </div>
                                    </div>

                                    <div class="col-sm-4">
                                        <div class="checkboxes">
                                            <label class="label_check" for="chkAlisFiyatiKdvDahilmi">
                                                <asp:CheckBox ID="chkAlisFiyatiKdvDahilmi" runat="server" />
                                                <span class="checkmark"></span>
                                                Alış Fiyatı KDV Dahil
                                           
                                            </label>
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
                                            <asp:TextBox ID="txtSatisKdv" runat="server" CssClass="form-control" TextMode="Number" placeholder="0" />
                                        </div>
                                    </div>
                                    
                                    <div class="col-sm-2">
                                        <div class="checkboxes">
                                            <label class="label_check" for="chkSatisFiyatiKdvDahilmi">
                                                <asp:CheckBox ID="chkSatisFiyatiKdvDahilmi" runat="server" />
                                                <span class="checkmark"></span>
                                                Satış Fiyatı KDV Dahil
                                           
                                            </label>
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
                                            <asp:TextBox ID="txtKDVOrani" runat="server" CssClass="form-control" TextMode="Number" placeholder="0" />
                                        </div>
                                    </div>
                                    <div class="col-sm-2">
                                        <div class="checkboxes">
                                            <label class="label_check" for="chkPerakendeSatisKdvDahilmi">
                                                <asp:CheckBox ID="chkPerakendeSatisKdvDahilmi" runat="server" />
                                                <span class="checkmark"></span>
                                                Perakende Fiyatı KDV Dahil
                                           
                                            </label>
                                        </div>
                                    </div>
                                    

                                </div>
                                
                                
              
                            </div></div>
                        </div>

                        <div class="tab-pane fade" id="detaylar">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-trademark"></i>Marka</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-trademark"></i></span>
                                            <asp:DropDownList ID="ddlMarkaID" runat="server" CssClass="form-control"></asp:DropDownList>
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
                                            <itemtemplate>
                                                <div class="col-md-3" style="margin-bottom:10px;">
                                                    <div class="thumbnail">
                                                        <asp:Image ID="imgUrunResim" runat="server" ImageUrl='<%# Eval("ImageUrl") %>' CssClass="img-responsive" />
                                </div>
                            </div>
                                            </itemtemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="tab-pane fade" id="varyant">
                            <div class="form-horizontal">
                                <asp:UpdatePanel ID="upVaryant" runat="server" UpdateMode="Conditional">
                                    <contenttemplate>
                                        <div class="form-group">
                                            <div class="col-sm-12">
                                                <div class="checkbox">
                                                    <label>
                                                        <asp:CheckBox ID="chkVaryantKullan" runat="server" AutoPostBack="true" OnCheckedChanged="chkVaryantKullan_CheckedChanged" Text="Bu ürün için varyant kullan" />
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:Panel ID="pnlVaryantlar" runat="server" Visible="false">
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label"><i class="fa fa-list"></i> Varyant Türü</label>
                                                <div class="col-sm-10">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="fa fa-list"></i></span>
                                                        <asp:DropDownList ID="ddlVaryantTuru" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlVaryantTuru_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label"><i class="fa fa-sitemap"></i> Varyant Seçimi</label>
                                                <div class="col-sm-10">
                                                    <select multiple="multiple" class="multi-select" id="varyantSecici" name="varyantSecici[]">
                                                    </select>
                                                    <span class="help-block">Birden fazla varyant seçmek için CTRL tuşuna basılı tutarak seçim yapabilirsiniz.</span>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-2 control-label"><i class="fa fa-image"></i> Varyant Resmi</label>
                                                <div class="col-sm-8">
                                                    <div class="fileupload fileupload-new" data-provides="fileupload">
                                                        <div class="fileupload-new thumbnail" style="width: 200px; height: 150px;">
                                                            <img src="/App_Themes/serdarnas_admin_flat/img/no-image.png" alt="Resim yok" />
                                                        </div>
                                                        <div class="fileupload-preview fileupload-exists thumbnail" style="max-width: 200px; max-height: 150px; line-height: 20px;"></div>
                                                        <div>
                                                            <span class="btn btn-white btn-file">
                                                                <span class="fileupload-new"><i class="fa fa-paper-clip"></i> Resim Seç</span>
                                                                <span class="fileupload-exists"><i class="fa fa-undo"></i> Değiştir</span>
                                                                <asp:FileUpload ID="fuVaryantResim" runat="server" class="default" />
                                                            </span>
                                                            <a href="#" class="btn btn-danger fileupload-exists" data-dismiss="fileupload"><i class="fa fa-trash"></i> Sil</a>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-2">
                                                    <asp:Button ID="btnVaryantEkle" runat="server" Text="Ekle" CssClass="btn btn-success btn-block" OnClick="btnVaryantEkle_Click" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-sm-12">
                                                    <asp:GridView ID="gvVaryantlar" runat="server" CssClass="table table-striped table-bordered table-hover" AutoGenerateColumns="false"
                                                        OnRowCommand="gvVaryantlar_RowCommand" OnRowEditing="gvVaryantlar_RowEditing" OnRowDeleting="gvVaryantlar_RowDeleting"
                                                        OnRowUpdating="gvVaryantlar_RowUpdating" OnRowCancelingEdit="gvVaryantlar_RowCancelingEdit">
                                                        <Columns>
                                                            <asp:BoundField DataField="VaryantID" HeaderText="ID" ReadOnly="true" />
                                                            <asp:BoundField DataField="VaryantTuru" HeaderText="Varyant Türü" />
                                                            <asp:BoundField DataField="VaryantDegeri" HeaderText="Varyant Değeri" />
                                                            <asp:TemplateField HeaderText="Resim">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgVaryant" runat="server" ImageUrl='<%# Eval("ResimYolu") %>' Width="50" Height="50" CssClass="img-thumbnail" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="İşlemler">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-danger btn-xs" ToolTip="Sil"
                                                                        OnClientClick="return confirm('Bu varyantı silmek istediğinizden emin misiniz?');">
                                                                        <i class="fa fa-trash"></i>
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </contenttemplate>
                                    <triggers>
                                        <asp:AsyncPostBackTrigger ControlID="chkVaryantKullan" EventName="CheckedChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="ddlVaryantTuru" EventName="SelectedIndexChanged" />
                                        <asp:PostBackTrigger ControlID="btnVaryantEkle" />
                                    </triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>

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
                                <div class="form-section-title"><i class="fa fa-dropbox"></i> Koli Bilgileri</div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Koli İçi Ürün Adedi</label>
                                    <div class="col-sm-4">
                                        <asp:TextBox ID="txtKoliIciAdet" runat="server" CssClass="form-control" TextMode="Number" placeholder="Koli İçi Adet" />
                                    </div>
                                    <label class="col-sm-2 control-label">Koli Boyutları (cm)</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtKoliUzunluk" runat="server" CssClass="form-control" TextMode="Number" placeholder="U" style="min-width:70px;max-width:110px;" />
                                            <span class="input-group-addon">x</span>
                                            <asp:TextBox ID="txtKoliGenislik" runat="server" CssClass="form-control" TextMode="Number" placeholder="G" style="min-width:70px;max-width:110px;" />
                                            <span class="input-group-addon">x</span>
                                            <asp:TextBox ID="txtKoliYukseklik" runat="server" CssClass="form-control" TextMode="Number" placeholder="Y" style="min-width:70px;max-width:110px;" />
                                             
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
                                <div class="form-section-title"><i class="fa fa-cubes"></i> Palet Bilgileri</div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Palet Tipi</label>
                                    <div class="col-sm-4">
                                        <asp:DropDownList ID="ddlPaletTipi" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Euro Palet (80x120cm)" Value="euro_80x120" Selected="True" />
                                            <asp:ListItem Text="Standart Palet (100x120cm)" Value="standart_100x120" />
                                            <asp:ListItem Text="Özel Boyut" Value="custom" />
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-6" id="customPaletBoyutlari" style="display:none;">
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

                    <!-- Butonlar: Tüm tabların dışında, panelin en altında -->
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

    <!-- JavaScript -->
    <script src="/App_Themes/serdarnas_admin_flat/js/jquery.js"></script>
    <script src="/App_Themes/serdarnas_admin_flat/js/bootstrap.min.js"></script>

    <!-- Multi-select için gerekli CSS ve JS dosyaları -->
    <link rel="stylesheet" type="text/css" href="/App_Themes/serdarnas_admin_flat/assets/jquery-multi-select/css/multi-select.css" />
    <script type="text/javascript" src="/App_Themes/serdarnas_admin_flat/assets/jquery-multi-select/js/jquery.multi-select.js"></script>
    <script type="text/javascript" src="/App_Themes/serdarnas_admin_flat/assets/jquery-multi-select/js/jquery.quicksearch.js"></script>

    <!-- Other plugins -->
    <script src="/App_Themes/serdarnas_admin_flat/assets/bootstrap-colorpicker/js/bootstrap-colorpicker.js"></script>
    <script src="/App_Themes/serdarnas_admin_flat/assets/bootstrap-fileupload/bootstrap-fileupload.js"></script>
    <script src="/App_Themes/serdarnas_admin_flat/js/jquery.tagsinput.js"></script>

    <script type="text/javascript">
        var multiSelectInitialized = false;
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        function initializeMultiSelect() {
            if (!$('#varyantSecici').length) {
                console.log('MultiSelect element not found');
                return;
            }

            try {
                if (multiSelectInitialized) {
                    $('#varyantSecici').multiSelect('destroy');
                    console.log('Previous MultiSelect instance destroyed');
                }

                $('#varyantSecici').multiSelect({
                    selectableHeader: "<input type='text' class='form-control search-input' autocomplete='off' placeholder='Varyant Ara...'>",
                    selectionHeader: "<input type='text' class='form-control search-input' autocomplete='off' placeholder='Seçili Varyantlarda Ara...'>",
                    afterInit: function (ms) {
                        var that = this,
                            $selectableSearch = that.$selectableUl.prev(),
                            $selectionSearch = that.$selectionUl.prev(),
                            selectableSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selectable:not(.ms-selected)',
                            selectionSearchString = '#' + that.$container.attr('id') + ' .ms-elem-selection.ms-selected';

                        that.qs1 = $selectableSearch.quicksearch(selectableSearchString)
                            .on('keydown', function (e) {
                                if (e.which === 40) {
                                    that.$selectableUl.focus();
                                    return false;
                                }
                            });

                        that.qs2 = $selectionSearch.quicksearch(selectionSearchString)
                            .on('keydown', function (e) {
                                if (e.which == 40) {
                                    that.$selectionUl.focus();
                                    return false;
                                }
                            });
                    },
                    afterSelect: function () {
                        this.qs1.cache();
                        this.qs2.cache();
                    },
                    afterDeselect: function () {
                        this.qs1.cache();
                        this.qs2.cache();
                    }
                });
                multiSelectInitialized = true;
                console.log('MultiSelect initialized successfully');
            } catch (e) {
                console.error('Error initializing multiSelect:', e);
            }
        }

        function setupEventHandlers() {
            $('#myTab a').on('shown.bs.tab', function (e) {
                var activeTab = $(e.target).attr('href').substr(1);
                $('#<%= hfActiveTab.ClientID %>').val(activeTab);
                // ViewState'e de yaz (sunucuya taşımak için)
                if (typeof __doPostBack !== 'undefined') {
                    // Sadece postback öncesi güncel olsun diye
                }
                console.log('Tab changed to:', activeTab);
            });

            $('#<%= ddlVaryantTuru.ClientID %>').on('change', function () {
                var activeTab = $('#<%= hfActiveTab.ClientID %>').val();
                console.log('Saving active tab before dropdown change:', activeTab);
                var varyantTurID = $(this).val();

                if (varyantTurID != "0") {
                    $.ajax({
                        type: "POST",
                        url: "YeniUrun.aspx/GetVaryantDegerleri",
                        data: '{varyantTurID: ' + varyantTurID + '}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            $('#varyantSecici').empty();
                            $.each(response.d, function (index, item) {
                                $('#varyantSecici').append($('<option></option>').val(item.VaryantDegerID).text(item.DegerAdi));
                            });
                            initializeMultiSelect(); // Re-initialize after populating options
                        },
                        error: function (xhr, status, error) {
                            console.error("Varyant değerleri yüklenirken hata oluştu:", error);
                        }
                    });
                } else {
                    $('#varyantSecici').empty();
                    initializeMultiSelect(); // Re-initialize after clearing options
                }
            });

            // Form submit edilmeden önce aktif tab bilgisini ViewState'e yaz
            $('form').on('submit', function () {
                var activeTab = $('#<%= hfActiveTab.ClientID %>').val();
                // Sunucuya taşınacak
                // (ASP.NET zaten HiddenField'ı postback ile gönderir)
                console.log('Form submit, activeTab:', activeTab);
            });
        }

        function restoreActiveTab() {
            var activeTab = $('#<%= hfActiveTab.ClientID %>').val();
            if (activeTab) {
                $('#myTab a[href="#' + activeTab + '"]').tab('show');
            }
        }

        // Initial setup
        $(document).ready(function () {
            setupEventHandlers();
            initializeMultiSelect();
            restoreActiveTab();

            // chkVaryantKullan tıklanınca aktif tabı 'varyant' olarak ayarla
            $('#<%= chkVaryantKullan.ClientID %>').on('click', function () {
                setActiveTabBeforePostback('varyant');
            });
        });

        // Handle partial postbacks
        prm.add_endRequest(function () {
            setupEventHandlers();
            initializeMultiSelect();
            restoreActiveTab();
        });

        // Varyant kullan checkbox'ı tıklanınca aktif tabı 'varyant' olarak ayarla
        function setActiveTabBeforePostback(tabName) {
            $('#<%= hfActiveTab.ClientID %>').val(tabName);
        }
    </script>

    <!-- Custom Checkbox & Radio script -->
    <script type="text/javascript">
        $(document).ready(function () {
            var d = document;
            var safari = (navigator.userAgent.toLowerCase().indexOf('safari') != -1) ? true : false;
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
        });
    </script>

    <style>
        .form-section-title {
            font-size: 1.2rem;
            font-weight: 600;
            color: #343a40;
            margin-top: 1.5rem;
            margin-bottom: 0.75rem;
            border-bottom: 2px solid #6f42c1;
            padding-bottom: 0.25rem;
        }

            .form-section-title i {
                margin-right: 0.5rem;
            }

        .calculated-value {
            font-size: 1.1rem;
            font-weight: 600;
            color: #0d6efd;
            padding-top: 0.5rem;
        }
    </style>
    <script type="text/javascript">
        // Varsayılan ürün net ağırlığı (örnek: 500ml yağ için 0.46kg)
        var urunNetAgirligiKg = 0.46;

        function calculateKoliHacmi() {
            var u = parseFloat($('#<%= txtKoliUzunluk.ClientID %>').val()) / 100;
            var g = parseFloat($('#<%= txtKoliGenislik.ClientID %>').val()) / 100;
            var y = parseFloat($('#<%= txtKoliYukseklik.ClientID %>').val()) / 100;
            if (!isNaN(u) && !isNaN(g) && !isNaN(y)) {
                $('#koliHacmiHesaplanan').text((u * g * y).toFixed(4) + " m³");
            } else {
                $('#koliHacmiHesaplanan').text("-");
            }
        }

        // Koli Net Ağırlığı (Kg) = Koli içi adet * Ürün Net Ağırlık (gr) / 1000
        function calculateKoliNetAgirlik() {
            var koliAdet = parseInt($('#<%= txtKoliIciAdet.ClientID %>').val());
            var urunNetAgirlik = parseFloat($('#<%= txtUrunNetAgirlik_gr.ClientID %>').val());
            if (!isNaN(koliAdet) && !isNaN(urunNetAgirlik)) {
                var netAgirlikKg = (koliAdet * urunNetAgirlik) / 1000;
                $('#<%= txtKoliNetAgirlik.ClientID %>').val(netAgirlikKg.toFixed(2));
            } else {
                $('#<%= txtKoliNetAgirlik.ClientID %>').val('');
            }
        }

        // Koli Brüt Ağırlığı (Kg) = Koli içi adet * Ürün Brüt Ağırlık (gr) / 1000
        function calculateKoliBrutAgirlik() {
            var koliAdet = parseInt($('#<%= txtKoliIciAdet.ClientID %>').val());
            var urunBrutAgirlik = parseFloat($('#<%= txtUrunBurutAgirlik_gr.ClientID %>').val());
            if (!isNaN(koliAdet) && !isNaN(urunBrutAgirlik)) {
                var brutAgirlikKg = (koliAdet * urunBrutAgirlik) / 1000;
                $('#<%= txtKoliBrutAgirlik.ClientID %>').val(brutAgirlikKg.toFixed(2));
            }
            // Elle girilmişse, elle girilen değeri değiştirme!
        }

        function calculatePalettekiUrunAdedi() {
            var koliAdet = parseInt($('#<%= txtKoliIciAdet.ClientID %>').val());
            var paletKoli = parseInt($('#<%= txtPalettekiKoliSayisi.ClientID %>').val());
            if (!isNaN(koliAdet) && !isNaN(paletKoli)) {
                $('#palettekiUrunAdediHesaplanan').text((koliAdet * paletKoli) + " Adet");
            } else {
                $('#palettekiUrunAdediHesaplanan').text("-");
            }
        }

        function calculatePaletBrutAgirlik() {
            var koliAgirlik = parseFloat($('#<%= txtKoliBrutAgirlik.ClientID %>').val());
            var paletKoli = parseInt($('#<%= txtPalettekiKoliSayisi.ClientID %>').val());
            var paletDara = parseFloat($('#<%= txtPaletDaraAgirligi.ClientID %>').val()) || 0;
            if (!isNaN(koliAgirlik) && !isNaN(paletKoli)) {
                $('#<%= txtPaletBrutAgirlik.ClientID %>').val(((koliAgirlik * paletKoli) + paletDara).toFixed(2));
            } else {
                $('#<%= txtPaletBrutAgirlik.ClientID %>').val('');
            }
        }

        // Palet tipi özel seçilirse özel boyut alanını göster
        function handlePaletTipiChange() {
            var val = $('#<%= ddlPaletTipi.ClientID %>').val();
            if (val === 'custom') {
                $('#customPaletBoyutlari').show();
            } else {
                $('#customPaletBoyutlari').hide();
            }
        }

        $(document).ready(function () {
            // Hesaplamalar
            $('#<%= txtKoliUzunluk.ClientID %>, #<%= txtKoliGenislik.ClientID %>, #<%= txtKoliYukseklik.ClientID %>').on('input', calculateKoliHacmi);
            $('#<%= txtKoliIciAdet.ClientID %>, #<%= txtUrunNetAgirlik_gr.ClientID %>').on('input', calculateKoliNetAgirlik);
            $('#<%= txtKoliIciAdet.ClientID %>, #<%= txtUrunBurutAgirlik_gr.ClientID %>').on('input', calculateKoliBrutAgirlik);
            $('#<%= txtKoliIciAdet.ClientID %>, #<%= txtPalettekiKoliSayisi.ClientID %>').on('input', calculatePalettekiUrunAdedi);
            $('#<%= txtKoliBrutAgirlik.ClientID %>, #<%= txtPalettekiKoliSayisi.ClientID %>, #<%= txtPaletDaraAgirligi.ClientID %>').on('input', calculatePaletBrutAgirlik);

            // Palet tipi değişimi
            $('#<%= ddlPaletTipi.ClientID %>').on('change', handlePaletTipiChange);
            handlePaletTipiChange(); // Sayfa yüklenince kontrol et
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

    <script>
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            if ($('#modalYeniKategori').length > 0 && $('#txtYeniKategoriAdi').val() === '') {
                $('#modalYeniKategori').modal('hide');
            }
        });
    </script>
</asp:content>

