<%@ Page Title="Zeytinyağı Makineleri" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="ZeytinyagiMakinalari.aspx.cs" Inherits="fabrika_Zeytinyagi_ZeytinyagiAyarlar_ZeytinyagiMakinalari" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <!-- jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    
    <!-- SweetAlert2 for beautiful alerts -->
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    
    <style type="text/css">
        .panel-heading h3 {
            margin: 0;
            padding: 0;
        }
        .btn-group {
            margin-bottom: 15px;
        }
        .form-group {
            margin-bottom: 15px;
        }
        .well {
            background-color: #f5f5f5;
            padding: 15px;
            border-radius: 4px;
            margin-bottom: 20px;
        }
        .datepicker {
            z-index: 1151 !important;
        }
        .label {
            padding: 3px 6px;
            font-size: 12px;
            font-weight: bold;
            border-radius: 3px;
        }
        .label-success {
            background-color: #5cb85c;
        }
        .label-danger {
            background-color: #d9534f;
        }
    </style>
    
    <script type="text/javascript">
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

        // Sayfa yüklendiğinde çalışacak kodlar
        $(document).ready(function () {
            // Tarih seçiciyi aktif et
            $('.datepicker').datepicker({
                format: 'yyyy-mm-dd',
                autoclose: true,
                language: 'tr',
                todayHighlight: true
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <h3>Zeytinyağı Makineleri Yönetimi</h3>
                </header>
                <div class="panel-body">
                    <!-- Hızlı İşlem Butonları -->
                    <div class="btn-group">
                        <asp:LinkButton ID="btnYeniMakina" runat="server" CssClass="btn btn-primary" OnClick="btnYeniMakina_Click">
                            <i class="icon-plus"></i> Yeni Makine Ekle
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnKaydet" runat="server" CssClass="btn btn-success" OnClick="btnKaydet_Click" ValidationGroup="MakineKayit">
                            <i class="icon-save"></i> Kaydet
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnIptal" runat="server" CssClass="btn btn-default" OnClick="btnIptal_Click">
                            <i class="icon-remove"></i> İptal
                        </asp:LinkButton>
                    </div>
                    
                    <!-- Makine Bilgileri Formu -->
                    <asp:Panel ID="pnlMakineForm" runat="server" Visible="false" CssClass="well">
                        <h4>Makine Bilgileri</h4>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Marka</label>
                                    <asp:DropDownList ID="ddlMarka" runat="server" CssClass="form-control" AutoPostBack="true" 
                                        OnSelectedIndexChanged="ddlMarka_SelectedIndexChanged" required>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label>Model</label>
                                    <asp:DropDownList ID="ddlModel" runat="server" CssClass="form-control" required>
                                        <asp:ListItem Text="-- Önce marka seçiniz --" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvModel" runat="server" ControlToValidate="ddlModel"
                                        InitialValue="0" ErrorMessage="Lütfen bir model seçiniz" Display="Dynamic"
                                        CssClass="text-danger" ValidationGroup="MakineKayit"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group">
                                    <label>Alım Tarihi</label>
                                    <asp:TextBox ID="txtAlimTarihi" runat="server" CssClass="form-control datepicker" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Yıkama Yaprak Ayıklama (kg/dk)</label>
                                    <asp:TextBox ID="txtYikamaHizi" runat="server" CssClass="form-control" TextMode="Number" step="0.01"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Kırma Hızı (kg/dk)</label>
                                    <asp:TextBox ID="txtKirmaHizi" runat="server" CssClass="form-control" TextMode="Number" step="0.01"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Malaksasyon Hızı (kg/dk)</label>
                                    <asp:TextBox ID="txtMalaksasyonHizi" runat="server" CssClass="form-control" TextMode="Number" step="0.01"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Dekantasyon/Santrifüj Hızı (kg/dk)</label>
                                    <asp:TextBox ID="txtDekantasyonHizi" runat="server" CssClass="form-control" TextMode="Number" step="0.01"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        
                        <!-- Malaksörler GridView -->
                        <h4>Malaksörler</h4>
                        <asp:GridView ID="gvMalaksorler" runat="server" CssClass="table table-striped table-bordered" 
                            AutoGenerateColumns="False" OnRowCommand="gvMalaksorler_RowCommand" DataKeyNames="SirketZeytinyagiMakinaMalaksorID">
                            <Columns>
                                <asp:BoundField DataField="MalaksorSiraNo" HeaderText="Sıra No" />
                                <asp:BoundField DataField="MalaksorKaparistesi_kg" HeaderText="Kapasite (kg)" DataFormatString="{0:N2}" />
                                <asp:TemplateField HeaderText="Durum">
                                    <ItemTemplate>
                                        <span class='<%# Eval("Durum").ToString() == "Aktif" ? "label label-success" : "label label-danger" %>'>
                                            <%# Eval("Durum") %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkSil" runat="server" CssClass="btn btn-danger btn-xs" 
                                            CommandName="Sil" CommandArgument='<%# Eval("SirketZeytinyagiMakinaMalaksorID") %>'
                                            OnClientClick="return confirm('Bu malaksörü silmek istediğinize emin misiniz?');">
                                            <i class="icon-trash"></i> Sil
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        
                        <div class="row" style="margin-top: 15px;">
                            <div class="col-md-3">
                                <div class="input-group">
                                    <asp:TextBox ID="txtYeniMalaksorKapasite" runat="server" CssClass="form-control" 
                                        placeholder="Kapasite (kg)" TextMode="Number" step="0.01"></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="btnMalaksorEkle" runat="server" CssClass="btn btn-primary" 
                                            OnClick="btnMalaksorEkle_Click">
                                            <i class="icon-plus"></i> Ekle
                                        </asp:LinkButton>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    
                    <!-- Makine Listesi -->
                    <asp:GridView ID="gvMakineler" runat="server" CssClass="table table-striped table-bordered" 
                        AutoGenerateColumns="False" OnRowCommand="gvMakineler_RowCommand" DataKeyNames="SirketZeytinyagiMakinaID">
                        <Columns>
                            <asp:BoundField DataField="Marka" HeaderText="Marka" />
                            <asp:BoundField DataField="Model" HeaderText="Model" />
                            <asp:BoundField DataField="AlimTarihi" HeaderText="Alım Tarihi" DataFormatString="{0:d}" />
                            <asp:TemplateField HeaderText="Durum">
                                <ItemTemplate>
                                    <span class='<%# Eval("Durumu").ToString().ToLower() == "true" ? "label label-success" : "label label-danger" %>'>
                                        <%# Eval("Durumu").ToString().ToLower() == "true" ? "Aktif" : "Pasif" %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDuzenle" runat="server" CssClass="btn btn-primary btn-xs" 
                                        CommandName="Duzenle" CommandArgument='<%# Eval("SirketZeytinyagiMakinaID") %>'>
                                        <i class="icon-pencil"></i> Düzenle
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lnkSil" runat="server" CssClass="btn btn-danger btn-xs" 
                                        CommandName="Sil" CommandArgument='<%# Eval("SirketZeytinyagiMakinaID") %>'
                                        OnClientClick="return confirm('Bu makineyi silmek istediğinize emin misiniz?');">
                                        <i class="icon-trash"></i> Sil
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="alert alert-info">Henüz kayıtlı makine bulunmamaktadır.</div>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </section>
        </div>
    </div>
    
    <!-- Hidden Fields -->
    <asp:HiddenField ID="hdnMakinaID" runat="server" Value="0" />
</asp:Content>
