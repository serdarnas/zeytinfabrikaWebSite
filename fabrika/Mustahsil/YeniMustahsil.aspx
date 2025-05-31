<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="YeniMustahsil.aspx.cs" Inherits="fabrika_Mustahsil_YeniMustahsil" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <!-- jQuery - sayfa başında yüklenmesi gerekiyor -->
    <script src="/App_Themes/serdarnas_admin_flat/js/jquery.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                    </ul>
                </header>
                <div class="panel-body">
                    <!-- Hata ve Başarı Mesajları -->
                

                    <!-- Tablar -->
                    <div class="tab-content">
                        <!-- Temel Bilgiler -->
                        <div class="tab-pane fade in active" id="temel">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-user"></i> Ad <span class="text-danger">*</span></label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-user"></i></span>
                                            <asp:TextBox ID="txtAd" runat="server" CssClass="form-control" placeholder="Adı" />
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvAd" runat="server" ControlToValidate="txtAd" ErrorMessage="Ad gereklidir" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                    <label class="col-sm-2 control-label"><i class="fa fa-user"></i> Soyad <span class="text-danger">*</span></label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-user"></i></span>
                                            <asp:TextBox ID="txtSoyad" runat="server" CssClass="form-control" placeholder="Soyadı" />
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvSoyad" runat="server" ControlToValidate="txtSoyad" ErrorMessage="Soyad gereklidir" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-id-card"></i> TC Kimlik No</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-id-card"></i></span>
                                            <asp:TextBox ID="txtTCKimlikNo" runat="server" CssClass="form-control" placeholder="TC Kimlik No (11 haneli)" MaxLength="11" AutoPostBack="True" OnTextChanged="txtTCKimlikNo_TextChanged" />
                                        </div>
                                        <small class="text-muted">TC Kimlik No benzersiz olmalıdır. Daha önce kaydedilmiş kişi otomatik olarak bulunacaktır.</small>
                                    </div>
                                    <label class="col-sm-2 control-label"><i class="fa fa-check-square-o"></i> Aktif</label>
                                    <div class="col-sm-4">
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
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-phone"></i></span>
                                            <asp:TextBox ID="txtTelefon" runat="server" CssClass="form-control" placeholder="Telefon numarası" />
                                        </div>
                                    </div>
                                    <label class="col-sm-2 control-label"><i class="fa fa-envelope"></i> E-posta</label>
                                    <div class="col-sm-4">
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
                                    <label class="col-sm-2 control-label"><i class="fa fa-sticky-note"></i> Notlar</label>
                                    <div class="col-sm-10">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-sticky-note"></i></span>
                                            <asp:TextBox ID="txtNotlar" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Notlar" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Finansal Bilgiler -->
                        <div class="tab-pane fade" id="finansal">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-money"></i> Bakiyesi</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-money"></i></span>
                                            <asp:TextBox ID="txtBakiyesi" runat="server" CssClass="form-control" placeholder="0,00" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-bank"></i> Banka Bilgileri</label>
                                    <div class="col-sm-10">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-bank"></i></span>
                                            <asp:TextBox ID="txtBankaBilgileri" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Banka Bilgileri" />
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
                            <asp:Button ID="btnKaydet" runat="server" CssClass="btn btn-success" Text="Müstahsili Kaydet" OnClick="btnKaydet_Click" />
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>

    <!-- JavaScript -->
    <script src="/App_Themes/serdarnas_admin_flat/js/bootstrap.min.js"></script>
    <script src="/App_Themes/serdarnas_admin_flat/js/form-component.js"></script>
    
    <script type="text/javascript">
        // TC Kimlik No için sadece rakam girişine izin ver
        $(document).ready(function() {
            $('#<%= txtTCKimlikNo.ClientID %>').on('input', function() {
                this.value = this.value.replace(/[^0-9]/g, '');
            });
        });
    </script>
</asp:Content>

