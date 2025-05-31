<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="YeniTedarikci.aspx.cs" Inherits="fabrika_Tedarikciler_YeniTedarikci" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
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
                        <li><a href="#finansal" data-toggle="tab"><i class="fa fa-bank"></i> Finansal Bilgiler</a></li>
                    </ul>
                </header>
                <div class="panel-body">
                    <!-- Tablar -->
                    <div class="tab-content">
                        <!-- Temel Bilgiler -->
                        <div class="tab-pane fade in active" id="temel">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-barcode"></i> Tedarikçi Kodu</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-barcode"></i></span>
                                            <asp:TextBox ID="txtTedarikciKodu" runat="server" CssClass="form-control" placeholder="Tedarikçi Kodu" />
                                        </div>
                                    </div>
                                    <label class="col-sm-2 control-label"><i class="fa fa-building"></i> Firma Adı <span class="text-danger">*</span></label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-building"></i></span>
                                            <asp:TextBox ID="txtFirmaAdi" runat="server" CssClass="form-control" placeholder="Firma Adı" />
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvFirmaAdi" runat="server" ControlToValidate="txtFirmaAdi" ErrorMessage="Firma Adı gereklidir" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-user"></i> Yetkili Adı</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-user"></i></span>
                                            <asp:TextBox ID="txtYetkiliAdi" runat="server" CssClass="form-control" placeholder="Yetkili Adı" />
                                        </div>
                                    </div>
                                    <label class="col-sm-2 control-label"><i class="fa fa-mobile"></i> Cep Telefonu</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-mobile"></i></span>
                                            <asp:TextBox ID="txtCepTelefonu" runat="server" CssClass="form-control" placeholder="Cep Telefonu" />
                                        </div>
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
                                    <label class="col-sm-2 control-label"><i class="fa fa-id-card"></i> Vergi Dairesi</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-id-card"></i></span>
                                            <asp:TextBox ID="txtVergiDairesi" runat="server" CssClass="form-control" placeholder="Vergi Dairesi" />
                                        </div>
                                    </div>
                                    <label class="col-sm-2 control-label"><i class="fa fa-id-card"></i> Vergi No</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-id-card"></i></span>
                                            <asp:TextBox ID="txtVergiNo" runat="server" CssClass="form-control" placeholder="Vergi No" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-money"></i> Bakiyesi</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-money"></i></span>
                                            <asp:TextBox ID="txtBakiyesi" runat="server" CssClass="form-control" placeholder="0,00" />
                                        </div>
                                    </div>
                                    <label class="col-sm-2 control-label"><i class="fa fa-money"></i> Para Birimi</label>
                                    <div class="col-sm-4">
                                        <asp:DropDownList ID="ddlParaBirimi" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="TL" Value="1" />
                                            <asp:ListItem Text="USD" Value="2" />
                                            <asp:ListItem Text="EUR" Value="3" />
                                        </asp:DropDownList>
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
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-check-square-o"></i> Aktif</label>
                                    <div class="col-sm-4">
                                        <asp:CheckBox ID="cbAktif" runat="server" CssClass="checkbox" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Butonlar -->
                    <div class="form-group mt-4">
                        <div class="col-sm-12 text-right">
                            <a href="Default.aspx" class="btn btn-outline-secondary"><i class="fa fa-times"></i> İptal</a>
                            <asp:Button ID="btnKaydet" runat="server" CssClass="btn btn-success" Text="Tedarikçiyi Kaydet" />
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>

    <!-- JavaScript -->
    <script src="/App_Themes/serdarnas_admin_flat/js/jquery.js"></script>
    <script src="/App_Themes/serdarnas_admin_flat/js/bootstrap.min.js"></script>
    <script src="/App_Themes/serdarnas_admin_flat/js/form-component.js"></script>
</asp:Content>

