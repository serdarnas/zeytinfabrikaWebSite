<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="KasaHareketleri.aspx.cs" Inherits="fabrika_Nakit_KasaHareketleri" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    
    <style>
        .hareket-card {
            border: 1px solid #ddd;
            border-radius: 8px;
            padding: 15px;
            margin-bottom: 15px;
            background: #fff;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        .hareket-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 10px;
            padding-bottom: 10px;
            border-bottom: 1px solid #eee;
        }
        .hareket-tutar {
            font-size: 1.2em;
            font-weight: bold;
        }
        .hareket-tutar.giris {
            color: #27ae60;
        }
        .hareket-tutar.cikis {
            color: #e74c3c;
        }
        .hareket-tipi {
            padding: 4px 8px;
            border-radius: 4px;
            font-size: 0.8em;
            color: white;
        }
        .hareket-tipi.giris {
            background-color: #27ae60;
        }
        .hareket-tipi.cikis {
            background-color: #e74c3c;
        }
        .kasa-ozet {
            background: #f8f9fa;
            border-radius: 8px;
            padding: 20px;
            margin-bottom: 20px;
            border-left: 4px solid #007bff;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <h3><i class="fa fa-list"></i> Kasa Hareketleri</h3>
                    <div class="pull-right">
                        <a href="Kasalar.aspx" class="btn btn-default btn-sm">
                            <i class="fa fa-arrow-left"></i> Kasalar Listesi
                        </a>
                    </div>
                </header>
                <div class="panel-body">
                    
                    <!-- Kasa Özet Bilgileri -->
                    <div class="kasa-ozet">
                        <div class="row">
                            <div class="col-md-3">
                                <h4><asp:Label ID="lblKasaAdi" runat="server" Text=""></asp:Label></h4>
                                <small class="text-muted">Kasa Kodu: <asp:Label ID="lblKasaKodu" runat="server" Text=""></asp:Label></small>
                            </div>
                            <div class="col-md-3">
                                <h5>Mevcut Bakiye</h5>
                                <h3 class="text-primary"><asp:Label ID="lblMevcutBakiye" runat="server" Text="0,00"></asp:Label> <asp:Label ID="lblParaBirimi" runat="server" Text="TL"></asp:Label></h3>
                            </div>
                            <div class="col-md-3">
                                <h5>Toplam Giriş</h5>
                                <h4 class="text-success"><asp:Label ID="lblToplamGiris" runat="server" Text="0,00"></asp:Label> <asp:Label ID="lblParaBirimi2" runat="server" Text="TL"></asp:Label></h4>
                            </div>
                            <div class="col-md-3">
                                <h5>Toplam Çıkış</h5>
                                <h4 class="text-danger"><asp:Label ID="lblToplamCikis" runat="server" Text="0,00"></asp:Label> <asp:Label ID="lblParaBirimi3" runat="server" Text="TL"></asp:Label></h4>
                            </div>
                        </div>
                    </div>

                    <!-- Filtreler -->
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
                                <label>Hareket Tipi:</label>
                                <asp:DropDownList ID="ddlHareketTipi" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="Tümü"></asp:ListItem>
                                    <asp:ListItem Value="G" Text="Giriş"></asp:ListItem>
                                    <asp:ListItem Value="C" Text="Çıkış"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label>&nbsp;</label>
                                <br />
                                <asp:Button ID="btnFiltrele" runat="server" Text="Filtrele" CssClass="btn btn-primary" OnClick="btnFiltrele_Click" />
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label>&nbsp;</label>
                                <br />
                                <asp:Button ID="btnTemizle" runat="server" Text="Temizle" CssClass="btn btn-default" OnClick="btnTemizle_Click" />
                            </div>
                        </div>
                    </div>

                    <!-- Hareketler Listesi -->
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Repeater ID="rptKasaHareketleri" runat="server">
                                <ItemTemplate>
                                    <div class="hareket-card">
                                        <div class="hareket-header">
                                            <div>
                                                <span class="hareket-tipi <%# Eval("IslemTipi").ToString() == "G" ? "giris" : "cikis" %>">
                                                    <%# Eval("IslemTipi").ToString() == "G" ? "GİRİŞ" : "ÇIKIŞ" %>
                                                </span>
                                                <strong style="margin-left: 10px;"><%# Eval("ReferansTipi") %></strong>
                                            </div>
                                            <div>
                                                <span class="hareket-tutar <%# Eval("IslemTipi").ToString() == "G" ? "giris" : "cikis" %>">
                                                    <%# Eval("IslemTipi").ToString() == "G" ? "+" : "-" %><%# String.Format("{0:N2}", Eval("Tutar")) %> TL
                                                </span>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <small class="text-muted">
                                                    <i class="fa fa-calendar"></i> <%# String.Format("{0:dd.MM.yyyy HH:mm}", Eval("IslemTarihi")) %>
                                                </small>
                                                <%# !String.IsNullOrEmpty(Eval("ReferansID").ToString()) && Eval("ReferansID").ToString() != "0" ? "<br /><small class=\"text-muted\"><i class=\"fa fa-link\"></i> Ref: " + Eval("ReferansID") + "</small>" : "" %>
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
                                <h4 class="text-muted">Kayıtlı hareket bulunmuyor</h4>
                                <p class="text-muted">Seçili tarih aralığında kasa hareketi bulunmuyor.</p>
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
