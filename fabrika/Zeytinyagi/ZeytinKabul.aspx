<%@ Page Title="Zeytin Kabul İşlemleri" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="ZeytinKabul.aspx.cs" Inherits="fabrika_Zeytinyagi_ZeytinKabul" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!-- jQuery -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    
    <!-- SweetAlert2 for beautiful alerts -->
    <link href="https://cdn.jsdelivr.net/npm/sweetalert2@11/dist/sweetalert2.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    
    <style type="text/css">
        .panel {
            background: #fff;
            box-shadow: 0 2px 5px 0 rgba(0,0,0,0.16), 0 2px 10px 0 rgba(0,0,0,0.12);
            border-radius: 4px;
            margin-bottom: 20px;
        }
        .panel-heading {
            background: #4CAF50;
            color: white;
            padding: 15px;
            border-radius: 4px 4px 0 0;
        }
        .panel-heading h3 {
            margin: 0;
            font-size: 1.2rem;
            font-weight: 500;
        }
        .panel-body {
            padding: 20px;
        }
        .btn-group {
            margin-bottom: 15px;
        }
        .btn {
            padding: 8px 16px;
            font-weight: 500;
            transition: all 0.2s;
            border-radius: 4px;
        }
        .btn i {
            margin-right: 5px;
        }
        .btn-shadow {
            box-shadow: 0 2px 5px rgba(0,0,0,0.16);
        }
        .table {
            margin-bottom: 0;
        }
        .table th {
            background: #f8f9fa;
            font-weight: 700;
            text-transform: uppercase;
            font-size: 0.85rem;
            border-bottom: 2px solid #dee2e6;
            color: #495057;
            letter-spacing: 0.5px;
        }
        .table td {
            vertical-align: middle;
            font-size: 0.85rem;
            color: #6c757d;
        }
        .btn-xs {
            padding: 0.2rem 0.5rem;
            font-size: 0.75rem;
            line-height: 1.4;
            border-radius: 0.2rem;
        }
        .btn-xs i {
            font-size: 0.75rem;
            margin-right: 3px;
        }
        .table td .btn {
            margin: 0 2px;
            min-width: 60px;
            text-align: center;
        }
        .badge {
            padding: 6px 10px;
            font-weight: 500;
            border-radius: 4px;
        }
        .bg-warning {
            background-color: #ffc107 !important;
            color: #000;
        }
        .bg-success {
            background-color: #28a745 !important;
        }
        .bg-primary {
            background-color: #007bff !important;
        }
        .input-group {
            margin-bottom: 1rem;
        }
        .input-group-text {
            background: #f8f9fa;
            border-right: none;
        }
        .form-control:focus {
            border-color: #4CAF50;
            box-shadow: 0 0 0 0.2rem rgba(76, 175, 80, 0.25);
        }
        .alert {
            border-radius: 4px;
            margin-bottom: 1rem;
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
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <!-- Hata ve Başarı Mesajları -->
    <asp:Panel ID="pnlHata" runat="server" CssClass="alert alert-danger" Visible="false">
        <asp:Literal ID="ltlHata" runat="server"></asp:Literal>
    </asp:Panel>

    <asp:Panel ID="pnlBasari" runat="server" CssClass="alert alert-success" Visible="false">
        <asp:Literal ID="ltlBasari" runat="server"></asp:Literal>
    </asp:Panel>

    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <h3><i class="fa fa-leaf me-2"></i>Zeytin Kabul İşlemleri</h3>
                </header>
                <div class="panel-body">
                    <div class="btn-group">
                        <asp:HyperLink ID="lnkYeniKayit" runat="server" CssClass="btn btn-shadow btn-success" NavigateUrl="~/fabrika/Zeytinyagi/ZeytinKabulYeni.aspx">
                            <i class="fa fa-plus"></i>Yeni Kayıt Ekle
                        </asp:HyperLink>
                        <asp:HyperLink ID="lnkYeniMustahsil" runat="server" CssClass="btn btn-shadow btn-primary ms-2" NavigateUrl="~/fabrika/Mustahsil/YeniMustahsil.aspx">
                            <i class="fa fa-user-plus"></i>Yeni Müstahsil
                        </asp:HyperLink>
                        <asp:HyperLink ID="HyperLink1" runat="server" CssClass="btn btn-shadow btn-danger ms-2" NavigateUrl="~/fabrika/Zeytinyagi/Default.aspx">
                            <i class="fa fa-arrow-left"></i>İşletme Paneli
                        </asp:HyperLink>
                    </div>
                    <div class="input-group mb-3">
                        <span class="input-group-text"><i class="fa fa-search"></i></span>
                        <asp:TextBox ID="txtArama" runat="server" CssClass="form-control" 
                            placeholder="Parti No, Müstahsil, Plaka No veya Box No ile arama yapın..." 
                            OnTextChanged="txtArama_TextChanged" AutoPostBack="true"></asp:TextBox>
                    </div>
                    
                    <div class="table-responsive">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvZeytinler" runat="server" AutoGenerateColumns="false" 
                                    CssClass="table table-striped table-hover" 
                                    OnRowCommand="gvZeytinler_RowCommand"
                                    DataKeyNames="ZeytinyagiUretimID" 
                                    AllowPaging="true" 
                                    PageSize="10" 
                                    OnPageIndexChanging="gvZeytinler_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="PartiNo" HeaderText="Parti No" />
                                        <asp:BoundField DataField="MustahsilAdi" HeaderText="Müstahsil" />
                                        <asp:BoundField DataField="PlakaNo" HeaderText="Plaka No" />
                                        <asp:BoundField DataField="ZeytinBoxNo" HeaderText="Box No" />
                                        <asp:BoundField DataField="GelisKg" HeaderText="Miktar (Kg)" DataFormatString="{0:N2}" />
                                        <asp:BoundField DataField="GelisTarihi" HeaderText="Geliş Tarihi" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                                        <asp:BoundField DataField="islem_Ad" HeaderText="İşlem Türü" />
                                        <asp:BoundField DataField="UrunAdi" HeaderText="Ürün" />
                                        <asp:TemplateField HeaderText="Durum">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDurum" runat="server" CssClass='<%# GetStatusBadgeClass(Eval("Durum").ToString()) %>'>
                                                    <%# GetStatusText(Eval("Durum").ToString()) %>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="İşlemler" ItemStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnDetay" runat="server" CssClass="btn btn-info btn-xs" CommandName="Detay" CommandArgument='<%# Eval("ZeytinyagiUretimID") %>' ToolTip="Detayları Gör">
                                                    <i class="fa fa-eye"></i> Detay
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnDuzenle" runat="server" CssClass='<%# Eval("Durum").ToString() == "Beklemede" ? "btn btn-primary btn-xs ms-1" : "btn btn-secondary btn-xs ms-1 disabled" %>' 
                                                    CommandName="Duzenle" CommandArgument='<%# Eval("ZeytinyagiUretimID") %>' ToolTip="Düzenle">
                                                    <i class="fa fa-pencil"></i> Düzenle
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnIslemBaslat" runat="server" CssClass='<%# Eval("Durum").ToString() == "Beklemede" ? "btn btn-success btn-xs ms-1" : "btn btn-secondary btn-xs ms-1 disabled" %>' 
                                                    CommandName="IslemBaslat" CommandArgument='<%# Eval("ZeytinyagiUretimID") %>' ToolTip="İşleme Başlat">
                                                    <i class="fa fa-play"></i> Başlat
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pagination pagination-sm justify-content-center mt-3" />
                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="İlk" LastPageText="Son" />
                                    <EmptyDataTemplate>
                                        <div class="alert alert-info">Kayıt bulunamadı.</div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>