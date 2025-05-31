<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="YeniKullanici.aspx.cs" Inherits="fabrika_Kullanici_YeniKullanici" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-md-10 col-md-offset-1">
            <section class="panel">
                <header class="panel-heading tab-bg-dark-navy-blue">
                    <ul class="nav nav-tabs" id="myTab">
                        <li class="active"><a href="#temel" data-toggle="tab"><i class="fa fa-user"></i> Temel Bilgiler</a></li>
                        <li><a href="#menuyetki" data-toggle="tab"><i class="fa fa-list"></i> Menü Yetkileri</a></li>
                        <li><a href="#yetkilendirme" data-toggle="tab"><i class="fa fa-shield"></i> Yetkilendirme</a></li>
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
                                    <label class="col-sm-2 control-label"><i class="fa fa-user"></i> Ad Soyad <span class="text-danger">*</span></label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-user"></i></span>
                                            <asp:TextBox ID="txtAdSoyad" runat="server" CssClass="form-control" placeholder="Ad Soyad" />
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvAdSoyad" runat="server" ControlToValidate="txtAdSoyad" ErrorMessage="Ad Soyad gereklidir" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                    <label class="col-sm-2 control-label"><i class="fa fa-lock"></i> Şifre <span class="text-danger">*</span></label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-lock"></i></span>
                                            <asp:TextBox ID="txtSifre" runat="server" CssClass="form-control" placeholder="Şifre" TextMode="Password" />
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvSifre" runat="server" ControlToValidate="txtSifre" ErrorMessage="Şifre gereklidir" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-envelope"></i> E-posta <span class="text-danger">*</span></label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-envelope"></i></span>
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="E-posta adresi" />
                                        </div>
                                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="E-posta gereklidir" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Geçerli bir e-posta giriniz" ForeColor="Red" Display="Dynamic" ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w{2,4}$"></asp:RegularExpressionValidator>
                                    </div>
                                    <label class="col-sm-2 control-label"><i class="fa fa-phone"></i> Telefon</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-phone"></i></span>
                                            <asp:TextBox ID="txtTelefon" runat="server" CssClass="form-control" placeholder="Telefon numarası" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-users"></i> Yetki</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-users"></i></span>
                                            <asp:DropDownList ID="ddlYetki" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Seçiniz" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Yönetici" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Kullanıcı" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <label class="col-sm-2 control-label"><i class="fa fa-check-square-o"></i> Aktif</label>
                                    <div class="col-sm-4">
                                        <asp:CheckBox ID="cbAktif" runat="server" CssClass="checkbox" Checked="true" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label"><i class="fa fa-calendar"></i> Oluşturma Tarihi</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-calendar"></i></span>
                                            <asp:TextBox ID="txtOlusturmaTarihi" runat="server" CssClass="form-control" ReadOnly="true" />
                                        </div>
                                    </div>
                                    <label class="col-sm-2 control-label"><i class="fa fa-sign-in"></i> Son Giriş Tarihi</label>
                                    <div class="col-sm-4">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="fa fa-sign-in"></i></span>
                                            <asp:TextBox ID="txtSonGirisTarihi" runat="server" CssClass="form-control" ReadOnly="true" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Menü Yetkileri -->
                        <div class="tab-pane fade" id="menuyetki">
                            <div class="form-horizontal">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <i class="fa fa-list"></i> Menü Yetkileri
                                        </h4>
                                    </div>
                                    <div class="panel-body">
                                        <style type="text/css">
                                            .menu-tree-container {
                                                margin: 10px 0;
                                            }
                                            .menu-category {
                                                font-weight: bold;
                                                margin: 15px 0 5px 0;
                                                padding: 5px;
                                                background-color: #f8f8f8;
                                                border-bottom: 1px solid #ddd;
                                                font-size: 16px;
                                            }
                                            .menu-category .category-checkbox {
                                                margin-right: 8px;
                                                transform: scale(1.3);
                                                vertical-align: middle;
                                            }
                                            .menu-items {
                                                display: flex;
                                                flex-wrap: wrap;
                                                margin-left: 15px;
                                            }
                                            .menu-item {
                                                width: 25%;
                                                padding: 6px 0;
                                            }
                                            .menu-item label {
                                                font-weight: normal;
                                                margin-bottom: 0;
                                                cursor: pointer;
                                                font-size: 14px;
                                            }
                                            .menu-item input[type="checkbox"] {
                                                transform: scale(1.2);
                                                margin-right: 5px;
                                            }
                                        </style>
                                        <div class="menu-tree-container">
                                            <asp:PlaceHolder ID="phMenuItems" runat="server"></asp:PlaceHolder>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Yetkilendirme -->
                        <div class="tab-pane fade" id="yetkilendirme">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Kullanıcının işlem yapabileceği hesaplar</label>
                                    <div class="col-sm-10">
                                        <asp:ListBox ID="lbHesaplar" runat="server" SelectionMode="Multiple" CssClass="form-control" Rows="4"></asp:ListBox>
                                        <span class="help-block">Bu kullanıcının tüm hesaplarınızı görmesini istemiyorsanız bu listeden istemediğiniz hesapları kaldırabilirsiniz.</span>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Kullanıcının işlem yapabileceği depolar</label>
                                    <div class="col-sm-10">
                                        <asp:ListBox ID="lbDepolar" runat="server" SelectionMode="Multiple" CssClass="form-control" Rows="4"></asp:ListBox>
                                        <span class="help-block">Alış ve satış ekranlarında bu kullanıcının işlem yapabileceği depoları seçin.</span>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Satışların görülmesi</label>
                                    <div class="col-sm-10">
                                        <asp:DropDownList ID="ddlSatisGorme" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Tüm kullanıcıların satışlarını görebilir" Value="tum" />
                                            <asp:ListItem Text="Sadece kendi satışlarını görebilir" Value="kendi" />
                                        </asp:DropDownList>
                                        <span class="help-block">Web ve mobil uygulamada bu kullanıcının diğer kullanıcılar tarafından yapılan satışları görüp göremeyeceğini seçebilirsiniz.</span>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Müşterilerin görülmesi</label>
                                    <div class="col-sm-10">
                                        <asp:DropDownList ID="ddlMusteriGorme" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Tüm müşterileri görebilir" Value="tum" />
                                            <asp:ListItem Text="Sadece kendi müşterilerini görebilir" Value="kendi" />
                                        </asp:DropDownList>
                                        <span class="help-block">Web ve mobil uygulamada bu kullanıcının hangi müşterileri görebileceğini seçebilirsiniz.</span>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-sm-2 control-label">Masrafların görülmesi</label>
                                    <div class="col-sm-10">
                                        <asp:DropDownList ID="ddlMasrafGorme" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Tüm masrafları görebilir" Value="tum" />
                                            <asp:ListItem Text="Sadece kendi masraflarını görebilir" Value="kendi" />
                                        </asp:DropDownList>
                                        <span class="help-block">"Masraflar" ekranında bu kullanıcının diğer kullanıcılar tarafından girilen masrafları görüp göremeyeceğini seçebilirsiniz.</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Butonlar -->
                    <div class="form-group mt-4">
                        <div class="col-sm-12 text-right">
                            <a href="Default.aspx" class="btn btn-outline-secondary"><i class="fa fa-times"></i> İptal</a>
                            <asp:Button ID="btnKaydet" runat="server" CssClass="btn btn-success" Text="Kullanıcıyı Kaydet" OnClick="btnKaydet_Click" />
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>

</asp:Content>
<asp:Content runat="server"  ID="scripts" ContentPlaceHolderID="scripts">

    <!-- JavaScript -->
    <script src="/App_Themes/serdarnas_admin_flat/js/jquery.js"></script>
    <script src="/App_Themes/serdarnas_admin_flat/js/bootstrap.min.js"></script>
    <script src="/App_Themes/serdarnas_admin_flat/js/form-component.js"></script>
</asp:Content>

