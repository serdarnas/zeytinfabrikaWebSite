<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Kasalar.aspx.cs" Inherits="fabrika_Nakit_Kasalar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <!-- SweetAlert2 -->
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    
    <style>
        .kasa-card {
            border: 1px solid #ddd;
            border-radius: 8px;
            padding: 15px;
            margin-bottom: 20px;
            background: #fff;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        .kasa-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 10px;
        }
        .kasa-bakiye {
            font-size: 1.5em;
            font-weight: bold;
            color: #27ae60;
        }
        .kasa-tipi {
            padding: 4px 8px;
            border-radius: 4px;
            font-size: 0.8em;
            color: white;
        }
        .kasa-tipi.fiziksel {
            background-color: #3498db;
        }
        .kasa-tipi.dijital {
            background-color: #9b59b6;
        }
        .stats-box {
            background: #f8f9fa;
            border-radius: 8px;
            padding: 20px;
            margin-bottom: 20px;
        }
        .btn-group-custom {
            margin-bottom: 20px;
        }
        .hareket-row {
            border-bottom: 1px solid #eee;
            padding: 10px 0;
        }
        .hareket-row:last-child {
            border-bottom: none;
        }
        .modal-header {
            background-color: #f8f9fa;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <h3><i class="fa fa-university"></i> Kasa Yönetimi</h3>
                </header>
                <div class="panel-body">
                    
                    <!-- Genel İstatistikler -->
                    <div class="stats-box">
                        <div class="row">
                            <div class="col-md-3">
                                <div class="text-center">
                                    <h4>Toplam Kasa</h4>
                                    <h2 class="text-info"><asp:Label ID="lblToplamKasa" runat="server" Text="0"></asp:Label></h2>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="text-center">
                                    <h4>Nakit Toplam (TL)</h4>
                                    <h2 class="text-success"><asp:Label ID="lblNakitToplam" runat="server" Text="0,00"></asp:Label></h2>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="text-center">
                                    <h4>Banka Toplam (TL)</h4>
                                    <h2 class="text-primary"><asp:Label ID="lblBankaToplam" runat="server" Text="0,00"></asp:Label></h2>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="text-center">
                                    <h4>Genel Toplam (TL)</h4>
                                    <h2 class="text-warning"><asp:Label ID="lblGenelToplam" runat="server" Text="0,00"></asp:Label></h2>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Butonlar -->
                    <div class="btn-group-custom">
                        <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#yeniKasaModal">
                            <i class="fa fa-plus"></i> Yeni Kasa Ekle
                        </button>
                        <button type="button" class="btn btn-info" data-toggle="modal" data-target="#kasaTransferModal">
                            <i class="fa fa-exchange"></i> Kasa Transferi
                        </button>
                        <button type="button" class="btn btn-success" data-toggle="modal" data-target="#nakitIslemModal">
                            <i class="fa fa-money"></i> Nakit İşlem
                        </button>
                        <asp:LinkButton ID="btnRaporla" runat="server" CssClass="btn btn-warning" OnClick="btnRaporla_Click">
                            <i class="fa fa-file-excel-o"></i> Excel Raporu
                        </asp:LinkButton>
                    </div>

                    <!-- Kasalar Listesi -->
                    <div class="row">
                        <asp:Repeater ID="rptKasalar" runat="server" OnItemCommand="rptKasalar_ItemCommand">
                            <ItemTemplate>
                                <div class="col-md-6">
                                    <div class="kasa-card">
                                        <div class="kasa-header">
                                            <div>
                                                <h4><%# Eval("KasaAdi") %></h4>
                                                <small class="text-muted">Kod: <%# Eval("KasaKodu") %></small>
                                            </div>
                                            <span class="kasa-tipi <%# Eval("KasaTipi").ToString() == "F" ? "fiziksel" : "dijital" %>">
                                                <%# Eval("KasaTipi").ToString() == "F" ? "Fiziksel" : "Dijital" %>
                                            </span>
                                        </div>
                                        <div class="kasa-bakiye">
                                            <%# String.Format("{0:N2}", Eval("Bakiye")) %> <%# Eval("ParaBirimi") %>
                                        </div>
                                        <div style="margin-top: 10px;">
                                            <small class="text-muted"><%# Eval("Aciklama") %></small>
                                        </div>
                                        <div style="margin-top: 15px;">
                                            <asp:LinkButton ID="btnHareketler" runat="server" 
                                                CommandName="Hareketler" 
                                                CommandArgument='<%# Eval("KasaID") %>'
                                                CssClass="btn btn-xs btn-info">
                                                <i class="fa fa-list"></i> Hareketler
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnDuzenle" runat="server" 
                                                CommandName="Duzenle" 
                                                CommandArgument='<%# Eval("KasaID") %>'
                                                CssClass="btn btn-xs btn-warning">
                                                <i class="fa fa-edit"></i> Düzenle
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnSil" runat="server" 
                                                CommandName="Sil" 
                                                CommandArgument='<%# Eval("KasaID") %>'
                                                CssClass="btn btn-xs btn-danger"
                                                OnClientClick="return confirm('Bu kasayı silmek istediğinizden emin misiniz?');">
                                                <i class="fa fa-trash"></i> Sil
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                </div>
            </section>
        </div>
    </div>

    <!-- Yeni Kasa Modal -->
    <div class="modal fade" id="yeniKasaModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Yeni Kasa Ekle</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label>Kasa Kodu:</label>
                        <asp:TextBox ID="txtKasaKodu" runat="server" CssClass="form-control" MaxLength="20" placeholder="Örn: KASA001"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Kasa Adı:</label>
                        <asp:TextBox ID="txtKasaAdi" runat="server" CssClass="form-control" MaxLength="100" placeholder="Örn: Ana Kasa"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Kasa Tipi:</label>
                        <asp:DropDownList ID="ddlKasaTipi" runat="server" CssClass="form-control">
                            <asp:ListItem Value="F" Text="Fiziksel Kasa"></asp:ListItem>
                            <asp:ListItem Value="D" Text="Dijital Kasa"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>Para Birimi:</label>
                        <asp:DropDownList ID="ddlParaBirimi" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>Başlangıç Bakiyesi:</label>
                        <asp:TextBox ID="txtBaslangicBakiye" runat="server" CssClass="form-control" Text="0" placeholder="0,00"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Açıklama:</label>
                        <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" MaxLength="500"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">İptal</button>
                    <asp:Button ID="btnKasaKaydet" runat="server" Text="Kaydet" CssClass="btn btn-primary" OnClick="btnKasaKaydet_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Kasa Transfer Modal -->
    <div class="modal fade" id="kasaTransferModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Kasalar Arası Transfer</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label>Kaynak Kasa:</label>
                        <asp:DropDownList ID="ddlKaynakKasa" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>Hedef Kasa:</label>
                        <asp:DropDownList ID="ddlHedefKasa" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>Transfer Tutarı:</label>
                        <asp:TextBox ID="txtTransferTutar" runat="server" CssClass="form-control" placeholder="0,00"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Açıklama:</label>
                        <asp:TextBox ID="txtTransferAciklama" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">İptal</button>
                    <asp:Button ID="btnTransferYap" runat="server" Text="Transfer Yap" CssClass="btn btn-success" OnClick="btnTransferYap_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Nakit İşlem Modal -->
    <div class="modal fade" id="nakitIslemModal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Nakit İşlemi</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>İşlem Türü:</label>
                                <asp:DropDownList ID="ddlIslemTuru" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="T" Text="Tahsilat (Para Girişi)"></asp:ListItem>
                                    <asp:ListItem Value="O" Text="Ödeme (Para Çıkışı)"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Kasa:</label>
                                <asp:DropDownList ID="ddlIslemKasa" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>İlgili Taraf Tipi:</label>
                                <asp:DropDownList ID="ddlTarafTipi" runat="server" CssClass="form-control" onchange="TarafTipiDegisti();">
                                    <asp:ListItem Value="" Text="Seçiniz..."></asp:ListItem>
                                    <asp:ListItem Value="M" Text="Müşteri"></asp:ListItem>
                                    <asp:ListItem Value="T" Text="Tedarikçi"></asp:ListItem>
                                    <asp:ListItem Value="MU" Text="Müstahsil"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>İlgili Taraf:</label>
                                <asp:DropDownList ID="ddlIlgiliTaraf" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label>Tutar:</label>
                                <asp:TextBox ID="txtIslemTutar" runat="server" CssClass="form-control" placeholder="0,00"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label>Ödeme Tipi:</label>
                                <asp:DropDownList ID="ddlOdemeTipi" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="form-group">
                                <label>Referans No:</label>
                                <asp:TextBox ID="txtReferansNo" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label>Açıklama:</label>
                        <asp:TextBox ID="txtIslemAciklama" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">İptal</button>
                    <asp:Button ID="btnNakitIslemKaydet" runat="server" Text="Kaydet" CssClass="btn btn-primary" OnClick="btnNakitIslemKaydet_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- SweetAlert2 Fonksiyonları -->
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

        // Modal kapatma fonksiyonları
        function closeModal(modalId) {
            $('#' + modalId).modal('hide');
        }

        // Taraf tipi değiştiğinde çağrılır
        function TarafTipiDegisti() {
            var tarafTipi = document.getElementById('<%= ddlTarafTipi.ClientID %>').value;
            var ddlIlgiliTaraf = document.getElementById('<%= ddlIlgiliTaraf.ClientID %>');
            
            // Önce dropdown'ı temizle
            ddlIlgiliTaraf.innerHTML = '<option value="0">Seçiniz...</option>';
            
            if (tarafTipi === "") {
                return;
            }
            
            // AJAX ile verileri çek
            var xhr = new XMLHttpRequest();
            xhr.onreadystatechange = function() {
                if (xhr.readyState === 4 && xhr.status === 200) {
                    try {
                        var data = JSON.parse(xhr.responseText);
                        ddlIlgiliTaraf.innerHTML = '<option value="0">Seçiniz...</option>';
                        
                        for (var i = 0; i < data.length; i++) {
                            var option = document.createElement('option');
                            option.value = data[i].ID;
                            option.text = data[i].Ad;
                            ddlIlgiliTaraf.appendChild(option);
                        }
                    } catch (e) {
                        console.error('JSON parse hatası:', e);
                    }
                }
            };
            
            xhr.open('POST', 'Kasalar.aspx/GetIlgiliTaraflar', true);
            xhr.setRequestHeader('Content-Type', 'application/json; charset=utf-8');
            xhr.send(JSON.stringify({ tarafTipi: tarafTipi }));
        }
    </script>
</asp:Content>

