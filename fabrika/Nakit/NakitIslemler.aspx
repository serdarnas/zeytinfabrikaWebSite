<%@ Page Title="Nakit İşlemleri" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="NakitIslemler.aspx.cs" Inherits="fabrika_Nakit_NakitIslemler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    
    <style>
        .payment-card {
            border: none;
            border-radius: 12px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
            margin-bottom: 1rem;
            transition: transform 0.2s ease;
        }
        .payment-card:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(0,0,0,0.15);
        }
        .payment-amount {
            font-size: 1.25rem;
            font-weight: 600;
        }
        .payment-in {
            color: #28a745;
        }
        .payment-out {
            color: #dc3545;
        }
        .payment-type-badge {
            font-size: 0.75rem;
            padding: 0.25rem 0.5rem;
        }
        .filter-section {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            border-radius: 12px;
            padding: 1.5rem;
            margin-bottom: 1.5rem;
            color: white;
        }
        .btn-filter {
            background: rgba(255,255,255,0.2);
            border: 1px solid rgba(255,255,255,0.3);
            color: white;
            border-radius: 8px;
        }
        .btn-filter:hover {
            background: rgba(255,255,255,0.3);
            color: white;
        }
        .form-control, .form-select {
            border-radius: 8px;
            border: 1px solid rgba(255,255,255,0.3);
            background: rgba(255,255,255,0.1);
            color: white;
        }
        .form-control::placeholder {
            color: rgba(255,255,255,0.7);
        }
        .form-control:focus, .form-select:focus {
            background: rgba(255,255,255,0.2);
            border-color: rgba(255,255,255,0.5);
            color: white;
            box-shadow: 0 0 0 0.2rem rgba(255,255,255,0.25);
        }
        .stats-card {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            border: none;
            border-radius: 12px;
            color: white;
        }
        @media (max-width: 768px) {
            .payment-card {
                margin-bottom: 0.75rem;
            }
            .filter-section {
                padding: 1rem;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <h3><i class="fa fa-money"></i> Nakit İşlemleri</h3>
                </header>
                <div class="panel-body">
                    
                    <!-- Filtreler -->
                    <div class="filter-box">
                        <div class="row">
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label>Başlangıç Tarihi:</label>
                                    <asp:TextBox ID="txtBaslangicTarihi" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label>Bitiş Tarihi:</label>
                                    <asp:TextBox ID="txtBitisTarihi" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <label>İşlem Türü:</label>
                                    <asp:DropDownList ID="ddlIslemTuruFiltre" runat="server" CssClass="form-control">
                                        <asp:ListItem Value="" Text="Tümü"></asp:ListItem>
                                        <asp:ListItem Value="T" Text="Tahsilat"></asp:ListItem>
                                        <asp:ListItem Value="O" Text="Ödeme"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <label>Ödeme Tipi:</label>
                                    <asp:DropDownList ID="ddlOdemeTipiFiltre" runat="server" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="form-group">
                                    <label>&nbsp;</label>
                                    <br />
                                    <asp:Button ID="btnFiltrele" runat="server" Text="Filtrele" CssClass="btn btn-primary" OnClick="btnFiltrele_Click" />
                                    <asp:Button ID="btnTemizle" runat="server" Text="Temizle" CssClass="btn btn-default" OnClick="btnTemizle_Click" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Özet İstatistikler -->
                    <div class="row">
                        <div class="col-md-3">
                            <div class="panel panel-success">
                                <div class="panel-body text-center">
                                    <h4>Toplam Tahsilat</h4>
                                    <h3><asp:Label ID="lblToplamTahsilat" runat="server" Text="0,00"></asp:Label> TL</h3>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="panel panel-danger">
                                <div class="panel-body text-center">
                                    <h4>Toplam Ödeme</h4>
                                    <h3><asp:Label ID="lblToplamOdeme" runat="server" Text="0,00"></asp:Label> TL</h3>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="panel panel-info">
                                <div class="panel-body text-center">
                                    <h4>Net Akış</h4>
                                    <h3><asp:Label ID="lblNetAkis" runat="server" Text="0,00"></asp:Label> TL</h3>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="panel panel-warning">
                                <div class="panel-body text-center">
                                    <h4>İşlem Sayısı</h4>
                                    <h3><asp:Label ID="lblIslemSayisi" runat="server" Text="0"></asp:Label></h3>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- İşlemler Listesi -->
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Repeater ID="rptNakitIslemler" runat="server">
                                <ItemTemplate>
                                    <div class="islem-card">
                                        <div class="islem-header">
                                            <div>
                                                <span class="islem-tipi <%# Eval("IslemTuru").ToString() == "T" ? "tahsilat" : "odeme" %>">
                                                    <%# Eval("IslemTuru").ToString() == "T" ? "TAHSİLAT" : "ÖDEME" %>
                                                </span>
                                                <strong style="margin-left: 10px;"><%# Eval("IlgiliTaraf") %></strong>
                                            </div>
                                            <div>
                                                <span class="islem-tutar <%# Eval("IslemTuru").ToString() == "T" ? "tahsilat" : "odeme" %>">
                                                    <%# String.Format("{0:N2}", Eval("Tutar")) %> <%# Eval("ParaBirimi") %>
                                                </span>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <small class="text-muted">
                                                    <i class="fa fa-calendar"></i> <%# String.Format("{0:dd.MM.yyyy HH:mm}", Eval("IslemTarihi")) %>
                                                </small>
                                                <br />
                                                <small class="text-muted">
                                                    <i class="fa fa-credit-card"></i> <%# Eval("OdemeTipi") %>
                                                </small>
                                            </div>
                                            <div class="col-md-6">
                                                <%# !String.IsNullOrEmpty(Eval("Aciklama").ToString()) ? "<small class=\"text-muted\"><strong>Açıklama:</strong> " + Eval("Aciklama") + "</small>" : "" %>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            
                            <asp:Panel ID="pnlVeriYok" runat="server" Visible="false" CssClass="text-center" style="padding: 50px;">
                                <i class="fa fa-info-circle fa-3x text-muted"></i>
                                <h4 class="text-muted">Kayıtlı nakit işlemi bulunmuyor</h4>
                                <p class="text-muted">Filtreleri değiştirerek tekrar deneyiniz.</p>
                            </asp:Panel>
                        </div>
                    </div>

                </div>
            </section>
        </div>
    </div>

    <script type="text/javascript">
        function showSuccessMessage(title, message) {
            Swal.fire({
                icon: 'success',
                title: title,
                text: message,
                confirmButtonText: 'Tamam'
            });
        }

        function showErrorMessage(title, message) {
            Swal.fire({
                icon: 'error',
                title: title,
                text: message,
                confirmButtonText: 'Tamam'
            });
        }

        function showWarningMessage(title, message) {
            Swal.fire({
                icon: 'warning',
                title: title,
                text: message,
                confirmButtonText: 'Tamam'
            });
        }

        function showInfoMessage(title, message) {
            Swal.fire({
                icon: 'info',
                title: title,
                text: message,
                confirmButtonText: 'Tamam'
            });
        }
    </script>
</asp:Content>
