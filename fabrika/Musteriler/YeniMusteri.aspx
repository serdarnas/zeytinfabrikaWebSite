<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="YeniMusteri.aspx.cs" Inherits="fabrika_Musteriler_YeniMusteri" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <!-- Modern CSS -->
    <link href="/App_Themes/serdarnas_admin_flat/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/App_Themes/serdarnas_admin_flat/assets/font-awesome/css/font-awesome.css" rel="stylesheet" />
    <link href="/App_Themes/serdarnas_admin_flat/css/style.css" rel="stylesheet" />
    <link href="/App_Themes/serdarnas_admin_flat/css/style-responsive.css" rel="stylesheet" />
    


    <div class="row">
        <div class="col-md-10 col-md-offset-1">
            <section class="panel">
                <header class="panel-heading tab-bg-dark-navy-blue"> 
                    <ul class="nav nav-tabs" id="myTab">
                        <li class="active"><a href="#temel" data-toggle="tab"><i class="fa fa-id-card"></i> Temel Bilgiler</a></li>
                        <li><a href="#iletisim" data-toggle="tab"><i class="fa fa-phone"></i> İletişim Bilgileri</a></li>
                        <li><a href="#finansal" data-toggle="tab"><i class="fa fa-money"></i> Finansal Bilgiler</a></li>
                        <li><a href="#sube" data-toggle="tab"><i class="fa fa-building"></i> Şube Bilgileri</a></li>
                    </ul>
                </header>
                <div class="panel-body">
                    <!-- Hata ve Başarı Mesajları -->
                    <asp:Panel ID="pnlHata" runat="server" CssClass="alert alert-danger" Visible="false">
                        <asp:Label ID="lblHata" runat="server" Text=""></asp:Label>
                    </asp:Panel>
                    <asp:Panel ID="pnlBasari" runat="server" CssClass="alert alert-success" Visible="false">
                        <asp:Label ID="lblBasari" runat="server" Text=""></asp:Label>
                    </asp:Panel>

                    <!-- Tablar -->
                    <div class="tab-content">
                        <!-- Temel Bilgiler -->
                        <div class="tab-pane fade in active" id="temel">
                            <div class="form-horizontal">
                               
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-building"></i> Müşteri Adı/ Unvani <span class="text-danger">*</span></label>
                                    <div class="col-sm-10">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-building"></i></span>
                                            <asp:TextBox ID="txtMusteriAdi" runat="server" CssClass="form-control" placeholder="Müşteri adı" />
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvMusteriAdi" runat="server" ControlToValidate="txtMusteriAdi" 
                                            ErrorMessage="Müşteri adı gereklidir" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-6">
                                        <label class="col-sm-4 control-label"><i class="fa fa-user"></i> Yetkili/Temsilci Ad Soyad</label>
                                        <div class="col-sm-8">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-user"></i></span>
                                                <asp:TextBox ID="txtYetkiliAdi" runat="server" CssClass="form-control" placeholder="Yetkili adı" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label class="col-sm-4 control-label"><i class="fa fa-mobile"></i> Cep Telefonu</label>
                                        <div class="col-sm-8">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-mobile"></i></span>
                                                <asp:TextBox ID="txtCepTelefonu" runat="server" CssClass="form-control" placeholder="Cep telefonu" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-list"></i> Kategori</label>
                                    <div class="col-sm-10">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-list"></i></span>
                                            <asp:DropDownList ID="ddlKategori" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Kategori seçiniz" Value="" />
                                            </asp:DropDownList>
                                            <span class="input-group-btn">
                                                <button type="button" class="btn btn-success" data-toggle="modal" data-target="#kategoriModal">
                                                    <i class="fa fa-plus"></i> Yeni Kategori
                                                </button>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-image"></i> Resim</label>
                                    <div class="col-sm-10">
                                        <div class="input-group">
                                            <asp:Image runat="server" ID="imgMusteri" CssClass="img-thumbnail" Style="max-width: 100px; margin-right: 10px;" Visible="false" />
                                            <span class="input-group-addon"><i class="fa fa-image"></i></span>
                                            <asp:FileUpload ID="fuResim" runat="server" CssClass="form-control" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-check-square-o"></i> Aktif Müşteri</label>
                                    <div class="col-sm-10">
                                        <asp:CheckBox ID="cbAktif" runat="server" CssClass="checkbox" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- İletişim Bilgileri -->
                        <div class="tab-pane fade" id="iletisim">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-phone"></i> Telefon</label>
                                    <div class="col-sm-10">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-phone"></i></span>
                                            <asp:TextBox ID="txtTelefon" runat="server" CssClass="form-control" placeholder="Telefon numarası" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-envelope"></i> E-posta</label>
                                    <div class="col-sm-10">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-envelope"></i></span>
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="E-posta adresi" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-map-marker"></i> Adres</label>
                                    <div class="col-sm-10">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-map-marker"></i></span>
                                            <asp:TextBox ID="txtAdres" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Adres bilgileri" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-map-marker"></i> Notlar</label>
                                    <div class="col-sm-10">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-map-marker"></i></span>
                                            <asp:TextBox ID="txtNot" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Adres bilgileri" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Finansal Bilgiler -->
                        <div class="tab-pane fade" id="finansal">
                            <div class="form-horizontal">
                           
                                <div class="form-group">
                                    <div class="col-sm-6">
                                        <label class="col-sm-4 control-label"><i class="fa fa-user"></i> Vergi Dairesi</label>
                                        <div class="col-sm-8">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-user"></i></span>
                                                <asp:TextBox ID="txtVergiDairesi" runat="server" CssClass="form-control" placeholder="Vergi dairesi" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label class="col-sm-4 control-label"><i class="fa fa-mobile"></i> Vergi Numarası</label>
                                        <div class="col-sm-8">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-mobile"></i></span>
                                                <asp:TextBox ID="txtVergiNo" runat="server" CssClass="form-control" placeholder="Vergi numarası" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-sm-6">
                                        <label class="col-sm-4 control-label"><i class="fa fa-user"></i> Sabit iskonto</label>
                                        <div class="col-sm-8">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-user"></i></span>
                                                <asp:TextBox ID="txtSabitİskonto" runat="server" CssClass="form-control" placeholder="0" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label class="col-sm-4 control-label"><i class="fa fa-mobile"></i> Bakiyesi</label>
                                        <div class="col-sm-8">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-mobile"></i></span>
                                                <asp:TextBox ID="txtBakiyesi" runat="server" CssClass="form-control" placeholder="0" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                
                                <div class="form-group">
                                    <div class="col-sm-6">
                                        <label class="col-sm-4 control-label"><i class="fa fa-user"></i> Vade Gün olarak</label>
                                        <div class="col-sm-8">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-user"></i></span>
                                                <asp:TextBox ID="txtVade" runat="server" CssClass="form-control" placeholder="0" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <label class="col-sm-4 control-label"><i class="fa fa-mobile"></i>  Para Birimi</label>
                                        <div class="col-sm-8">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="fa fa-money"></i></span>
                                                <asp:DropDownList ID="ddlParaBirimi" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="TL" Value="TL" />
                                                    <asp:ListItem Text="USD" Value="USD" />
                                                    <asp:ListItem Text="EUR" Value="EUR" />
                                                </asp:DropDownList>
                                        </div>
                                    </div>
                                    </div> 

                                </div>
                        
                                
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-barcode"></i> Müşteri Kodu</label>
                                    <div class="col-sm-10">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-barcode"></i></span>
                                            <asp:TextBox ID="txtMusteriKodu" runat="server" CssClass="form-control" placeholder="Müşteri kodu" />
                                        </div>
                                    </div>
                                </div>
                                
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-map-marker"></i> Banka Bilgileri</label>
                                    <div class="col-sm-10">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-map-marker"></i></span>
                                            <asp:TextBox ID="txtBankaBilgileri" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Banka Bilgileri" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Şube Bilgileri -->
                        <div class="tab-pane fade" id="sube">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-building"></i> Şube Adı</label>
                                    <div class="col-sm-10">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-building"></i></span>
                                            <asp:TextBox ID="txtSubaAd" runat="server" CssClass="form-control" placeholder="Şube adı" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-map-marker"></i> Şube Adresi</label>
                                    <div class="col-sm-10">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-map-marker"></i></span>
                                            <asp:TextBox ID="txtSubeAdres" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Şube adresi" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Butonlar -->
                    <div class="form-group mt-4">
                        <div class="col-sm-12 text-right">
                            <a href="Default.aspx" class="btn btn-outline-secondary"><i class="fa fa-times"></i> İptal</a>
                            <asp:Button ID="btnKaydet" runat="server" CssClass="btn btn-success" Text="Müşteriyi Kaydet" />
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>

    <!-- Kategori Ekleme Modal -->
    <div class="modal fade" id="kategoriModal" tabindex="-1" role="dialog" aria-labelledby="kategoriModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="kategoriModalLabel"><i class="fa fa-plus-circle"></i> Yeni Kategori Ekle</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="txtYeniKategoriAdi">Kategori Adı</label>
                        <asp:TextBox ID="txtYeniKategoriAdi" runat="server" CssClass="form-control" placeholder="Kategori adını giriniz" />
                    </div>
                    <div class="form-group">
                        <label for="txtYeniKategoriAciklama">Açıklama</label>
                        <asp:TextBox ID="txtYeniKategoriAciklama" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Kategori açıklaması" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">İptal</button>
                    <asp:Button ID="btnKategoriKaydet" runat="server" Text="Kaydet" CssClass="btn btn-success" OnClick="btnKategoriKaydet_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- JavaScript -->
    <script src="/App_Themes/serdarnas_admin_flat/js/jquery.js"></script>
    <script src="/App_Themes/serdarnas_admin_flat/js/bootstrap.min.js"></script>
    <script src="/App_Themes/serdarnas_admin_flat/js/form-component.js"></script>
    
    <script type="text/javascript">
        function kategoriEklendi() {
            $('#kategoriModal').modal('hide');
            // Sayfayı yenile veya dropdown'ı güncelle
            location.reload();
        }
    </script>
</asp:content>
