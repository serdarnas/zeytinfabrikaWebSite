<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Cekler.aspx.cs" Inherits="fabrika_Nakit_Cekler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    
    <style>
        .cek-card {
            border: 1px solid #ddd;
            border-radius: 8px;
            padding: 15px;
            margin-bottom: 15px;
            background: #fff;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        .cek-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 10px;
            padding-bottom: 10px;
            border-bottom: 1px solid #eee;
        }
        .cek-tutar {
            font-size: 1.3em;
            font-weight: bold;
            color: #27ae60;
        }
        .cek-durum {
            padding: 4px 8px;
            border-radius: 4px;
            font-size: 0.8em;
            color: white;
        }
        .cek-durum.portfoyde {
            background-color: #27ae60;
        }
        .cek-durum.ciro {
            background-color: #3498db;
        }
        .cek-durum.tahsil {
            background-color: #9b59b6;
        }
        .cek-durum.faktoring {
            background-color: #e67e22;
        }
        .cek-durum.tahsil-edildi {
            background-color: #95a5a6;
        }
        .cek-durum.karsilik {
            background-color: #e74c3c;
        }
        .stats-box {
            background: #f8f9fa;
            border-radius: 8px;
            padding: 20px;
            margin-bottom: 20px;
        }
        .vade-yaklasan {
            border-left: 3px solid #f39c12;
        }
        .vade-gecen {
            border-left: 3px solid #e74c3c;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <h3><i class="fa fa-file-text"></i> Çek Yönetimi</h3>
                    <div class="pull-right">
                        <a href="Default.aspx" class="btn btn-default btn-sm">
                            <i class="fa fa-arrow-left"></i> Ana Sayfa
                        </a>
                    </div>
                </header>
                <div class="panel-body">
                    
                    <!-- Çek İstatistikleri -->
                    <div class="stats-box">
                        <div class="row">
                            <div class="col-md-2">
                                <div class="text-center">
                                    <h4>Toplam Çek</h4>
                                    <h2 class="text-info"><asp:Label ID="lblToplamCek" runat="server" Text="0"></asp:Label></h2>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="text-center">
                                    <h4>Portföyde</h4>
                                    <h3 class="text-success"><asp:Label ID="lblPortfoyde" runat="server" Text="0,00"></asp:Label> TL</h3>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="text-center">
                                    <h4>Ciro Edildi</h4>
                                    <h3 class="text-primary"><asp:Label ID="lblCiroEdildi" runat="server" Text="0,00"></asp:Label> TL</h3>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="text-center">
                                    <h4>Tahsile Verildi</h4>
                                    <h3 class="text-purple"><asp:Label ID="lblTahsileVerildi" runat="server" Text="0,00"></asp:Label> TL</h3>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="text-center">
                                    <h4>Tahsil Edildi</h4>
                                    <h3 class="text-secondary"><asp:Label ID="lblTahsilEdildi" runat="server" Text="0,00"></asp:Label> TL</h3>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <div class="text-center">
                                    <h4>Karşılıksız</h4>
                                    <h3 class="text-danger"><asp:Label ID="lblKarsilik" runat="server" Text="0,00"></asp:Label> TL</h3>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Butonlar -->
                    <div style="margin-bottom: 20px;">
                        <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#yeniCekModal">
                            <i class="fa fa-plus"></i> Yeni Çek Ekle
                        </button>
                        <button type="button" class="btn btn-warning" data-toggle="modal" data-target="#cekIslemModal">
                            <i class="fa fa-exchange"></i> Çek İşlemi
                        </button>
                        <asp:LinkButton ID="btnFinansalKurumlar" runat="server" CssClass="btn btn-info" OnClick="btnFinansalKurumlar_Click">
                            <i class="fa fa-university"></i> Finansal Kurumlar
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnExcelRapor" runat="server" CssClass="btn btn-success" OnClick="btnExcelRapor_Click">
                            <i class="fa fa-file-excel-o"></i> Excel Raporu
                        </asp:LinkButton>
                    </div>

                    <!-- Filtreler -->
                    <div class="row">
                        <div class="col-md-2">
                            <div class="form-group">
                                <label>Başlangıç Tarihi:</label>
                                <asp:TextBox ID="txtBaslangicTarihi" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label>Bitiş Tarihi:</label>
                                <asp:TextBox ID="txtBitisTarihi" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label>Durum:</label>
                                <asp:DropDownList ID="ddlDurum" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="Tümü"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Portföyde"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Ciro Edildi"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Tahsile Verildi"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Faktöringe Verildi"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="Tahsil Edildi"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="Karşılıksız"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <div class="form-group">
                                <label>Müşteri:</label>
                                <asp:DropDownList ID="ddlMusteri" runat="server" CssClass="form-control">
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

                    <!-- Çekler Listesi -->
                    <div class="row">
                        <asp:Repeater ID="rptCekler" runat="server" OnItemCommand="rptCekler_ItemCommand">
                            <ItemTemplate>
                                <div class="col-md-6">
                                    <div class="cek-card <%# GetVadeDurumClass(Eval("VadeTarihi")) %>">
                                        <div class="cek-header">
                                            <div>
                                                <h5><%# Eval("SeriNo") %></h5>
                                                <small class="text-muted"><%# Eval("BankaAdi") %> - <%# Eval("SubeAdi") %></small>
                                            </div>
                                            <span class="cek-durum <%# GetDurumClass(Eval("DurumID").ToString()) %>">
                                                <%# GetDurumText(Eval("DurumID").ToString()) %>
                                            </span>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="cek-tutar"><%# String.Format("{0:N2}", Eval("Tutar")) %> TL</div>
                                                <small class="text-muted">
                                                    <strong>Kesideci:</strong> <%# Eval("Kesideci") %><br/>
                                                    <strong>Müşteri:</strong> <%# Eval("MusteriAdi") %>
                                                </small>
                                            </div>
                                            <div class="col-md-6">
                                                <small class="text-muted">
                                                    <i class="fa fa-calendar"></i> Vade: <%# String.Format("{0:dd.MM.yyyy}", Eval("VadeTarihi")) %><br/>
                                                    <i class="fa fa-calendar"></i> Alış: <%# String.Format("{0:dd.MM.yyyy}", Eval("AlisTarihi")) %>
                                                </small>
                                            </div>
                                        </div>
                                        <div style="margin-top: 10px;">
                                            <asp:LinkButton ID="btnDetay" runat="server" 
                                                CommandName="Detay" 
                                                CommandArgument='<%# Eval("CekID") %>'
                                                CssClass="btn btn-xs btn-info">
                                                <i class="fa fa-eye"></i> Detay
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnIslem" runat="server" 
                                                CommandName="Islem" 
                                                CommandArgument='<%# Eval("CekID") %>'
                                                CssClass="btn btn-xs btn-warning"
                                                Visible='<%# Eval("DurumID").ToString() == "1" %>'>
                                                <i class="fa fa-exchange"></i> İşlem Yap
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnDuzenle" runat="server" 
                                                CommandName="Duzenle" 
                                                CommandArgument='<%# Eval("CekID") %>'
                                                CssClass="btn btn-xs btn-primary"
                                                Visible='<%# Eval("DurumID").ToString() == "1" %>'>
                                                <i class="fa fa-edit"></i> Düzenle
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnSil" runat="server" 
                                                CommandName="Sil" 
                                                CommandArgument='<%# Eval("CekID") %>'
                                                CssClass="btn btn-xs btn-danger"
                                                OnClientClick="return confirm('Bu çeki silmek istediğinizden emin misiniz?');"
                                                Visible='<%# Eval("DurumID").ToString() == "1" %>'>
                                                <i class="fa fa-trash"></i> Sil
                                            </asp:LinkButton>
                                        </div>
                                        <%# !String.IsNullOrEmpty(Eval("Aciklama").ToString()) ? "<div style=\"margin-top: 5px;\"><small><strong>Açıklama:</strong> " + Eval("Aciklama") + "</small></div>" : "" %>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        
                        <asp:Panel ID="pnlVeriYok" runat="server" Visible="false" CssClass="col-md-12 text-center" style="padding: 50px;">
                            <i class="fa fa-info-circle fa-3x text-muted"></i>
                            <h4 class="text-muted">Kayıtlı çek bulunmuyor</h4>
                            <p class="text-muted">Henüz hiç çek kaydı yapılmamış veya filtrelere uygun çek bulunmuyor.</p>
                        </asp:Panel>
                    </div>

                </div>
            </section>
        </div>
    </div>

    <!-- Yeni Çek Modal -->
    <div class="modal fade" id="yeniCekModal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Yeni Çek Ekle</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Çeki Aldığımız Müşteri:</label>
                                <asp:DropDownList ID="ddlCekMusteri" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Alış Tarihi:</label>
                                <asp:TextBox ID="txtAlisTarihi" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Seri No:</label>
                                <asp:TextBox ID="txtSeriNo" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Tutar:</label>
                                <asp:TextBox ID="txtTutar" runat="server" CssClass="form-control" placeholder="0,00"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Banka Adı:</label>
                                <asp:TextBox ID="txtBankaAdi" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Şube Adı:</label>
                                <asp:TextBox ID="txtSubeAdi" runat="server" CssClass="form-control" MaxLength="150"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Hesap No:</label>
                                <asp:TextBox ID="txtHesapNo" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Kesideci:</label>
                                <asp:TextBox ID="txtKesideci" runat="server" CssClass="form-control" MaxLength="250"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Vade Tarihi:</label>
                                <asp:TextBox ID="txtVadeTarihi" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Kesim Tarihi:</label>
                                <asp:TextBox ID="txtKesimTarihi" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Ödeme Yeri:</label>
                                <asp:TextBox ID="txtOdemeYeri" runat="server" CssClass="form-control" MaxLength="100"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Para Birimi:</label>
                                <asp:DropDownList ID="ddlParaBirimi" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label>Açıklama:</label>
                        <asp:TextBox ID="txtCekAciklama" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" MaxLength="1000"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">İptal</button>
                    <asp:Button ID="btnCekKaydet" runat="server" Text="Kaydet" CssClass="btn btn-primary" OnClick="btnCekKaydet_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Çek İşlem Modal -->
    <div class="modal fade" id="cekIslemModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Çek İşlemi</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label>Çek Seçiniz:</label>
                        <asp:DropDownList ID="ddlCekSecim" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>İşlem Tipi:</label>
                        <asp:DropDownList ID="ddlIslemTipi" runat="server" CssClass="form-control" onchange="IslemTipiDegisti();">
                            <asp:ListItem Value="" Text="Seçiniz..."></asp:ListItem>
                            <asp:ListItem Value="2" Text="Tedarikçiye Ciro Et"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Bankaya Tahsile Ver"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Faktöringe Ver"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Tahsil Edildi Olarak İşaretle"></asp:ListItem>
                            <asp:ListItem Value="6" Text="Karşılıksız Olarak İşaretle"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group" id="divIlgiliTaraf">
                        <label id="lblIlgiliTaraf">İlgili Taraf:</label>
                        <asp:DropDownList ID="ddlIlgiliTarafIslem" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>Açıklama:</label>
                        <asp:TextBox ID="txtIslemAciklama" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">İptal</button>
                    <asp:Button ID="btnIslemKaydet" runat="server" Text="İşlem Yap" CssClass="btn btn-primary" OnClick="btnIslemKaydet_Click" />
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

        function IslemTipiDegisti() {
            var islemTipi = document.getElementById('<%= ddlIslemTipi.ClientID %>').value;
            var divIlgiliTaraf = document.getElementById('divIlgiliTaraf');
            var lblIlgiliTaraf = document.getElementById('lblIlgiliTaraf');
            
            if (islemTipi == '2') {
                // Tedarikçiye ciro
                divIlgiliTaraf.style.display = 'block';
                lblIlgiliTaraf.innerHTML = 'Tedarikçi:';
                // Tedarikçileri yükle
            } else if (islemTipi == '3' || islemTipi == '4') {
                // Banka/Faktöring
                divIlgiliTaraf.style.display = 'block';
                lblIlgiliTaraf.innerHTML = islemTipi == '3' ? 'Banka:' : 'Faktöring:';
                // Finansal kurumları yükle
            } else {
                divIlgiliTaraf.style.display = 'none';
            }
        }
    </script>
</asp:Content> 