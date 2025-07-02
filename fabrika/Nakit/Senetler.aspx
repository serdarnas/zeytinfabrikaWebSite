<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Senetler.aspx.cs" Inherits="fabrika_Nakit_Senetler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    
    <style>
        .senet-card {
            border: 1px solid #ddd;
            border-radius: 8px;
            padding: 15px;
            margin-bottom: 15px;
            background: #fff;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        .senet-card.alinan {
            border-left: 4px solid #28a745;
        }
        .senet-card.verilen {
            border-left: 4px solid #dc3545;
        }
        .senet-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 10px;
            padding-bottom: 10px;
            border-bottom: 1px solid #eee;
        }
        .senet-tutar {
            font-size: 1.3em;
            font-weight: bold;
        }
        .senet-tutar.alinan {
            color: #28a745;
        }
        .senet-tutar.verilen {
            color: #dc3545;
        }
        .senet-tipi {
            padding: 4px 8px;
            border-radius: 4px;
            font-size: 0.8em;
            color: white;
        }
        .senet-tipi.alinan {
            background-color: #28a745;
        }
        .senet-tipi.verilen {
            background-color: #dc3545;
        }
        .senet-durum {
            padding: 4px 8px;
            border-radius: 4px;
            font-size: 0.8em;
            color: white;
            margin-left: 5px;
        }
        .senet-durum.portfoyde { background-color: #17a2b8; }
        .senet-durum.ciro { background-color: #6f42c1; }
        .senet-durum.tahsil { background-color: #20c997; }
        .senet-durum.faktoring { background-color: #fd7e14; }
        .senet-durum.tahsil-edildi { background-color: #6c757d; }
        .senet-durum.protesto { background-color: #e74c3c; }
        .senet-durum.tedarikci { background-color: #ffc107; color: #000; }
        .senet-durum.odendi { background-color: #28a745; }
        .stats-box {
            background: #f8f9fa;
            border-radius: 8px;
            padding: 20px;
            margin-bottom: 20px;
        }
        .tab-content {
            margin-top: 20px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <h3><i class="fa fa-file-o"></i> Senet Yönetimi</h3>
                    <div class="pull-right">
                        <a href="Default.aspx" class="btn btn-default btn-sm">
                            <i class="fa fa-arrow-left"></i> Ana Sayfa
                        </a>
                    </div>
                </header>
                <div class="panel-body">
                    
                    <!-- Senet İstatistikleri -->
                    <div class="stats-box">
                        <div class="row">
                            <div class="col-md-12">
                                <h4><i class="fa fa-bar-chart"></i> Senet Durumu</h4>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <div class="text-center">
                                    <h4>Alınan Senetler</h4>
                                    <h3 class="text-success"><asp:Label ID="lblAlinanSenet" runat="server" Text="0,00"></asp:Label> TL</h3>
                                    <small><asp:Label ID="lblAlinanAdet" runat="server" Text="0"></asp:Label> Adet</small>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="text-center">
                                    <h4>Verilen Senetler</h4>
                                    <h3 class="text-danger"><asp:Label ID="lblVerilenSenet" runat="server" Text="0,00"></asp:Label> TL</h3>
                                    <small><asp:Label ID="lblVerilenAdet" runat="server" Text="0"></asp:Label> Adet</small>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="text-center">
                                    <h4>Net Durum</h4>
                                    <h3 class="text-primary"><asp:Label ID="lblNetDurum" runat="server" Text="0,00"></asp:Label> TL</h3>
                                    <small>Alınan - Verilen</small>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="text-center">
                                    <h4>Toplam Senet</h4>
                                    <h3 class="text-info"><asp:Label ID="lblToplamSenet" runat="server" Text="0"></asp:Label></h3>
                                    <small>Tüm Senetler</small>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Sekmeler -->
                    <ul class="nav nav-tabs" role="tablist">
                        <li role="presentation" class="active">
                            <a href="#alinan-senetler" aria-controls="alinan-senetler" role="tab" data-toggle="tab">
                                <i class="fa fa-arrow-down text-success"></i> Alınan Senetler
                            </a>
                        </li>
                        <li role="presentation">
                            <a href="#verilen-senetler" aria-controls="verilen-senetler" role="tab" data-toggle="tab">
                                <i class="fa fa-arrow-up text-danger"></i> Verilen Senetler
                            </a>
                        </li>
                    </ul>

                    <!-- Sekme İçerikleri -->
                    <div class="tab-content">
                        <!-- Alınan Senetler Sekmesi -->
                        <div role="tabpanel" class="tab-pane active" id="alinan-senetler">
                            
                            <!-- Butonlar -->
                            <div style="margin-bottom: 20px;">
                                <button type="button" class="btn btn-success" onclick="AlinanSenetModal();">
                                    <i class="fa fa-plus"></i> Yeni Alınan Senet
                                </button>
                                <button type="button" class="btn btn-warning" onclick="SenetIslemModal('A');">
                                    <i class="fa fa-exchange"></i> Senet İşlemi
                                </button>
                                <asp:LinkButton ID="btnAlinanExcel" runat="server" CssClass="btn btn-info" OnClick="btnAlinanExcel_Click">
                                    <i class="fa fa-file-excel-o"></i> Excel Raporu
                                </asp:LinkButton>
                            </div>

                            <!-- Filtreler -->
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Başlangıç Tarihi:</label>
                                        <asp:TextBox ID="txtAlinanBaslangic" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label>Bitiş Tarihi:</label>
                                        <asp:TextBox ID="txtAlinanBitis" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label>Durum:</label>
                                        <asp:DropDownList ID="ddlAlinanDurum" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="" Text="Tümü"></asp:ListItem>
                                            <asp:ListItem Value="10" Text="Portföyde"></asp:ListItem>
                                            <asp:ListItem Value="11" Text="Ciro Edildi"></asp:ListItem>
                                            <asp:ListItem Value="12" Text="Tahsile Verildi"></asp:ListItem>
                                            <asp:ListItem Value="13" Text="Faktöringe Verildi"></asp:ListItem>
                                            <asp:ListItem Value="14" Text="Tahsil Edildi"></asp:ListItem>
                                            <asp:ListItem Value="15" Text="Protesto Edildi"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label>&nbsp;</label>
                                        <br />
                                        <asp:Button ID="btnAlinanFiltrele" runat="server" Text="Filtrele" CssClass="btn btn-primary" OnClick="btnAlinanFiltrele_Click" />
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label>&nbsp;</label>
                                        <br />
                                        <asp:Button ID="btnAlinanTemizle" runat="server" Text="Temizle" CssClass="btn btn-default" OnClick="btnAlinanTemizle_Click" />
                                    </div>
                                </div>
                            </div>

                            <!-- Alınan Senetler Listesi -->
                            <div class="row">
                                <asp:Repeater ID="rptAlinanSenetler" runat="server" OnItemCommand="rptAlinanSenetler_ItemCommand">
                                    <ItemTemplate>
                                        <div class="col-md-6">
                                            <div class="senet-card alinan">
                                                <div class="senet-header">
                                                    <div>
                                                        <h5>Senet No: <%# Eval("SeriNo") %></h5>
                                                        <small class="text-muted">Müşteri: <%# Eval("MusteriAdi") %></small>
                                                    </div>
                                                    <div>
                                                        <span class="senet-tipi alinan">ALINAN</span>
                                                        <span class="senet-durum <%# GetDurumClass(Eval("DurumID").ToString()) %>">
                                                            <%# GetDurumText(Eval("DurumID").ToString()) %>
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="senet-tutar alinan"><%# String.Format("{0:N2}", Eval("Tutar")) %> TL</div>
                                                        <small class="text-muted">
                                                            <strong>Borçlu:</strong> <%# Eval("Borclu") %>
                                                        </small>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <small class="text-muted">
                                                            <i class="fa fa-calendar"></i> Vade: <%# String.Format("{0:dd.MM.yyyy}", Eval("VadeTarihi")) %><br/>
                                                            <i class="fa fa-calendar"></i> Düzenleme: <%# String.Format("{0:dd.MM.yyyy}", Eval("DuzenlemeTarihi")) %>
                                                        </small>
                                                    </div>
                                                </div>
                                                <div style="margin-top: 10px;">
                                                    <asp:LinkButton ID="btnDetay" runat="server" 
                                                        CommandName="Detay" 
                                                        CommandArgument='<%# Eval("SenetID") %>'
                                                        CssClass="btn btn-xs btn-info">
                                                        <i class="fa fa-eye"></i> Detay
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnIslem" runat="server" 
                                                        CommandName="Islem" 
                                                        CommandArgument='<%# Eval("SenetID") %>'
                                                        CssClass="btn btn-xs btn-warning"
                                                        Visible='<%# Eval("DurumID").ToString() == "10" %>'>
                                                        <i class="fa fa-exchange"></i> İşlem
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnDuzenle" runat="server" 
                                                        CommandName="Duzenle" 
                                                        CommandArgument='<%# Eval("SenetID") %>'
                                                        CssClass="btn btn-xs btn-primary"
                                                        Visible='<%# Eval("DurumID").ToString() == "10" %>'>
                                                        <i class="fa fa-edit"></i> Düzenle
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnSil" runat="server" 
                                                        CommandName="Sil" 
                                                        CommandArgument='<%# Eval("SenetID") %>'
                                                        CssClass="btn btn-xs btn-danger"
                                                        OnClientClick="return confirm('Bu seneti silmek istediğinizden emin misiniz?');"
                                                        Visible='<%# Eval("DurumID").ToString() == "10" %>'>
                                                        <i class="fa fa-trash"></i> Sil
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                
                                <asp:Panel ID="pnlAlinanVeriYok" runat="server" Visible="false" CssClass="col-md-12 text-center" style="padding: 50px;">
                                    <i class="fa fa-info-circle fa-3x text-muted"></i>
                                    <h4 class="text-muted">Alınan senet bulunmuyor</h4>
                                </asp:Panel>
                            </div>
                        </div>

                        <!-- Verilen Senetler Sekmesi -->
                        <div role="tabpanel" class="tab-pane" id="verilen-senetler">
                            
                            <!-- Butonlar -->
                            <div style="margin-bottom: 20px;">
                                <button type="button" class="btn btn-danger" onclick="VerilenSenetModal();">
                                    <i class="fa fa-plus"></i> Yeni Verilen Senet
                                </button>
                                <button type="button" class="btn btn-success" onclick="SenetOdemeModal();">
                                    <i class="fa fa-check"></i> Senet Öde
                                </button>
                                <asp:LinkButton ID="btnVerilenExcel" runat="server" CssClass="btn btn-info" OnClick="btnVerilenExcel_Click">
                                    <i class="fa fa-file-excel-o"></i> Excel Raporu
                                </asp:LinkButton>
                            </div>

                            <!-- Verilen Senetler Listesi -->
                            <div class="row">
                                <asp:Repeater ID="rptVerilenSenetler" runat="server" OnItemCommand="rptVerilenSenetler_ItemCommand">
                                    <ItemTemplate>
                                        <div class="col-md-6">
                                            <div class="senet-card verilen">
                                                <div class="senet-header">
                                                    <div>
                                                        <h5>Senet No: <%# Eval("SeriNo") %></h5>
                                                        <small class="text-muted">Tedarikçi: <%# Eval("TedarikciAdi") %></small>
                                                    </div>
                                                    <div>
                                                        <span class="senet-tipi verilen">VERİLEN</span>
                                                        <span class="senet-durum <%# GetDurumClass(Eval("DurumID").ToString()) %>">
                                                            <%# GetDurumText(Eval("DurumID").ToString()) %>
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-6">
                                                        <div class="senet-tutar verilen"><%# String.Format("{0:N2}", Eval("Tutar")) %> TL</div>
                                                        <small class="text-muted">
                                                            <strong>Borçlu:</strong> <%# Eval("Borclu") %>
                                                        </small>
                                                    </div>
                                                    <div class="col-md-6">
                                                        <small class="text-muted">
                                                            <i class="fa fa-calendar"></i> Vade: <%# String.Format("{0:dd.MM.yyyy}", Eval("VadeTarihi")) %><br/>
                                                            <i class="fa fa-calendar"></i> Düzenleme: <%# String.Format("{0:dd.MM.yyyy}", Eval("DuzenlemeTarihi")) %>
                                                        </small>
                                                    </div>
                                                </div>
                                                <div style="margin-top: 10px;">
                                                    <asp:LinkButton ID="btnDetay" runat="server" 
                                                        CommandName="Detay" 
                                                        CommandArgument='<%# Eval("SenetID") %>'
                                                        CssClass="btn btn-xs btn-info">
                                                        <i class="fa fa-eye"></i> Detay
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnOde" runat="server" 
                                                        CommandName="Ode" 
                                                        CommandArgument='<%# Eval("SenetID") %>'
                                                        CssClass="btn btn-xs btn-success"
                                                        Visible='<%# Eval("DurumID").ToString() == "20" %>'>
                                                        <i class="fa fa-check"></i> Öde
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnDuzenle" runat="server" 
                                                        CommandName="Duzenle" 
                                                        CommandArgument='<%# Eval("SenetID") %>'
                                                        CssClass="btn btn-xs btn-primary"
                                                        Visible='<%# Eval("DurumID").ToString() == "20" %>'>
                                                        <i class="fa fa-edit"></i> Düzenle
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                
                                <asp:Panel ID="pnlVerilenVeriYok" runat="server" Visible="false" CssClass="col-md-12 text-center" style="padding: 50px;">
                                    <i class="fa fa-info-circle fa-3x text-muted"></i>
                                    <h4 class="text-muted">Verilen senet bulunmuyor</h4>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>

                </div>
            </section>
        </div>
    </div>

    <!-- Alınan Senet Modal -->
    <div class="modal fade" id="alinanSenetModal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Yeni Alınan Senet</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Müşteri:</label>
                                <asp:DropDownList ID="ddlSenetMusteri" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Seri No:</label>
                                <asp:TextBox ID="txtSenetSeriNo" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Tutar:</label>
                                <asp:TextBox ID="txtSenetTutar" runat="server" CssClass="form-control" placeholder="0,00"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Borçlu:</label>
                                <asp:TextBox ID="txtBorclu" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Vade Tarihi:</label>
                                <asp:TextBox ID="txtSenetVadeTarihi" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Düzenleme Tarihi:</label>
                                <asp:TextBox ID="txtSenetDuzenlemeTarihi" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Ödeme Yeri:</label>
                                <asp:TextBox ID="txtSenetOdemeYeri" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Para Birimi:</label>
                                <asp:DropDownList ID="ddlSenetParaBirimi" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label>Açıklama:</label>
                        <asp:TextBox ID="txtSenetAciklama" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" MaxLength="1000"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">İptal</button>
                    <asp:Button ID="btnSenetKaydet" runat="server" Text="Kaydet" CssClass="btn btn-success" OnClick="btnSenetKaydet_Click" />
                </div>
            </div>
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

        function closeModal(modalId) {
            $('#' + modalId).modal('hide');
        }

        function AlinanSenetModal() {
            $('#alinanSenetModal').modal('show');
        }

        function VerilenSenetModal() {
            showInfoMessage('Bilgi', 'Verilen senet ekleme özelliği yakında eklenecektir.');
        }

        function SenetIslemModal(tip) {
            showInfoMessage('Bilgi', 'Senet işlemleri özelliği yakında eklenecektir.');
        }

        function SenetOdemeModal() {
            showInfoMessage('Bilgi', 'Senet ödeme özelliği yakında eklenecektir.');
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